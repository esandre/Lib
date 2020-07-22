using System;

namespace Lib.Patterns
{
    public class BrothersEither<TParent, TLeft, TRight> 
        : Either<TLeft, TRight> 
        where TLeft : TParent where TRight : TParent
    {
        public BrothersEither(TLeft value) : base(value)
        {
        }

        public BrothersEither(TRight value) : base(value)
        {
        }

        public void Select(Action<TParent> parentSelector) 
            => Select(left => parentSelector(left), right => parentSelector(right));

        public TReturn Select<TReturn>(Func<TParent, TReturn> parentSelector)
        {
            TReturn result = default;
            Select((Action<TParent>) (element => result = parentSelector(element)));
            return result;
        }
    }
}
