using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRessources
{
    public interface IStore
    {
        byte[] this[Guid AssemblyID]
        {
            get;
        }

        byte[] this[int hash]
        {
            get;
        }
        
        bool Store(byte[] assemblyCode);

        List<byte[]> LoadAssemblies();

    }
}
