using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Network;
using Core.Component;
using Newtonsoft.Json;

namespace Server
{
    public class ExternalServer : CommonServer
    {
        private CommonServer masterServer;

        // fucking threads
        private object locker = new object();

        public ExternalServer(IPAddress address, CommonServer masterServer)
        {
            this.Address = address;
            this.masterServer = masterServer;

            this.IsAlive = true;
        }

        public bool IsLoggedOn { get; private set; }

        public bool IsAlive { get; private set; }

        public IPAddress Address { get; private set; }

        // 1. events for logon handling

        public event EventHandler<ExternalServerLogonCompletedEventArgs> OnLogonCompleted;

        // 2. events for keep alive status checking

        public event EventHandler<ExternalServerDiedEventArgs> OnTerminated;

        // 3. events for component submitting

        /// <summary>
        /// Gets called when the external server sends a respond to our component submit request.
        /// </summary>
        public event EventHandler<InternalComponentSubmitResponseEventArgs> OnInternalComponentSubmitResponseReceived;

        /// <summary>
        /// The external component must be accepted by setting the property IsAccepted of the event args e.
        /// </summary>
        public event EventHandler<ExternalComponentSubmittedEventArgs> OnExternalComponentSubmitRequestReceived;

        // 4. events for job request and response handling

        /// <summary>
        /// The job request must be accepted by setting the property IsAccepted of the event args e.
        /// </summary>
        public event EventHandler<ExternalJobRequestEventArgs> OnJobRequestReceived;

        public event EventHandler<ExternalJobResponseEventArgs> OnJobResponseReceived;

        // 5. events for job result requeset and response handling

        public event EventHandler<ExternalJobResultRequestEventArgs> OnJobResultRequestReceived;

        public event EventHandler<ExternalJobResultResponseEventArgs> OnJobResultResponseReceived;

        // 6. events for assembly request handling

        /// <summary>
        /// The binary content must be delivered by setting the property BinaryContent of the event args e.
        /// </summary>
        public event EventHandler<ExternalServerAssemblyRequestedEventArgs> OnAssemblyRequested;

        public event EventHandler<ExternalServerAssemblyRequestedEventArgs> OnAssemblyReceived;

        // 7. events for client update handling. 

        public event EventHandler<ExternalClientUpdateRequestEventArgs> OnExternalClientUpdated;

        /// <summary>
        /// Gets called when the external server sends a respond to our client update request.
        /// </summary>
        public event EventHandler<InternalClientUpdateResponseEventArgs> OnInternalClientUpdatedResponseReceived;

        // ----------------------------------------------

        public TcpClient GetConnection()
        {
            try
            {
                IPEndPoint ip = new IPEndPoint(Address, Protocol.ExternalServerCommunicationPort);

                TcpClient client = new TcpClient();

                client.Connect(ip);

                return client;
            }
            catch (Exception)
            {
                this.TerminateExternalServer();

                return null;
            }
        }

        public void Logon(Guid id, string friendlyName, List<Component> availableComponents, List<ClientInfo> availableClients)
        {
            lock (locker)
            {
                if (!this.IsLoggedOn)
                {
                    this.IsLoggedOn = true;

                    this.ServerGuid = id;
                    this.FriendlyName = friendlyName;
                    this.AvailableComponents = availableComponents;
                    this.AvailableClients = availableClients;

                    // check for alive status permanently
                    Thread t = new Thread(new ThreadStart(this.SendKeepAliveRequests));
                    t.Start();

                    if (this.OnLogonCompleted != null)
                    {
                        ExternalServerLogonCompletedEventArgs args = new ExternalServerLogonCompletedEventArgs();
                        args.ExternalServer = this;

                        this.OnLogonCompleted(this, args);
                    }
                }
            }
        }

        // send logon request

        public void SendLogonRequest()
        {
            Thread t = new Thread(new ParameterizedThreadStart(this.SendLogonRequest));
            TcpClient client = this.GetConnection();

            if (client != null)
            {
                t.Start(client);
            }
        }

        private void SendLogonRequest(object data)
        {
            // send request

            LogonRequest logRequest = new LogonRequest();

            TcpClient client = (TcpClient)data;
            Tuple<byte, string> message = null;

            NetworkStream stream = client.GetStream();
            stream.ReadTimeout = Protocol.ExternalDefaultTimeout;

            logRequest.LogonRequestGuid = Guid.NewGuid();
            logRequest.ServerGuid = masterServer.ServerGuid;
            logRequest.FriendlyName = masterServer.FriendlyName;
            logRequest.AvailableComponents = masterServer.AvailableComponents;
            logRequest.AvailableClients = masterServer.AvailableClients;

            byte[] buff = Protocol.GetBytesFromLogonMessage(logRequest);

            stream.Write(buff, 0, buff.Length);

            // wait for response

            message = this.ReadSpecifiedDataFromStream(stream);

            client.Close();

            if (message != null && message.Item1 == 1)
            {
                LogonResponse response = Protocol.DeserializeStringToMessage<LogonResponse>(message.Item2);

                if (response != null)
                {
                    if (response.LogonRequestGuid.Equals(logRequest.LogonRequestGuid))
                    {
                        this.Logon(response.ServerGuid, response.FriendlyName, response.AvailableComponents.ToList(), response.AvailableClients.ToList());
                    }
                }
            }
        }

        // -------------------------------------------------------------

        // send keep alive requests (requests!!!!)

        public void SendKeepAliveRequests()
        {
            while (this.IsAlive)
            {
                Thread.Sleep(Protocol.ExternalKeepAliveInterval);

                Console.WriteLine("this checking " + DateTime.Now.ToString());

                TcpClient client = this.GetConnection();

                if (client != null)
                {
                    this.SendKeepAliveRequest(client);
                }
            }
        }

        private void SendKeepAliveRequest(object data)
        {
            // send request

            KeepAliveRequest request = new KeepAliveRequest();
            Tuple<byte, string> message = null;

            TcpClient client = (TcpClient)data;

            Console.WriteLine("send keep alive...");

            NetworkStream stream = client.GetStream();
            stream.ReadTimeout = Protocol.ExternalKeepAliveTimeout;

            request.KeepAliveRequestGuid = Guid.NewGuid();
            //request.ServerGuid = masterServer.ServerGuid; // Patrick says no
            request.CpuLoad = masterServer.CpuLoad;
            request.NumberOfClients = masterServer.NumberOfClients;
            request.Terminate = masterServer.Terminate;

            byte[] buff = Protocol.GetBytesFromKeepAliveMessage(request);

            stream.Write(buff, 0, buff.Length);

            // send response

            message = this.ReadSpecifiedDataFromStream(stream);

            client.Close();

            Console.WriteLine("finished waiting.");

            if (message != null && message.Item1 == 2)
            {
                KeepAliveResponse response = Protocol.DeserializeStringToMessage<KeepAliveResponse>(message.Item2);

                if (response != null && response.KeepAliveRequestGuid.Equals(request.KeepAliveRequestGuid))
                {
                    // success
                }
                else
                {
                    this.TerminateExternalServer();
                }
            }
        }

        // -------------------------------------------------------------

        // send component submit request

        public void SendComponentSubmitRequest(Guid componentSubmitRequestGuid, Component component)
        {
            Thread t = new Thread(new ParameterizedThreadStart(this.SendComponentSubmitRequest));

            TcpClient client = this.GetConnection();

            if (client != null)
            {
                t.Start(new Tuple<Guid, Component, TcpClient>(componentSubmitRequestGuid, component, client));
            }
        }

        private void SendComponentSubmitRequest(object data)
        {
            Tuple<Guid, Component, TcpClient> tData = (Tuple<Guid, Component, TcpClient>)data;

            this.SendComponentSubmitRequest(tData.Item1, tData.Item2, tData.Item3);
        }

        private void SendComponentSubmitRequest(Guid componentSubmitRequestGuid, Component component, TcpClient client)
        {
            // send request

            NetworkStream stream = client.GetStream();
            stream.ReadTimeout = Protocol.ExternalDefaultTimeout;

            ComponentSubmitRequest request = new ComponentSubmitRequest();
            request.ComponentSubmitRequestGuid = componentSubmitRequestGuid;
            request.Component = component;

            byte[] buff = Protocol.GetBytesFromMessage(request, 3);

            stream.Write(buff, 0, buff.Length);

            // wait for response

            Tuple<byte, string> message = this.ReadSpecifiedDataFromStream(stream);

            if (message != null && message.Item1 == 3)
            {
                ComponentSubmitResponse response = Protocol.DeserializeStringToMessage<ComponentSubmitResponse>(message.Item2);

                if (response != null && response.ComponentSubmitRequestGuid.Equals(request.ComponentSubmitRequestGuid))
                {
                    if (this.OnInternalComponentSubmitResponseReceived != null)
                    {
                        InternalComponentSubmitResponseEventArgs args = new InternalComponentSubmitResponseEventArgs();
                        args.ComponentSubmitRequestGuid = response.ComponentSubmitRequestGuid;
                        args.IsAccepted = response.IsAccepted;

                        this.OnInternalComponentSubmitResponseReceived(this, args);
                    }
                }
            }
        }

        // -------------------------------------------------------------

        // send job request

        // without JobRequest there would be too much parameters
        public void SendJobRequest(JobRequest request)
        {
            Thread t = new Thread(new ParameterizedThreadStart(this.SendJobRequest));

            TcpClient client = this.GetConnection();

            if (client != null)
            {
                t.Start(new Tuple<JobRequest, TcpClient>(request, client));
            }
        }

        private void SendJobRequest(object data)
        {
            // send request

            JobRequest request = ((Tuple<JobRequest, TcpClient>)data).Item1;
            TcpClient client = ((Tuple<JobRequest, TcpClient>)data).Item2;

            NetworkStream stream = client.GetStream();
            stream.ReadTimeout = Protocol.ExternalDefaultTimeout;

            byte[] buff = Protocol.GetBytesFromMessage(request, 4);

            stream.Write(buff, 0, buff.Length);

            // wait for response

            Tuple<byte, string> message = this.ReadSpecifiedDataFromStream(stream);

            if (message != null && message.Item1 == 4)
            {
                JobResponse response = Protocol.DeserializeStringToMessage<JobResponse>(message.Item2);

                if (response != null && response.JobRequestGuid.Equals(request.JobRequestGuid))
                {
                    if (this.OnJobResponseReceived != null)
                    {
                        ExternalJobResponseEventArgs args = new ExternalJobResponseEventArgs();
                        args.JobRequestGuid = response.JobRequestGuid;
                        args.IsAccepted = response.IsAccepted;

                        this.OnJobResponseReceived(this, args);
                    }
                }
            }
        }

        // --------------------------------------------------------------

        // send job result request

        public void SendJobResultRequest(JobResultRequest request)
        {
            Thread t = new Thread(new ParameterizedThreadStart(this.SendJobResultRequest));

            TcpClient client = this.GetConnection();

            if (client != null)
            {
                t.Start(new Tuple<JobResultRequest, TcpClient>(request, client));
            }
        }

        private void SendJobResultRequest(object data)
        {
            // send request

            JobResultRequest request = ((Tuple<JobResultRequest, TcpClient>)data).Item1;
            TcpClient client = ((Tuple<JobResultRequest, TcpClient>)data).Item2;

            NetworkStream stream = client.GetStream();
            stream.ReadTimeout = Protocol.ExternalDefaultTimeout;

            byte[] buff = Protocol.GetBytesFromMessage(request, 5);

            stream.Write(buff, 0, buff.Length);

            // wait for response

            Tuple<byte, string> message = this.ReadSpecifiedDataFromStream(stream);

            if (message != null && message.Item1 == 5)
            {
                JobResultResponse response = Protocol.DeserializeStringToMessage<JobResultResponse>(message.Item2);

                if (response != null && response.JobResultGuid.Equals(request.JobResultGuid))
                {
                    if (this.OnJobResultResponseReceived != null)
                    {
                        ExternalJobResultResponseEventArgs args = new ExternalJobResultResponseEventArgs();
                        args.JobResultGuid = response.JobResultGuid;

                        this.OnJobResultResponseReceived(this, args);
                    }
                }
            }
        }

        // --------------------------------------------------------------

        // send assembly request

        public void SendAssemblyRequest(Guid assemblyRequestGuid, Guid componentGuid)
        {
            Thread t = new Thread(new ParameterizedThreadStart(this.SendAssemblyRequest));

            TcpClient client = this.GetConnection();

            if (client != null)
            {
                t.Start(new Tuple<Guid, Guid, TcpClient>(assemblyRequestGuid, componentGuid, client));
            }
        }

        private void SendAssemblyRequest(object data)
        {
            // send request

            Tuple<Guid, Guid, TcpClient> tData = (Tuple<Guid, Guid, TcpClient>)data;
            TcpClient client = tData.Item3;

            NetworkStream stream = client.GetStream();
            stream.ReadTimeout = Protocol.ExternalDefaultTimeout;

            AssemblyRequest request = new AssemblyRequest();
            request.AssemblyRequestGuid = tData.Item1;
            request.ComponentGuid = tData.Item2;

            byte[] buff = Protocol.GetBytesFromMessage(request, 6);

            stream.Write(buff, 0, buff.Length);

            byte[] begin = new byte[5];

            // wait for binary content (special case!!)

            if (stream.Read(begin, 0, begin.Length) == 5)
            {
                if (begin[0] == 6)
                {
                    byte[] binaryContent = new byte[BitConverter.ToInt32(begin, 1)];

                    stream.Read(binaryContent, 0, binaryContent.Length);

                    // Handle binary content

                    // fire event
                    ExternalServerAssemblyRequestedEventArgs args = new ExternalServerAssemblyRequestedEventArgs();
                    args.AssemblyRequestGuid = request.AssemblyRequestGuid;
                    args.ComponentGuid = request.ComponentGuid;
                    args.BinaryContent = binaryContent;

                    if (this.OnAssemblyReceived != null)
                    {
                        this.OnAssemblyReceived(this, args);
                    }
                }
            }
        }

        // --------------------------------------------------------------

        // send client update request

        public void SendClientUpdateRequest(ClientUpdateRequest request)
        {
            Thread t = new Thread(new ParameterizedThreadStart(this.SendClientUpdateRequest));

            TcpClient client = this.GetConnection();

            if (client != null)
            {
                t.Start(new Tuple<ClientUpdateRequest, TcpClient>(request, client));
            }
        }

        private void SendClientUpdateRequest(object data)
        {
            // send request
            Tuple<ClientUpdateRequest, TcpClient> tData = (Tuple<ClientUpdateRequest, TcpClient>)data;
            TcpClient client = tData.Item2;

            NetworkStream stream = client.GetStream();
            stream.ReadTimeout = Protocol.ExternalDefaultTimeout;

            ClientUpdateRequest request = tData.Item1;

            byte[] buff = Protocol.GetBytesFromMessage(request, 7);

            stream.Write(buff, 0, buff.Length);

            // wait for response

            Tuple<byte, string> message = this.ReadSpecifiedDataFromStream(stream);

            if (message != null && message.Item1 == 7)
            {
                ClientUpdateResponse response = Protocol.DeserializeStringToMessage<ClientUpdateResponse>(message.Item2);

                if (response != null && response.ClientUpdateRequestGuid.Equals(request.ClientUpdateRequestGuid))
                {
                    if (this.OnInternalClientUpdatedResponseReceived != null)
                    {
                        InternalClientUpdateResponseEventArgs args = new InternalClientUpdateResponseEventArgs();
                        args.ClientUpdateRequestGuid = response.ClientUpdateRequestGuid;

                        this.OnInternalClientUpdatedResponseReceived(this, args);
                    }
                }
            }
        }

        // ----------------------------------------------------------------

        public void StartListening(TcpClient client)
        {
            Thread t = new Thread(new ParameterizedThreadStart(this.ListenToExternalServer));
            t.Start(client);
        }

        public void TerminateExternalServer()
        {
            if (this.IsAlive && this.IsLoggedOn)
            {
                this.IsAlive = false;

                if (this.OnTerminated != null)
                {
                    // fail
                    this.OnTerminated(this, new ExternalServerDiedEventArgs() { ExternalServer = this });
                }
            }
        }

        private void ListenToExternalServer(object data)
        {
            TcpClient client = (TcpClient)data;
            NetworkStream stream = client.GetStream();
            stream.ReadTimeout = Protocol.ExternalDefaultTimeout;

            Tuple<byte, string> message = this.ReadSpecifiedDataFromStream(stream);

            if (message != null)
            {
                byte[] buff;

                if (message.Item1 == 6)
                {
                    buff = Protocol.GetBytesFromAssemblyResponse((byte[])GetHandledMessage(message.Item1, message.Item2, stream));
                }
                else
                {
                    buff = Protocol.GetBytesFromMessage(GetHandledMessage(message.Item1, message.Item2, stream), message.Item1);
                }

                stream.Write(buff, 0, buff.Length);
            }

            stream.Close();
            client.Close();
        }

        // Returns for example a LogonResponse
        private object GetHandledMessage(byte code, string message, NetworkStream stream)
        {
            object msg = null;

            switch (code)
            {
                case 1:
                    {
                        LogonRequest request = Protocol.DeserializeStringToMessage<LogonRequest>(message);

                        if (request != null)
                        {
                            msg = this.GetHandledLogonRequest(request);
                        }

                        break;
                    }
                case 2:
                    {
                        KeepAliveRequest request = Protocol.DeserializeStringToMessage<KeepAliveRequest>(message);

                        if (request != null)
                        {
                            msg = this.GetHandledKeepAliveRequest(request);
                        }

                        break;
                    }
                case 3:
                    {
                        ComponentSubmitRequest request = Protocol.DeserializeStringToMessage<ComponentSubmitRequest>(message);

                        if (request != null)
                        {
                            msg = this.GetHandledComponentSubmitRequest(request);
                        }

                        break;
                    }
                case 4:
                    {
                        JobRequest request = Protocol.DeserializeStringToMessage<JobRequest>(message);

                        if (request != null)
                        {
                            msg = this.GetHandledJobRequest(request);
                        }

                        break;
                    }
                case 5:
                    {
                        JobResultRequest request = Protocol.DeserializeStringToMessage<JobResultRequest>(message);

                        if (request != null)
                        {
                            msg = this.GetHandledJobResultRequest(request);
                        }

                        break;
                    }
                case 6:
                    {
                        AssemblyRequest request = Protocol.DeserializeStringToMessage<AssemblyRequest>(message);

                        if (request != null)
                        {
                            msg = this.GetHandledAssemblyRequest(request);
                        }

                        break;
                    }
                case 7:
                    {
                        ClientUpdateRequest request = Protocol.DeserializeStringToMessage<ClientUpdateRequest>(message);

                        if (request != null)
                        {
                            msg = this.GetHandledClientUpdateRequest(request);
                        }

                        break;
                    }
            }

            return msg;
        }

        private object GetHandledLogonRequest(LogonRequest request)
        {
            this.Logon(request.ServerGuid, request.FriendlyName, request.AvailableComponents.ToList(), request.AvailableClients.ToList());

            LogonResponse response = new LogonResponse();

            response.LogonRequestGuid = request.LogonRequestGuid;
            response.ServerGuid = masterServer.ServerGuid;
            response.FriendlyName = masterServer.FriendlyName;
            response.AvailableClients = masterServer.AvailableClients.AsEnumerable();
            response.AvailableComponents = masterServer.AvailableComponents.AsEnumerable();

            return response;
        }

        private object GetHandledKeepAliveRequest(KeepAliveRequest request)
        {
            this.CpuLoad = request.CpuLoad;
            this.NumberOfClients = request.NumberOfClients;

            KeepAliveResponse response = new KeepAliveResponse();
            response.KeepAliveRequestGuid = request.KeepAliveRequestGuid;

            if (request.Terminate)
            {
                this.TerminateExternalServer();
            }

            return response;
        }

        private object GetHandledComponentSubmitRequest(ComponentSubmitRequest request)
        {
            ComponentSubmitResponse response = new ComponentSubmitResponse();
            response.ComponentSubmitRequestGuid = request.ComponentSubmitRequestGuid;

            ExternalComponentSubmittedEventArgs args = new ExternalComponentSubmittedEventArgs();
            args.Component = request.Component;

            if (this.OnExternalComponentSubmitRequestReceived != null)
            {
                this.OnExternalComponentSubmitRequestReceived(this, args);
            }

            response.IsAccepted = args.IsAccepted;

            return response;
        }

        private object GetHandledJobRequest(JobRequest request)
        {
            JobResponse response = new JobResponse();
            response.JobRequestGuid = request.JobRequestGuid;

            ExternalJobRequestEventArgs args = new ExternalJobRequestEventArgs();

            args.JobRequestGuid = request.JobRequestGuid;
            args.FriendlyName = request.FriendlyName;
            args.InputData = request.InputData;
            args.JobComponent = request.JobComponent;
            args.JobSourceClientGuid = request.JobSourceClientGuid;
            args.JobGuid = request.JobGuid;

            args.TargetDisplayClient = request.TargetDisplayClient;
            args.TargetCalcClientGuid = request.TargetCalcClientGuid;

            args.HopCount = request.HopCount;

            if (this.OnJobRequestReceived != null)
            {
                this.OnJobRequestReceived(this, args);
            }

            response.IsAccepted = args.IsAccepted;

            return response;
        }

        private object GetHandledJobResultRequest(JobResultRequest request)
        {
            JobResultResponse response = new JobResultResponse();
            response.JobResultGuid = request.JobResultGuid;

            ExternalJobResultRequestEventArgs args = new ExternalJobResultRequestEventArgs();

            args.JobResultGuid = request.JobResultGuid;
            args.State = request.State;
            args.JobGuid = request.JobGuid;
            args.OutputData = request.OutputData;

            if (this.OnJobResultRequestReceived != null)
            {
                this.OnJobResultRequestReceived(this, args);
            }

            return response;
        }

        private object GetHandledAssemblyRequest(AssemblyRequest request)
        {
            ExternalServerAssemblyRequestedEventArgs args = new ExternalServerAssemblyRequestedEventArgs();
            args.AssemblyRequestGuid = request.AssemblyRequestGuid;
            args.ComponentGuid = request.ComponentGuid;
            args.BinaryContent = new byte[] { 8, 2 };

            if (this.OnAssemblyRequested != null)
            {
                this.OnAssemblyRequested(this, args);
            }

            return args.BinaryContent;
        }

        private object GetHandledClientUpdateRequest(ClientUpdateRequest request)
        {
            if (request.ClientState == ClientState.Connected)
            {
                this.AvailableClients.Add(request.ClientInfo);
            }
            else
            {
                foreach (ClientInfo info in this.AvailableClients)
                {
                    if (info.ClientGuid.Equals(request.ClientInfo.ClientGuid))
                    {
                        this.AvailableClients.Remove(info);
                    }
                }
            }

            ClientUpdateResponse response = new ClientUpdateResponse();
            response.ClientUpdateRequestGuid = request.ClientUpdateRequestGuid;

            ExternalClientUpdateRequestEventArgs args = new ExternalClientUpdateRequestEventArgs();
            args.ClientInfo = request.ClientInfo;
            args.ClientState = request.ClientState;
            args.ClientUpdateRequestGuid = request.ClientUpdateRequestGuid;

            if (this.OnExternalClientUpdated != null)
            {
                this.OnExternalClientUpdated(this, args);
            }

            return response;
        }

        private Tuple<byte, string> ReadSpecifiedDataFromStream(NetworkStream stream)
        {
            try
            {
                byte[] buffer = new byte[5];

                if (stream.Read(buffer, 0, buffer.Length) == 5)
                {
                    byte[] messBuff = new byte[BitConverter.ToInt32(buffer, 1)];

                    stream.Read(messBuff, 0, messBuff.Length);

                    string message = Encoding.UTF8.GetString(messBuff);

                    return new Tuple<byte, string>(buffer[0], message);
                }
            }
            catch (Exception)
            {

            }

            // It seems that the external server doesn't want to communicate anymore, because he sent invalid messages. Other ways to handle this?
            this.TerminateExternalServer();

            return null;
        }
    }
}