using System;
using Lib.Chaining.Enumeration;
using NFluent;

namespace Lib.Chaining.Test.Fixtures
{
    public interface IReversibleEnumeratorImplicitBehaviorFixture : IEnumeratorImplicitBehaviorFixture
    {
        void PreviousReturnsFalse_AtTheBeginning();
        void PreviousNeverThrows_BeforeTheBeginning();

        void NextAndPrevious_Commutative();
    }

    public class ReversibleEnumeratorImplicitBehaviorFixtures<TEnumerator> : EnumeratorImplicitBehaviorFixture<TEnumerator>, IReversibleEnumeratorImplicitBehaviorFixture
    {
        private readonly Func<IReversibleEnumerator<TEnumerator>> _enumeratorFactory;
        private readonly byte _elementsNumberBackwards;

        public ReversibleEnumeratorImplicitBehaviorFixtures(
            Func<IReversibleEnumerator<TEnumerator>> enumeratorFactory, 
            byte elementsNumberForward, 
            byte elementsNumberBackwards) 
            : base(enumeratorFactory, elementsNumberForward)
        {
            _enumeratorFactory = enumeratorFactory;
            _elementsNumberBackwards = elementsNumberBackwards;
        }

        public void PreviousReturnsFalse_AtTheBeginning()
        {
            using (var enumerator = _enumeratorFactory())
            {
                Check.That(enumerator.Current).IsDefaultValue();
                var latest = default(TEnumerator);

                for (var i = 0; i < _elementsNumberBackwards; i++)
                {
                    Check.That(enumerator.Current).Equals(latest);
                    Check.That(enumerator.MovePrevious()).IsTrue();
                    latest = enumerator.Current;
                }

                Check.That(enumerator.Current).Equals(latest);

                Check.That(enumerator.MovePrevious()).IsFalse();
                Check.That(enumerator.Current).IsDefaultValue();
            }
        }

        public void PreviousNeverThrows_BeforeTheBeginning()
        {
            using (var enumerator = _enumeratorFactory())
            {
                for (var i = 0; i < _elementsNumberBackwards; i++)
                {
                    Check.That(enumerator.MovePrevious()).IsTrue();
                }

                Check.That(enumerator.MovePrevious()).IsFalse();
                Check.That(enumerator.MovePrevious()).IsFalse();
            }
        }

        public void NextAndPrevious_Commutative()
        {
            using (var enumerator = _enumeratorFactory())
            {
                //Passing primer, which is not commutative
                Check.That(enumerator.MoveNext()).IsTrue();

                var value = enumerator.Current;
                Check.That(enumerator.MoveNext()).IsTrue();
                Check.That(enumerator.MovePrevious()).IsTrue();
                Check.That(enumerator.Current).Equals(value);
            }

            using (var enumerator = _enumeratorFactory())
            {
                //Passing primer, which is not commutative
                Check.That(enumerator.MovePrevious()).IsTrue();

                var value = enumerator.Current;
                Check.That(enumerator.MovePrevious()).IsTrue();
                Check.That(enumerator.MoveNext()).IsTrue();
                Check.That(enumerator.Current).Equals(value);
            }
        }
    }
}
