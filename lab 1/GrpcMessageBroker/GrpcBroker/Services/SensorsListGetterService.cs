using Grpc.Core;
using GrpcAgent;
using GrpcBroker.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcBroker.Services
{
	public class SensorsListGetterService : SensorsListGetter.SensorsListGetterBase
	{
		private readonly IMessageStorageService _messageStorageService;

		public SensorsListGetterService(IMessageStorageService messageStorageService)
		{
			_messageStorageService = messageStorageService;
		}

		public override Task<SensorsListReponse> GetSensorsList(SensorsListRequest request, ServerCallContext context)
		{
			var sensors = _messageStorageService.GetSensorsList();

			return Task.FromResult(new SensorsListReponse {IsEmpty = !sensors.Any(), Sensors = string.Join(';', sensors) });
		}
	}
}
