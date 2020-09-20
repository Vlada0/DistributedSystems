using GrpcBroker.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcBroker.Services.Interfaces
{
	public interface IConnectionManagementService
	{
		bool TryAddConnection(Connection connection);
		void RemoveConnection(string address);
		IEnumerable<Connection> GetConnectionsBySensor(string sensor);
	}
}
