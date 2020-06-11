using Assets.Unity_Agentify_Package.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Unity_Agentify_Package.Model
{
    public interface ICommunicationLine
    {
        ICommunicationManager GetCommunicationManager();
    }
}
