using GrpcBroker.DTO;
using GrpcBroker.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcBroker.Services
{
	public class MessageStorageService : IMessageStorageService
	{
		private readonly ConcurrentQueue<Message> _messageQueue;
		private readonly List<string> _sensors;
		private readonly object _locker;

		public MessageStorageService()
		{
			_messageQueue = new ConcurrentQueue<Message>();
			_sensors = new List<string>();
			_locker = new object();
		}

		public void Add(Message message) 
		{
			_messageQueue.Enqueue(message);

			lock (_locker)
			{
				if(_sensors.Any(s => s.ToLower() == message.Sensor.ToLower()))
				{
					return;
				}
				else
				{
					_sensors.Add(message.Sensor);
				}
			}
		}

		public bool IsEmpty() => _messageQueue.IsEmpty;
		public IEnumerable<string> GetSensorsList() => _sensors;

		public Message GetNext()
		{
			_messageQueue.TryDequeue(out Message msg);

			return msg;
		}
	}
}
