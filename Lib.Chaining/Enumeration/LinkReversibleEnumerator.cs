using System.Collections;
using Lib.Chaining.Chain;

namespace Lib.Chaining.Enumeration
{
    /// <summary>
    /// Reversible link enumerable
    /// </summary>
    internal class LinkReversibleEnumerator<TPayload> : IReversibleEnumerator<TPayload>
    {
        private readonly ILink<TPayload> _primer;
        private ILink<TPayload> _currentLink;

        /// <summary>
        /// Constructor
        /// </summary>
        public LinkReversibleEnumerator(ILink<TPayload> primer)
        {
            _primer = primer;
            Reset();
        }

        /// <inheritdoc />
        public bool MoveNext()
        {
            _currentLink = _currentLink is IPredecessor<TPayload> predecessor 
                ? predecessor.Next 
                : new EndLink<TPayload>(_currentLink);

            return !(_currentLink is EndLink<TPayload>);
        }

        /// <inheritdoc />
        public bool MovePrevious()
        {
            _currentLink = _currentLink is ISuccessor<TPayload> successor 
                ? successor.Previous 
                : new StartLink<TPayload>(_currentLink);

            return !(_currentLink is StartLink<TPayload>);
        }

        /// <inheritdoc />
        public void Reset()
        {
            _currentLink = new PrimerLink<TPayload>(_primer);
        }

        /// <inheritdoc />
        public TPayload Current => _currentLink.Payload;

        object IEnumerator.Current => Current;

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}
