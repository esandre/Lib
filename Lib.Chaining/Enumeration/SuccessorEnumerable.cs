using System.Collections;
using System.Collections.Generic;
using Lib.Chaining.Chain;

namespace Lib.Chaining.Enumeration
{
    /// <summary>
    /// Enumerates a Link succession while they are Successors
    /// </summary>
    /// <typeparam name="TPayload"></typeparam>
    internal class SuccessorEnumerable<TPayload> : IEnumerable<TPayload>
    {
        private readonly ISuccessor<TPayload> _primer;

        /// <summary>
        /// Constructor
        /// </summary>
        public SuccessorEnumerable(ISuccessor<TPayload> primer)
        {
            _primer = primer;
        }

        /// <inheritdoc />
        public IEnumerator<TPayload> GetEnumerator() => new SuccessorEnumerator<TPayload>(_primer);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
