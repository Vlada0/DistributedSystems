using Grpc.Core;
using GrpcAgent;
using GrpcExecutor.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrpcExecutor.Services
{
	public class NotificationService : Notifier.NotifierBase
	{
		static readonly Dictionary<ExecutorTypeEnum, string[]> Scenarios = new Dictionary<ExecutorTypeEnum, string[]>
		{
			[ExecutorTypeEnum.Valve] = new string[] { "Valve has opened.", "Valve has closed." },
			[ExecutorTypeEnum.Pump] = new string[] { "Air has pumped in.", "Air has pumped out." },
			[ExecutorTypeEnum.Switch] = new string[] { "Light has turned on.", "Light has turned off." }
		};

		public override Task<NotifyResponse> Notify(NotifyCommand command, ServerCallContext context)
		{
			ExecutorTypeEnum executor = (ExecutorTypeEnum)command.ExecutorType;
			var color = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"{command.Sensor} - {command.Data}: {Scenarios[executor][command.Data > 3500 ? 1 : 0]}");
			Console.ForegroundColor = color;
			
			return Task.FromResult(new NotifyResponse());
		}
	}
}
