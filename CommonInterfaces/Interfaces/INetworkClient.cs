﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRessources
{
    public interface INetworkClient
    {
        NetworkState NetworkClient
        {
            get;
            set;
        }

        bool SendResult(List<object> Result, Guid id);

        bool SendJobRequest(IComponent component);

        event EventHandler<ClientComponentEventArgs> RequestEvent;

        void Connect(string ip);

        void Disconnect();
    }
}
