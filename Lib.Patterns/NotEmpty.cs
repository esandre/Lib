using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lib.Patterns
{
    public class NotEmpty<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _input;

        public NotEmpty(IEnumerable<T> input)
        {
            // ReSharper disable PossibleMultipleEnumeration
            if(!input.Any()) throw new ArgumentException("Enumerable is empty", nameof(input));
            _input = input;
        }

        public IEnumerator<T> GetEnumerator() => _input.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) _input).GetEnumerator();
    }
}
