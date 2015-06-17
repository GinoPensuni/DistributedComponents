using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonInterfaces;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Client
{
    public static class Protocol
    {
        public static Component GetComponentFromMessage(byte[] msg)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();

            memoryStream.Write(msg, 0, msg.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);

            ComponentMessage message = (ComponentMessage)binForm.Deserialize(memoryStream);

            return message.Component;
        }
    }
}
