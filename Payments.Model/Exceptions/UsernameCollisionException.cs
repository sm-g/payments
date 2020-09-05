using System;
using System.Runtime.Serialization;

namespace Payments.Model
{
    public class UsernameCollisionException : Exception
    {

        public UsernameCollisionException()
        { }

        public UsernameCollisionException(string message)
            : base(message)
        { }

        public UsernameCollisionException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public UsernameCollisionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
