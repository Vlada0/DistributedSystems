using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
	internal class ClientData
	{
		public Socket Socket { get; }
		public Thread ClientThread { get; }
		public Guid ClientId { get; set; }
		public ICollection<Guid> TopicIds { get; set; } = new List<Guid>();
		public bool HasCurrentMesageSent { get; set; } = false;
		public bool ShoulGetTopicHistory { get; set; } = false;

        public ClientData()
		{
			ClientId = Guid.NewGuid();
			ClientThread = new Thread(MessageBroker.ClientDataIn);
			ClientThread.Start(Socket);
		}

		public ClientData(Socket socket)
		{
			Socket = socket;
			ClientId = Guid.NewGuid();
			ClientThread = new Thread(MessageBroker.ClientDataIn);
			ClientThread.Start(Socket);
		}
	}
}
