using System;
using System.Collections.Generic;
using System.Linq;
using Lib.ACL.Rule;

namespace Lib.ACL
{
    public class Ruleset
    {
        private readonly IOrderedEnumerable<IRule> _allRules;

        public Ruleset(IEnumerable<IRule> rules)
        {
            _allRules = rules.OrderBy(rule => rule.Priority);
        }

        public bool IsAuthorizedNow(ISubject subject, IObject target) 
            => IsAuthorizedAt(DateTime.Now, subject, target);

        private bool IsAuthorizedAt(DateTime when, ISubject subject, IObject @object)
        {
            var applicableRule = _allRules.FirstOrDefault(rule => rule.IsApplicableFor(subject, @object, when));
            return applicableRule?.Authorize ?? false;
        }
    }
}
