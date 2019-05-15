using System;
using System.Runtime.Serialization;

namespace mhuscslib
{
    [Serializable]
    internal class AssertationException : Exception
    {
        public AssertationException()
        {
        }

        public AssertationException(string message) : base(message)
        {
        }

        public AssertationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AssertationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}