using System.Text.RegularExpressions;

namespace DotEngine.Config.WDB
{
    public class WDBFieldValidation : WDBValidation
    {
        private const string FIELD_NAME_REGEX = @"^[A-Za-z][A-Za-z0-9]{2,9}";

        protected WDBField GetField(WDBContext context)
        {
            return context.Get<WDBField>(WDBContextKey.CURRENT_FIELD_NAME);
        }

        public override void Verify(WDBContext context)
        {
            WDBField field = GetField(context);
            if(string.IsNullOrEmpty(field.Name))
            {
                AddErrorMessage(context,string.Format(WDBErrorMessages.FIELD_NAME_EMPTY_ERROR,field.Column));
            }
            else
            {
                if(!Regex.IsMatch(field.Name,FIELD_NAME_REGEX))
                {
                    AddErrorMessage(context, string.Format(WDBErrorMessages.FIELD_NAME_REGEX_ERROR, field.Name, field.Column));
                }
            }

            string defaultContent = field.GetContent();
            if(string.IsNullOrEmpty(defaultContent))
            {
                AddErrorMessage(context, string.Format(WDBErrorMessages.FIELD_CONTENT_EMPTY_ERROR, field.Column));
            }
        }
    }
}
