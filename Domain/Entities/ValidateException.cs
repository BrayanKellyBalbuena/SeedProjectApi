using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class ValidateException : Exception
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
