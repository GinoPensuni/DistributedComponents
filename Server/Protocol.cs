using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRessources;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Newtonsoft.Json;

namespace Server
{
    public static class Protocol
    {
        public const string BroadcastText = "PWSP";

        public const int BroadcastPort = 10001;

        public const int BroadcastSendingInterval = 180000;

        public const int ExternalServerCommunicationPort = 10000;

        public const int ExternalKeepAliveInterval = 5000; // Milliseconds

        public const int ExternalKeepAliveTimeout = 2000; // milli

        public const int ExternalDefaultTimeout = 2000; 

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

        public static Message GetComponentMessageFromByteArray(byte[] arr)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();

            memoryStream.Write(arr, 0, arr.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);


            Message message = (Message)binForm.Deserialize(memoryStream);

            return message;
        }

        public static byte[] GetBytesFromBroadcastText()
        {
            return Encoding.UTF8.GetBytes(Protocol.BroadcastText);
        }

        public static byte[] GetBytesFromMessage(object msg, byte statusCode)
        {
            List<byte> message = new List<byte>();

            message.Add(statusCode); // logon message code

            byte[] buff;
            string serialized = JsonConvert.SerializeObject(msg);

            buff = Encoding.UTF8.GetBytes(serialized);

            message.AddRange(BitConverter.GetBytes(buff.Length));
            message.AddRange(buff);

            return message.ToArray();
        }

        public static byte[] GetBytesFromAssemblyResponse(byte[] assembly)
        {
            List<byte> message = new List<byte>();

            message.Add(6); // logon message code

            message.AddRange(BitConverter.GetBytes(assembly.Length));
            message.AddRange(assembly);

            return message.ToArray();
        }

        // msg must be a object of logonresponse or request
        public static byte[] GetBytesFromLogonMessage(object msg)
        {
            return Protocol.GetBytesFromMessage(msg, 1);
        }

        // msg must be a object of keepaliverequest or response
        public static byte[] GetBytesFromKeepAliveMessage(object msg)
        {
            return Protocol.GetBytesFromMessage(msg, 2);
        }

        public static T DeserializeStringToMessage<T>(string s)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(s);
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
