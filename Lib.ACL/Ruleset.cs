using System;
using System.Collections.Generic;
using System.Linq;
using Lib.ACL.Rule;
using Lib.Patterns;

namespace Lib.ACL
{
    public class Ruleset
    {
        private readonly IOrderedEnumerable<IRule> _allRules;

        public Ruleset(IEnumerable<IRule> rules)
        {
            _allRules = rules.OrderBy(rule => rule.Priority);
        }

        public (bool? Issue, Maybe<IRule> Raison) IsAuthorizedNow(ISubject subject, IObject target) 
            => IsAuthorizedAt(DateTime.Now, subject, target);
        
        private (bool? Issue, Maybe<IRule> Raison) IsAuthorizedAt(DateTime when, ISubject subject, IObject @object)
        {
            var applicableRule = _allRules.FirstOrDefault(rule => rule.IsApplicableFor(subject, @object, when));
            if (applicableRule is null) return (null, new Maybe<IRule>());

            return (applicableRule.Authorize, new Maybe<IRule>(applicableRule));
        }
    }
}
