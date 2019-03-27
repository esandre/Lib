using System;
using System.Collections.Generic;
using NFluent;

namespace Lib.Chaining.Test.Fixtures
{
    public interface IEnumeratorImplicitBehaviorFixture
    {
        void Current_Initially_ReturnsDefault();
        void Reset_SetsCurrent_ToDefault();
        void NextReturnsFalse_AtTheEnd();
        void NextNeverThrows_AfterTheEnd();
    }

    public class EnumeratorImplicitBehaviorFixture<TEnumerator> : IEnumeratorImplicitBehaviorFixture
    {
        private readonly Func<IEnumerator<TEnumerator>> _enumeratorFactory;
        private readonly byte _elementsNumber;

        public EnumeratorImplicitBehaviorFixture(Func<IEnumerator<TEnumerator>> enumeratorFactory, byte elementsNumber)
        {
            _enumeratorFactory = enumeratorFactory;
            _elementsNumber = elementsNumber;
        }

        public void Current_Initially_ReturnsDefault()
        {
            using (var enumerator = _enumeratorFactory())
                Check.That(enumerator.Current).IsDefaultValue();
        }

        public void Reset_SetsCurrent_ToDefault()
        {
            using (var enumerator = _enumeratorFactory())
            {
                enumerator.MoveNext();
                enumerator.MoveNext();

                enumerator.Reset();

                Check.That(enumerator.Current).IsDefaultValue();
            }
        }

        public void NextReturnsFalse_AtTheEnd()
        {
            using (var enumerator = _enumeratorFactory())
            {
                Check.That(enumerator.Current).IsDefaultValue();
                var latest = default(TEnumerator);

                for (var i = 0; i < _elementsNumber ; i++)
                {
                    Check.That(enumerator.Current).Equals(latest);
                    Check.That(enumerator.MoveNext()).IsTrue();
                    latest = enumerator.Current;
                }

                Check.That(enumerator.Current).Equals(latest);

                Check.That(enumerator.MoveNext()).IsFalse();
                Check.That(enumerator.Current).IsDefaultValue();
            }
        }

        public void NextNeverThrows_AfterTheEnd()
        {
            using (var enumerator = _enumeratorFactory())
            {
                for (var i = 0; i < _elementsNumber; i++)
                {
                    Check.That(enumerator.MoveNext()).IsTrue();
                }

                Check.That(enumerator.MoveNext()).IsFalse();
                Check.That(enumerator.MoveNext()).IsFalse();
            }
        }
    }
}
