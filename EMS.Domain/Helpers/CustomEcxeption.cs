using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Domain.Helpers
{
    public class BusinessRuleException : Exception
    {
        // Default constructor
        public BusinessRuleException()
        {
        }

        // Constructor that accepts a custom message
        public BusinessRuleException(string message)
            : base(message)
        {
        }

        // Constructor that accepts a custom message and an inner exception
        // The inner exception is used to store the original exception that caused this exception
        public BusinessRuleException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
