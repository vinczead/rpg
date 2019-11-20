using System;
using System.Runtime.Serialization;

namespace GameScript.SymbolTable
{
    [Serializable]
    internal class NameAlreadyDefinedException : Exception
    {
        public NameAlreadyDefinedException()
        {
        }

        public NameAlreadyDefinedException(string message) : base(message)
        {
        }

        public NameAlreadyDefinedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NameAlreadyDefinedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}