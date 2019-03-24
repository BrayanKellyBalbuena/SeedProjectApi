using System.Collections.Generic;

namespace BusinessLogic.Exception
{
    public class ValidateException : System.Exception
    {
        public ValidateException() : base() { }
        public ValidateException(string message) : base(message) { }

        public ValidateException(string message, string code) : base(message)
        {
            this.Code = code;
        }

        public IEnumerable<string> Errors { get; set; }
        public string Code { get; set; }
    }
}
