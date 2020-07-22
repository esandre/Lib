using System;
using System.Threading.Tasks;

namespace Lib.Patterns
{
    public class AndMaybe<TLeft, TRight>
    {
        private readonly Maybe<TLeft> _left;
        private readonly Maybe<TRight> _right;

        public AndMaybe(Maybe<TLeft> left, Maybe<TRight> right)
        {
            _left = left;
            _right = right;
        }

        public void OnlyRightIsPresent(Action<TRight> action)
        {
            _left.IfAbsent(() => _right.SelectIfHasItem(action));
        }

        public void OnlyLeftIsPresent(Action<TLeft> action)
        {
            _right.IfAbsent(() => _left.SelectIfHasItem(action));
        }

        public void BothAreAbsent(Action action)
        {
            _right.IfAbsent(() => _left.IfAbsent(action));
        }

        public async Task BothArePresentAsync(Func<TLeft, TRight, Task> action)
        {
            await _right.SelectIfHasItemAsync(
                async right => await _left.SelectIfHasItemAsync(
                    async left => await action(left, right)));
        }
    }
}
