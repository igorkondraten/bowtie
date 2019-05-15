using System;

namespace BowTie.BLL.Exceptions
{
    /// <summary>
    /// The exception is thrown when received data don't pass business logic validation.
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException() { }
        public ValidationException(string message) : base(message) { }
        public ValidationException(string message, Exception inner) : base(message, inner) { }
        public override string ToString() { return Message; }
    }
}
