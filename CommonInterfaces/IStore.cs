using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonInterfaces
{
    public interface IStore
    {
        Task<bool> Store(byte[] assemblyCode);
    }
}
