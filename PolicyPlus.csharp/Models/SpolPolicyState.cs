using System.Collections.Generic;

namespace PolicyPlus.csharp.Models
{
    public class SpolPolicyState
    {
        public string UniqueId { get; set; }
        public AdmxPolicySection Section { get; set; }
        public PolicyState BasicState { get; set; }
        public string Comment { get; set; }
        public Dictionary<string, object> ExtraOptions { get; set; } = new();

        public void Apply(IPolicySource policySource, AdmxBundle admxWorkspace, Dictionary<string, string> commentsMap)
        {
            var pol = admxWorkspace.Policies[UniqueId];
            if (commentsMap is not null && !string.IsNullOrEmpty(Comment))
            {
                commentsMap[UniqueId] = Comment;
            }

            PolicyProcessing.ForgetPolicy(policySource, pol);
            PolicyProcessing.SetPolicyState(policySource, pol, BasicState, ExtraOptions);
        }
    }
}