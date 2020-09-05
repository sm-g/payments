using System;
using System.Runtime.Serialization;

namespace Payments.Model
{
    public class PasswordStrengthException : Exception
    {

        public PasswordStrengthException()
        { }

        public PasswordStrengthException(string message)
            : base(message)
        { }

        public PasswordStrengthException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public PasswordStrengthException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
