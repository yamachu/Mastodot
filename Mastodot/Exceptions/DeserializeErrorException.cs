using System;
using Mastodot.Entities;

namespace Mastodot.Exceptions
{
    // Will wrapped AggregateExcepton
    public class DeserializeErrorException: Exception
    {
        public Error Error { get; set; }

        public DeserializeErrorException(string message)
            : base(message)
        {
        }

        public DeserializeErrorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
