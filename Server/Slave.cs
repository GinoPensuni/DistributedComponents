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
            this.IsAssigned = false;
            this.IsAlive = true;
            this.StartListening();
            Thread aliveStatusThread = new Thread(new ThreadStart(this.CheckAliveStatus));
            aliveStatusThread.Start();
        }

        public bool IsAssigned { get; private set; }

        public bool IsAlive { get; private set; }

        public TcpClient Client { get; private set; }

        public NetworkStream ClientStream { get; private set; }

        public event EventHandler<MessageReceivedEventArgs> OnMessageReceived;

        public event EventHandler<SlaveDiedEventArgs> OnSlaveDied;

        public bool IsListening { get; private set; }

        public List<Message> UnconfirmedMessages { get; private set; }

        public bool SendMessage(Message msg)
        {
            byte[] message = Protocol.GetByteArrayFromMessage(msg);
            this.UnconfirmedMessages.Add(msg);

            try
            {
                this.ClientStream.Write(message, 0, message.Length);
                Console.WriteLine("Message sent");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SendComponent(Component comp, IEnumerable<object> values)
        {
            ComponentMessage compmsg = new ComponentMessage(Guid.NewGuid());
            compmsg.Component = comp;
            compmsg.Values = values;
            Console.Write("Request Component id: ");
            Console.WriteLine(compmsg.ID);
            return this.SendMessage(compmsg);
        }

        public void AssignGuid(Guid guid)
        {
            this.clientGuid = guid;
            AssignMessage assignmsg = new AssignMessage(Guid.NewGuid());
            assignmsg.ClientGuid = this.clientGuid;
            this.SendMessage(assignmsg);
        }

        private void CheckAliveStatus()
        {
            Console.WriteLine("checking alive...");
            Thread.Sleep(2000); //30000
            AliveMessage alivemsg = new AliveMessage(Guid.NewGuid());
            this.SendMessage(alivemsg);
            Thread.Sleep(3000);

            if (this.ConfirmMessage(alivemsg.ID))
            {
                this.IsAlive = false;
                this.StopListening();
                if (this.OnSlaveDied != null)
                {
                    this.OnSlaveDied(this, new SlaveDiedEventArgs());
                }
            }

            //Message msg = this.SearchForMessage(alivemsg.ID);
            //if (msg != null)
            //{
            //    this.IsAlive = false;
            //    this.UnconfirmedMessages.Remove(msg);
            //}
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

                    if (msg is AssignMessage)
                    {
                        if (this.ConfirmMessage(msg.ID))
                        {
                            this.IsAssigned = true;
                        }
                    }
                    else if (msg is AliveMessage)
                    {
                        if (this.ConfirmMessage(msg.ID))
                        {
                            Thread aliveStatusThread = new Thread(new ThreadStart(this.CheckAliveStatus));
                            aliveStatusThread.Start();
                        }
                    }
                    else if (msg is ResultMessage)
                    {
                        this.ConfirmMessage(msg.ID);
                    }

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

            this.ClientStream.Close();
            this.Client.Close();
        }

        public bool ConfirmMessage(Guid guid)
        {
            Message found = this.SearchForMessage(guid);

            if (found != null)
            {
                return this.UnconfirmedMessages.Remove(found);
            }

            return false;
        }

        public void StopListening()
        {
            this.IsListening = false;
        }
    }

}
