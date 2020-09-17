using Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{

	class MessageBroker
	{
		static Socket _listenerSocket;
		const int port = 4242;
		static IList<ClientData> _subscribers;
		static IList<ClientData> _clients;
		static IList<Topic> _topics = new List<Topic>();

		static void Main(string[] args)
		{
			var ip = GetIpAddress();
			Console.WriteLine($"Starting server on {ip}");

			_subscribers = new List<ClientData>();
			_clients = new List<ClientData>();

			_listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			var ipEndpoint = new IPEndPoint(IPAddress.Parse(ip), port);
			_listenerSocket.Bind(ipEndpoint);

			new Thread(HandleClientConnection).Start();
		}

		private static void HandleClientConnection()
		{
			while (true)
			{
				ClientData newClient = null;
				try
				{
					_listenerSocket.Listen(0);
					newClient = new ClientData(_listenerSocket.Accept());
					_clients.Add(newClient);
					var response = new Response(newClient.ClientId.ToString(), StatusCode.ClientAccepted);
					byte[] buffer = /*Encoding.ASCII.GetBytes(client.ClientId.ToString())*/ response.ToBytes();
					Console.WriteLine($"{newClient.ClientId} has connected.");
					newClient.Socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
				}
				catch (Exception e)
				{
					var response = new Response(e.Message, StatusCode.Fail);
					byte[] buffer = response.ToBytes();
					newClient.Socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
				}
			}
		}

		public static void ClientDataIn(object socket)
		{
			var receiver = socket as Socket;
			byte[] buffer;
			int bytesRead;

			try
			{
				while (true)
				{
					buffer = new byte[receiver.Available];
					bytesRead = receiver.Receive(buffer, SocketFlags.None);
					if(bytesRead > 0)
					{
						DistributeData(Encoding.ASCII.GetString(buffer), buffer);
					}
				}
			}
			catch (SocketException)
			{
				Console.WriteLine($"A client has disconnected.");
				receiver.Close();
				var clientToRemove = _clients.FirstOrDefault(c => c.Socket.IsDead());
				if(clientToRemove != null)
				{
					_clients.Remove(clientToRemove);
				}
				var subscribersToRemove = _subscribers.Where(s => s.Socket.IsDead()).ToList();
				if (subscribersToRemove.Any())
				{
					_subscribers.RemoveRange(subscribersToRemove);
				}
			}
		}

		//clientId=clientId?action=Send?topic=TopicName?payload=MessageText
		//clientId=clientId?action=Subscribe?topic=Topic1Id;Topic2Id;Topic3Id
		//clientId=clientId?action=Unsubscribe?topic=Topic1Id;Topic2Id;Topic3Id
		//clientId=clientId?action=TopicList
		public static void DistributeData(string message, byte[] bytesReceived)
		{
			var receivedPacket = new Packet(bytesReceived);

			#region OLD_IMPLEMENTATION_URL_PARSING
			//var messageParts = message.Split('?').Select(item => 
			//{
			//	var keyValue = item.Split('=');
			//	return new ConcurrentDictionary<string, string>() { [keyValue[0]] = keyValue[1] };
			//});

			//messageParts.SingleOrDefault(dict => dict.ContainsKey("clientId")).TryGetValue(key: "clientId", out var clientId);
			//var clientGuid = Guid.Parse(clientId);

			//messageParts.SingleOrDefault(dict => dict.ContainsKey("action")).TryGetValue(key: "action", out var action);
			//Enum.TryParse(action, out ClientAction actionType);
			#endregion

			var sentTopic = string.Empty;
			var topicExists = false;

			switch (receivedPacket.Action)
			{
				case ClientAction.Send:
					//messageParts.SingleOrDefault(dict => dict.ContainsKey("topic")).TryGetValue(key: "topic", out sentTopic);
					//               messageParts.SingleOrDefault(dict => dict.ContainsKey("payload")).TryGetValue(key: "payload", out var payload);
					//PublishMessage(clientGuid, sentTopic, payload);
					//SendMessageToSubscribers(sentTopic, payload);
					PublishMessage(receivedPacket.ClientId, receivedPacket.SensorTypes[0], receivedPacket.Data.ToString());
					SendMessageToSubscribers(receivedPacket.SensorTypes[0], receivedPacket.Data.ToString());
					break;
				case ClientAction.Subscribe:
					//messageParts.SingleOrDefault(dict => dict.ContainsKey("topic")).TryGetValue(key: "topic", out var topics);
					//topicExists = Subscribe(clientGuid, topics.Contains(';') ? topics.Split(';') : new string[] { topics});
					topicExists = Subscribe(receivedPacket.ClientId, receivedPacket.SensorTypes);
					break;
				case ClientAction.Unsubscribe:
					//messageParts.SingleOrDefault(dict => dict.ContainsKey("topic")).TryGetValue(key: "topic", out topics);
					//               Unsubscribe(clientGuid, topics.Contains(';') ? topics.Split(';') : new string[] { topics });
					Unsubscribe(receivedPacket.ClientId, receivedPacket.SensorTypes);
					break;
				case ClientAction.TopicList:
					SendListOfTopicsToClients(receivedPacket.ClientId);
					break;
                default:
					throw new ArgumentOutOfRangeException();
			}

			#region OLD_VERSION
			//if((ClientAction) receivedPacket.Action == ClientAction.Subscribe && _subscribers.Any(s => s.ClientId == receivedPacket.ClientId && s.ShouldGetTopicHistory))
			//         {
			//	var recv = _subscribers.FirstOrDefault(s => s.ClientId == receivedPacket.ClientId);
			//	var dataToSend = _topics
			//			.Where(t => recv.TopicIds.Contains(t.TopicId))
			//			.Select(t => $"{t.Name} : {string.Join(';', t.Messages)}")
			//			.ToArray();
			//	var bytes = Encoding.ASCII.GetBytes(string.Join(Environment.NewLine, dataToSend));
			//	recv.Socket.Send(bytes, 0, bytes.Length, SocketFlags.None);
			//}
			#endregion

			if (!topicExists && (ClientAction)receivedPacket.Action == ClientAction.Subscribe)
			{
				var recv = _subscribers.FirstOrDefault(s => s.ClientId == receivedPacket.ClientId);
				var errMsg = "ERROR: One or more specified sensors not found.";
				var response = new Response(errMsg, StatusCode.Fail);
				var bytes = response.ToBytes();
					//Encoding.ASCII.GetBytes(errMsg);
				recv.Socket.Send(bytes, 0, bytes.Length, SocketFlags.None);
			}
        }

		private static void SendMessageToSubscribers(string sentTopic, string payload)
        {
			var topicId = _topics.FirstOrDefault(t => t.Name == sentTopic).TopicId;
			foreach (var subscriber in _subscribers)
			{
				if (subscriber.TopicIds.Contains(topicId))
				{
					//Console.WriteLine($"Send to {subscriber.ClientId}");

					var message = $"{sentTopic}: {payload}";

					var response = new Response(message, StatusCode.Success);

					var bytes = response.ToBytes();
						//Encoding.ASCII.GetBytes(message);
					subscriber.Socket.Send(bytes, 0, bytes.Length, SocketFlags.None);
				}
			}
		}

		private static void SendListOfTopicsToClients(Guid clientGuid)
		{
			var nl = Environment.NewLine;
			var topics = _topics.Select(t => t.Name).ToArray();
			var message = topics.Length > 0 ? $"All connected sensors: {(topics.Length > 1 ? string.Join(';', topics) : topics[0])}" : "No topics.";
			var response = new Response(message, topics.Any() ? StatusCode.Success : StatusCode.Fail);
			var bytes = /*Encoding.ASCII.GetBytes(message)*/ response.ToBytes();
			_clients.FirstOrDefault(c => c.ClientId == clientGuid).Socket.Send(bytes, 0, bytes.Length, SocketFlags.None);
		}


		private static void PublishMessage(Guid publisherId, string topic, string payload)
		{
			Console.WriteLine($"Publishing a message...{topic} - {payload}");
			if (_topics.Any())
			{
				var aTopic = _topics.FirstOrDefault(t => t.Name.ToLower() == topic.ToLower());
				if(aTopic == null)
				{
					aTopic = new Topic
					{
						TopicId = Guid.NewGuid(),
						Name = topic,
						Messages = new List<string> { payload }
					};
					_topics.Add(aTopic);
				}
				else
				{
					aTopic.Messages.Add(payload);
				}
			}
			else
			{
				var newTopic = new Topic { TopicId = Guid.NewGuid(), Name = topic, Messages = new List<string> { payload } };
				_topics = new List<Topic> { newTopic };
			}
		}

        private static void Unsubscribe(Guid clientGuid, IEnumerable<string> topics)
		{
			Console.WriteLine("Unsubscribing from a topic...");
			var subscriber = _subscribers.FirstOrDefault(s => s.ClientId == clientGuid);
			if (subscriber != null)
			{
				var topicIds = _topics.Where(t => topics.Contains(t.Name)).Select(t => t.TopicId);

				foreach (var topicId in topicIds)
				{
					if (subscriber.TopicIds.Contains(topicId))
					{
						subscriber.TopicIds.Remove(topicId);
					}
				}

				if (subscriber.TopicIds.Count() == 0)
				{
					_subscribers.Remove(subscriber);
				}
			}
		}

		private static bool Subscribe(Guid clientGuid, IEnumerable<string> topics)
		{
			if (!_subscribers.Any(s => s.ClientId == clientGuid))
			{
				return AddNewSubscriber(clientGuid, topics);
			}
			else
			{
				return AddNewSubscription(clientGuid, topics);
			}
		}

		private static bool AddNewSubscriber(Guid clientGuid, IEnumerable<string> topics)
		{
            var topicGuids = _topics.Where(t => topics.Contains(t.Name.ToLower())).Select(t => t.TopicId);
			var subscriber = _clients.FirstOrDefault(cd => cd.ClientId == clientGuid);
			foreach (var topicId in topicGuids)
			{
				if (!subscriber.TopicIds.Any(t => t == topicId))
				{
					subscriber.TopicIds.Add(topicId);
					subscriber.ShouldGetTopicHistory = true;
				}
			}
			_subscribers.Add(subscriber);

			return topicGuids.Any();
		}

		private static bool AddNewSubscription(Guid clientGuid, IEnumerable<string> topics)
		{
			var anyTopicExists = _topics.Any(t => topics.Contains(t.Name));
			if (!anyTopicExists) return false;

			var subscriber = _subscribers.SingleOrDefault(s => s.ClientId == clientGuid);
			subscriber.TopicIds = new List<Guid>();
			foreach (var topicName in topics)
			{
				var topicId = _topics.SingleOrDefault(t => t.Name == topicName).TopicId;
				if (!subscriber.TopicIds.Any(t => t == topicId))
				{
					subscriber.TopicIds.Add(topicId);
				}
			}

			return anyTopicExists;
		}

		static string GetIpAddress()
		{
			IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
			foreach (IPAddress iP in ips)
			{
				if (iP.AddressFamily == AddressFamily.InterNetwork)
				{
					return iP.ToString();
				}
			}

			return "127.0.0.1";
		}
	}
}
