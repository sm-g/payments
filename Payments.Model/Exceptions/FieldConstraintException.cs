using System;
using System.Runtime.Serialization;

namespace Payments.Model
{
    public class FieldConstraintException : Exception
    {

        public FieldConstraintException()
        { }

        public FieldConstraintException(string message)
            : base(message)
        { }

        public FieldConstraintException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public FieldConstraintException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
