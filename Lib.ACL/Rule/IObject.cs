using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.ACL.Rule
{
    public interface IObject  
    {
        string UniqueId { get; }
        Task<IEnumerable<IRule>> GetApplyingRulesAsync();
    }
}
