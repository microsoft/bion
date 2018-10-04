using System;
using System.Runtime.Serialization;

namespace Bion
{
    public class BionSyntaxException : Exception
    {
        public BionSyntaxException(BionReader reader, BionToken expected)
            : this($"@{reader.BytesRead:n0}, expected {expected} but found {reader.TokenType}.")
        { }

        public BionSyntaxException() { }
        public BionSyntaxException(string message) : base(message) { }
        public BionSyntaxException(string message, Exception inner) : base(message, inner) { }
        protected BionSyntaxException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
