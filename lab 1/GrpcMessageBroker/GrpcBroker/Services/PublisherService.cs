using Grpc.Core;
using GrpcAgent;
using GrpcBroker.DTO;
using GrpcBroker.Services.Interfaces;
using System.Threading.Tasks;

namespace GrpcBroker.Services
{
	public class PublisherService : Publisher.PublisherBase
	{
		private readonly IMessageStorageService _messageStorageService;

		public PublisherService(IMessageStorageService messageStorageService)
		{
			_messageStorageService = messageStorageService;
		}

		public override Task<PublishResponse> PublishMessage(PublishCommand command, ServerCallContext context)
		{
			var message = Message.From(command);
			_messageStorageService.Add(message);

			return Task.FromResult(new PublishResponse
			{
				IsSucceed = true
			});
		}
	}
}
