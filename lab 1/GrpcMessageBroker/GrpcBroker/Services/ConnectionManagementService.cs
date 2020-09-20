using GrpcBroker.DTO;
using GrpcBroker.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcBroker.Services
{
	public class ConnectionManagementService : IConnectionManagementService
	{
		private readonly List<Connection> _connections;
		private readonly object _locker;

		public ConnectionManagementService()
		{
			_connections = new List<Connection>();
			_locker = new object();
		}

		public bool TryAddConnection(Connection connection)
		{
			lock (_locker)
			{
				if(_connections.Any(c => c.Address == connection.Address))
				{
					return false;
				}
				else
				{
					_connections.Add(connection);
					return true;
				}
			}
		}

		public IEnumerable<Connection> GetConnectionsBySensor(string sensor)
		{
			lock (_locker)
			{
				return _connections.Where(conn => conn.Sensor == sensor);
			}
		}

		public void RemoveConnection(string address)
		{
			lock (_locker)
			{
				_connections.RemoveAll(conn => conn.Address == address);
			}
		}
	}
}
