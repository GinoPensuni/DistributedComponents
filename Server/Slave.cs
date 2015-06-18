using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using CommonInterfaces;
using System.Threading;

namespace Server
{
    public class Slave : CommonClient
    {
        public Slave(TcpClient client)
        {
            this.UnconfirmedMessages = new List<Message>();
            this.Client = client;
            this.ClientStream = this.Client.GetStream();
            this.StartListening();
        }

        public TcpClient Client { get; private set; }

        public NetworkStream ClientStream { get; private set; }

        public event EventHandler<MessageReceivedEventArgs> OnMessageReceived;

        public bool IsListening { get; private set; }

        public List<Message> UnconfirmedMessages { get; private set; }

        public void SendMessage(Message msg)
        {
            byte[] message = Protocol.GetByteArrayFromMessage(msg);
            this.UnconfirmedMessages.Add(msg);
            this.ClientStream.Write(message, 0, message.Length);
            Console.WriteLine("Message sent");
        }

        public bool AssignGuid(Guid guid)
        {
            this.clientGuid = guid;
            AssignMessage assignmsg = new AssignMessage(Guid.NewGuid());
            assignmsg.ClientGuid = this.clientGuid;
            this.SendMessage(assignmsg);
            Thread.Sleep(3000);

            if (this.SearchForMessage(assignmsg.ID) == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Message SearchForMessage(Guid id)
        {
            foreach (Message msg in this.UnconfirmedMessages)
            {
                if (msg.ID.Equals(id))
                {
                    return msg;
                }
            }

            return null;
        }

        public void StartListening()
        {
            this.IsListening = true;
            Thread listenerThread = new Thread(new ThreadStart(CommunicationHandler));
            listenerThread.Start();
        }

        private void CommunicationHandler()
        {
            while (this.IsListening)
            {
                if (this.ClientStream.DataAvailable)
                {
                    byte[] length = new byte[4];
                    this.ClientStream.Read(length, 0, length.Length);

                    byte[] messageBytes = new byte[BitConverter.ToInt32(length, 0)];
                    this.ClientStream.Read(messageBytes, 0, messageBytes.Length);

                    Message msg = Protocol.GetComponentMessageFromByteArray(messageBytes);
                    Console.WriteLine("Message received");
                    if (this.OnMessageReceived != null)
                    {
                        MessageReceivedEventArgs e = new MessageReceivedEventArgs(msg);
                        this.OnMessageReceived(this, e);
                    }
                }
                else
                {
                    Console.WriteLine("waiting...");
                    Thread.Sleep(1000);
                }
            }
        }

        public void StopListening()
        {
            this.IsListening = false;
        }
    }

}
