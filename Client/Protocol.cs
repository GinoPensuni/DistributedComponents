﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRessources;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Client
{
    public static class Protocol
    {
        /*public static Component GetComponentFromMessage(byte[] msg)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();

            memoryStream.Write(msg, 0, msg.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);

            ComponentMessage message = (ComponentMessage)binForm.Deserialize(memoryStream);

            return message.Component;
        }*/

        public static Message GetComponentMessageFromByteArray(byte[] arr)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();

            memoryStream.Write(arr, 0, arr.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);

            
            Message message = (Message)binForm.Deserialize(memoryStream);

            return message;
        }

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
