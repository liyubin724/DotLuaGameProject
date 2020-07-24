namespace DotTool.ETD.Validation
{
    public class ErrorValidation : IValidation
    {
        public string Rule { get; set; }

        public ValidationResult Verify()
        {
            throw new System.NotImplementedException();
        }
    }
}
