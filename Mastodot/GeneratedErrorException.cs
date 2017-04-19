using System;
using Mastodot.Entities;

namespace Mastodot
{
    // Will wrapped AggregateExcepton
    public class GeneratedErrorException: Exception
    {
        public Error Error { get; set; }
    }
}
