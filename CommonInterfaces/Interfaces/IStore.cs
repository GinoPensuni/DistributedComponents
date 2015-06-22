using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRessources
{
    public interface IStore
    {
        Tuple<byte[], bool> this[Guid AssemblyID]
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
