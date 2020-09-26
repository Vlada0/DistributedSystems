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
		private readonly IMessageSenderService _messageSender;

		public PublisherService(IMessageStorageService messageStorageService, IMessageSenderService messageSender)
		{
			_messageStorageService = messageStorageService;
			_messageSender = messageSender;
		}

		public override async Task<PublishResponse> PublishMessage(PublishCommand command, ServerCallContext context)
		{
			var message = Message.From(command);
			_messageStorageService.Add(message);

			await _messageSender.SendMessageAsync(message);

			return new PublishResponse
			{
				IsSucceed = true
			};
		}
	}
}
