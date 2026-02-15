using System;

namespace VaultFiles.Exceptions
{
    /// <summary>
    /// Exception thrown when a file with a disallowed extension is provided.
    /// </summary>
    public class InvalidFileTypeException : Exception
    {
        public InvalidFileTypeException() { }

        public InvalidFileTypeException(string message) : base(message) { }

        public InvalidFileTypeException(string message, Exception innerException) : base(message, innerException) { }
    }
}