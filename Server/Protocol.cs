using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonInterfaces;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Server
{
    public static class Protocol
    {
        public static byte[] GetByteArrayFromMessage(Message msg)
        {
            List<byte> message = new List<byte>();
            BinaryFormatter bf = new BinaryFormatter();

            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, msg);
                byte[] bytes = ms.ToArray();
                message.AddRange(BitConverter.GetBytes(bytes.Length));
                message.AddRange(bytes);

                return message.ToArray();
            }
        }
    }
}
