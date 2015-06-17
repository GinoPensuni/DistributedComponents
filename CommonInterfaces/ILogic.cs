using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonInterfaces
{
    public interface ILogic
    {
        bool DisconnectFromServer();

        void SaveComponent(IComponent component);

        void ConnenctToServer();
    }
    
}
