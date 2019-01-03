using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipDemoMVVM.Model
{
    /// <summary>
    /// Class for Messenger which sends object to receiver with 
    /// contained data for process.
    /// </summary>
    public class MessageMemberModel
    {
        // Denotes type of process that is intended.
        public string Selection { get; private set; }

        // Membership that needs to be processed.
        public MembershipModel Membership { get; private set; }

        // Default constructor not needed.
        private MessageMemberModel() { }

        // Parametrized constructor which is used to instantiate this object.
        public MessageMemberModel(MembershipModel mem, string sel)
        {
            Membership = mem;
            Selection = sel;
        }
    }
}
