using System;
using System.Runtime.Serialization;

namespace Payments.Model
{
    public class NoUsernameException : Exception
    {

        public NoUsernameException()
        { }

        public NoUsernameException(string message)
            : base(message)
        { }

        public NoUsernameException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public NoUsernameException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
