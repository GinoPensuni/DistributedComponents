using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonInterfaces
{
    public interface ILogic
    {
        Task<bool> DisconnectFromServer();

        Task SaveComponent(IComponent component);

        void ConnenctToServer();

        Task<List<Tuple<Type, IComponent>>> LoadComponents();
    }
    
}
