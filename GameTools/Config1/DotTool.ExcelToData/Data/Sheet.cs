using DotEngine.Context;
using DotTool.ETD.Log;
using DotTool.ETD.Validation;
using DotTool.ETD.Verify;
using System.Collections.Generic;
using System.Text;

namespace DotTool.ETD.Data
{
    public class Sheet : IVerify
    {
        private string name;

        private List<Field> fields = new List<Field>();
        private List<Line> lines = new List<Line>();

        public List<Field> Fields { get => fields; }

        public string Name { get => name; }

        public Sheet(string n)
        {
            name = n;
        }

        public int LineCount { get => lines.Count; }
        public int FieldCount { get => fields.Count; }

        public void AddField(Field field)
        {
            fields.Add(field);
        }

        public Field GetFieldByCol(int col)
        {
            foreach(var field in fields)
            {
                if(field.Col == col)
                {
                    return field;
                }
            }
            return null;
        }

        public Field GetFieldByIndex(int index)
        {
            if(index>=0 && index < fields.Count)
            {
                return fields[index];
            }
            return null;
        }

        public void AddLine(Line line)
        {
            lines.Add(line);
        }

        public Line GetLineByRow(int row)
        {
            foreach(var line in lines)
            {
                if(line.Row == row)
                {
                    return line;
                }
            }
            return null;
        }

        public Line GetLineByIndex(int index)
        {
            if(index>=0&&index<lines.Count)
            {
                return lines[index];
            }
            return null;
        }

        public string GetLineIDByRow(int row)
        {
            Line line = GetLineByRow(row);
            for(int i =0;i<FieldCount;++i)
            {
                Field field = GetFieldByIndex(i);
                if(field.FieldType == FieldType.Id)
                {
                    return line.GetCellByIndex(i).GetContent(field);
                }
            }
            return null;
        }

        public string GetLineIDByIndex(int index)
        {
            Line line = GetLineByIndex(index);
            for (int i = 0; i < FieldCount; ++i)
            {
                Field field = GetFieldByIndex(i);
                if (field.FieldType == FieldType.Id)
                {
                    return line.GetCellByIndex(i).GetContent(field);
                }
            }
            return null;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(name);
            sb.AppendLine("-------------------------------------------");
            foreach(var field in fields)
            {
                sb.Append(field.ToString());
            }
            sb.AppendLine();
            sb.AppendLine("------------------------------------------");
            foreach(var line in lines)
            {
                sb.AppendLine(line.ToString());
            }
            sb.AppendLine();
            sb.AppendLine();
            return sb.ToString();
        }

        public bool Verify(TypeContext context)
        {
            LogHandler logHandler = context.Get<LogHandler>(typeof(LogHandler));

            logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_VERIFY_START, name);

            if (FieldCount == 0)
            {
                logHandler.Log(LogType.Error, LogMessage.LOG_SHEET_FIELD_EMPTY);
                return false;
            }

            Field idField = fields[0];
            if(idField.FieldType != FieldType.Id)
            {
                return false;
            }

            context.Add(typeof(Sheet), this);

            bool result = true;
            foreach(var field in fields)
            {
                if(!field.Verify(context))
                {
                    result = false;
                }
            }

            if (result)
            {
                foreach (var line in lines)
                {
                    logHandler.Log(LogType.Info, LogMessage.LOG_LINE_VERIFY_START, line.Row);

                    if(line.Count != fields.Count)
                    {
                        logHandler.Log(LogType.Error, LogMessage.LOG_LINE_COUNT_NOT_EQUAL);
                        result = false;
                    }else
                    {
                        for (int i = 0; i < fields.Count; ++i)
                        {
                            Field field = fields[i];
                            Cell cell = line.GetCellByIndex(i);

                            context.Add(typeof(Field), field);
                            context.Add(typeof(Cell), cell);

                            IValidation[] validations = field.GetValidations();
                            if(validations!=null && validations.Length>0)
                            {
                                foreach(var validation in validations)
                                {
                                    if(validation.GetType() != typeof(ErrorValidation))
                                    {
                                        context.Inject(validation);

                                        ValidationResult resultCode = validation.Verify();
                                        if (resultCode != ValidationResult.Success)
                                        {
                                            result = false;
                                        }
                                    }else
                                    {
                                        result = false;
                                    }
                                }
                            }

                            context.Remove(typeof(Field));
                            context.Remove(typeof(Cell));

                        }
                    }

                    logHandler.Log(LogType.Info, LogMessage.LOG_LINE_VERIFY_END, result);
                }

                context.Remove(typeof(Sheet));
            }

            logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_VERIFY_END, name, result);

            return result;
        }
    }
}
