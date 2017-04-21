using System;
using Mastodot.Entities;

namespace Mastodot
{
    // Will wrapped AggregateExcepton
    public class DeserializeErrorException: Exception
    {
        public Error Error { get; set; }

        public DeserializeErrorException(string message)
            : base(message)
        {
        }
    }
}
