using System;
using System.Collections.Generic;
using System.Linq;
using Lib.ACL.Rule;

namespace Lib.ACL
{
    public class Ruleset
    {
        public IList<IRule> Rules => _allRules.Values.Aggregate((c, e) => c.Concat(e).ToList());

        private readonly IDictionary<int, IList<IRule>> _allRules = new Dictionary<int, IList<IRule>>();

        public void Add(IRule rule)
        {
            if (!_allRules.ContainsKey(rule.Priority)) 
				_allRules.Add(rule.Priority, new List<IRule>());
            _allRules[rule.Priority].Add(rule);
        }

        public void Remove(IRule rule)
        {
            if (!_allRules.ContainsKey(rule.Priority)) 
				return;
            _allRules[rule.Priority].Remove(rule);
        }

        public bool IsAuthorizedNow(ISubject subject, IObject target)
        {
            return IsAuthorizedAt(DateTime.Now, subject, target);
        }

        public bool IsAuthorizedAt(DateTime when, ISubject subject, IObject target)
        {
			// Vérification de règles existantes.
            if (_allRules.Keys.Count == 0)             	
                return subject.AuthorizedByDefault; 
			
			bool regleTrouvee;
			bool regleVoulue;

			if (subject.AuthorizedByDefault)
				regleVoulue = false;
			else
				regleVoulue = true;

			regleTrouvee = IndividualRule (when, subject, target, regleVoulue);

			if (regleTrouvee)
				return regleVoulue;
			else
			{
				if (target.Groups.Count () >= 1) {
					regleTrouvee = GroupsDoorsRule (when, subject, target, regleVoulue);

					if (regleTrouvee)
						return regleVoulue;
				}

				if (subject.Groups.Count () >= 1) {
					regleTrouvee = GroupsUsersRule (when, subject, target, regleVoulue);

					if (regleTrouvee)
						return regleVoulue;
				}

				if (subject.Groups.Count () >= 1 && target.Groups.Count () >= 1) {
					regleTrouvee = GroupsUsersDoorsRule (when, subject, target, regleVoulue);

					if (regleTrouvee)
						return regleVoulue;
				}
				return subject.AuthorizedByDefault;
			}
			
        }

        private static bool? ApplyRules(IEnumerable<IRule> rules, IRightBearer subject, IRuleTarget target, DateTime time)
        {
			// On récupère et on ajoute dans une liste les règles enregistrés qui on comme paramètre : 
			// - le lecteur devant lequel un badge a été passé (target)
			// - la personne intérogé (subject)
			// - la date et l'heure de la comparaison (time)
			var applicableRules = rules.Where(rule => rule.Objet.Equals(target)
                                                   && rule.Subject.Equals(subject)
                                                   && rule.TimeSlot.Contains(time)).ToList();

			if (!applicableRules.Any())				
				return null;

			return applicableRules.OrderByDescending(rule => rule.CreationDate).First().Authorize;
        }

		private bool CheckRule (DateTime when, IRightBearer subject, IRuleTarget target, bool ruleWanted)
		{
			return _allRules
				.OrderBy (kv => kv.Key)
				.Any (ruleList => ApplyRules (ruleList.Value, subject, target, when) == ruleWanted);
		}

		private bool IndividualRule (DateTime when, ISubject subject, IObject target, bool ruleWanted)
		{
			return CheckRule (when, subject, target, ruleWanted);			
		}

		private bool GroupsDoorsRule (DateTime when, ISubject subject, IObject target, bool ruleWanted)
        {
            return target.Groups.Select(groupePorte => CheckRule(when, subject, groupePorte, ruleWanted)).Any(ruleFounded => ruleFounded);
        }

		private bool GroupsUsersRule (DateTime when, ISubject subject, IObject target, bool ruleWanted)
		{
            foreach (ISubjectGroup groupeUtilisateur in subject.Groups)
            {
                var ruleFounded = CheckRule (when, groupeUtilisateur, target, ruleWanted);
                if (ruleFounded)
					return true;
            }
			return false;
		}

		private bool GroupsUsersDoorsRule (DateTime when, ISubject subject, IObject target, bool ruleWanted)
		{
            foreach (IObjectGroup groupePorte in target.Groups) 
			{
				foreach (ISubjectGroup groupeUtilisateur in subject.Groups)
                {
                    var ruleFounded = CheckRule (when, groupeUtilisateur, groupePorte, ruleWanted);
                    if (ruleFounded)
						return true;
                }
			}
			return false;
		}
    }
}
