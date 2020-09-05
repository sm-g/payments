using System;
using System.Runtime.Serialization;

namespace Payments.Model
{
    public class WrongPasswordException : Exception
    {

        public WrongPasswordException()
        { }

        public WrongPasswordException(string message)
            : base(message)
        { }

        public WrongPasswordException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public WrongPasswordException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
