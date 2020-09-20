using GrpcAgent;

namespace GrpcBroker.DTO
{
	public class Message
	{
		public string Sensor { get; set; }
		public int Data { get; set; }

		public static Message From(PublishCommand command) =>
			new Message { Sensor = command.Sensor, Data = command.Data };
	}
}
