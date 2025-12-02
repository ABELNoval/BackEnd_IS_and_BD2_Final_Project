using System;
using System.Collections.Generic;

namespace Application.Exceptions
{
    /// <summary>
    /// Exception thrown when DTO validation fails using FluentValidation
    /// </summary>
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException() 
            : base("One or more validation errors occurred")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(string message) 
            : base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(string message, Exception innerException) 
            : base(message, innerException)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IDictionary<string, string[]> errors) 
            : this()
        {
            Errors = errors;
        }

        public ValidationException(string propertyName, string errorMessage)
            : this()
        {
            Errors.Add(propertyName, new[] { errorMessage });
        }
    }
}