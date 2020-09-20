using Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
	enum Executor
	{
		Valve = 1,
		Pump,
		Switch
	}

	class Client
	{
		static readonly string nl = Environment.NewLine;
		public static Socket _socket;
		const int port = 4242;
		static Guid CLIENT_ID = Guid.Empty;
		static Executor SensorType = default;
		static StatusCode ReceiviedCode = StatusCode.Fail;

		static readonly Dictionary<Executor, string[]> Scenarios = new Dictionary<Executor, string[]>
		{
			[Executor.Valve] = new string[] { "Valve has opened.", "Valve has closed." },
			[Executor.Pump] = new string[] {"Air has pumped in.", "Air has pumped out."},
			[Executor.Switch] = new string[] {"Light has turned on.", "Light has turned off."}
		};

		static async Task Main(string[] args)
		{
			bool connected = false;
			while (!connected)
			{
				Console.Write($"Enter host ip: ");
				string ip = Console.ReadLine().Trim();

				_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				IPEndPoint ipEndpoint = new IPEndPoint(IPAddress.Parse(ip), port);

				try
				{
					await _socket.ConnectAsync(ipEndpoint);
					connected = true;
					var thread = new Thread(async () => await GetClientId());
					thread.Start();
				}
				catch (Exception)
				{  
					Console.WriteLine($"Could not connect to host.");
					connected = false;
					Thread.Sleep(2000);
				}
			}

			await Run();

			#region OLD_VERSION
			//while (true)
			//{
			//	Console.WriteLine($"Operation:{nl}1.Publish{nl}2.Subscribe{nl}3.Unsubscribe{nl}4.Topic List");
			//	int.TryParse(Console.ReadLine(), out var input);
			//	var url = string.Empty;
			//	switch (input)
			//	{
			//		case 1:
			//			url = Publish(1);
			//			var bytesToSend = Encoding.Default.GetBytes(url);
			//			_socket.Send(bytesToSend);
			//			break;
			//		case 2:
			//			url = Subscribe(2);
			//			var bytes = Encoding.ASCII.GetBytes(url);
			//			_socket.Send(bytes, 0, bytes.Length, SocketFlags.None);
			//			new Thread(ListenForMessages).Start();
			//			break;
			//		case 3:
			//			url = Unsubscribe(3);
			//			var buffer = Encoding.ASCII.GetBytes(url);
			//			_socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
			//			break;
			//		case 4:
			//			url = GetTopicListUrl(4);
			//			var bufferData = Encoding.ASCII.GetBytes(url);
			//			_socket.Send(bufferData, 0, bufferData.Length, SocketFlags.None);
			//			new Thread(ListenForMessages).Start();
			//			break;
			//		default:
			//			break;
			//	}
			//}
			#endregion
		}

		static async Task Run()
		{
			Console.WriteLine($"1 - Sensor{nl}2 - Executor");
			if(int.TryParse(Console.ReadLine(), out var input))
			{
				switch (input)
				{
					case 1:
						await RunAsSensor();
						break;
					case 2:
						await RunAsExecutor();
						break;
					default:
						break;
				}
			}
		}

		static async Task RunAsSensor()
		{
			
			Console.WriteLine("Sensor type: ");
			var sensorType = Console.ReadLine().Trim();

			const int low = 2048;
			const int high = 4096;
			//const int barrier = high - low / 2;
			var rnd = new Random();
			while (true)
			{
				var data = rnd.Next(low, high + 1);
				Console.WriteLine($"{sensorType} : {data}");
				var packet = new Packet(CLIENT_ID, ClientAction.Send, new string[] { sensorType }, data);
				var bytesToSend = packet.ToBytes();
				try
				{
					_socket.Send(bytesToSend, 0, bytesToSend.Length, SocketFlags.None);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"An error occured: {ex.Message}");
					Thread.Sleep(2000);
					Environment.Exit(0);
				}
				//if (data > barrier)
				//{
					
				//}
				await Task.Delay(data);
			}
		}

		
		static async Task RunAsExecutor()
		{
			int input;

			GetSensorList();
			await Task.Delay(1000);

			if (ReceiviedCode == StatusCode.Fail)
			{
				Environment.Exit(0);
			}

			Console.WriteLine("Type: 1 - Valve; 2 - Pump; 3 - Switch;");
			while (!int.TryParse(Console.ReadLine(), out input))
			{
				Console.WriteLine("Type: 1 - Valve; 2 - Pump; 3 - Switch;");
			}

			SensorType = (Executor)input;

			Console.WriteLine($"{(Executor)input} - Get data from sensor: ");
			var sensorToSubscribe = Console.ReadLine().Trim();
			var packet = new Packet(CLIENT_ID, ClientAction.Subscribe, sensorToSubscribe.Contains(';') ? sensorToSubscribe.Split(';') : new string[] { sensorToSubscribe }, null);

			var bytes = packet.ToBytes();
			_socket.Send(bytes, 0, bytes.Length, SocketFlags.None);
			new Thread(ListenForMessages).Start();
		}

		private static void GetSensorList()
		{
			var packet = new Packet(CLIENT_ID, ClientAction.TopicList, null, null);
			var bytes = packet.ToBytes();
			_socket.Send(bytes, 0, bytes.Length, SocketFlags.None);
			new Thread(() =>
			{
				try
				{
					byte[] buffer;
					int readBytes;
					var contentRead = false;
					while (!contentRead)
					{
						buffer = new byte[_socket.Available];
						readBytes = _socket.Receive(buffer, SocketFlags.None);
						if (readBytes > 0)
						{
							var response = new Response(buffer);
							if(response.Code == StatusCode.Fail)
							{
								Console.WriteLine("An error occured.");
							}
							else
							{
								Console.WriteLine(response.Message);
								//var content = Encoding.ASCII.GetString(buffer);
								//Console.WriteLine(content);
								contentRead = true;
							}
							ReceiviedCode = response.Code;
						}
					}
				}
				catch (SocketException)
				{
					Console.WriteLine("A server has disconnected.");
					Console.ReadLine();
					Environment.Exit(0);
				}
			}).Start();
		}

		static async Task GetClientId()
		{
			byte[] buffer;
			int readBytes;
			var hasGot = false;
			try
			{
				while (!hasGot)
				{
					buffer = new byte[_socket.Available];
					readBytes = await _socket.ReceiveAsync(buffer, SocketFlags.None);
					if (readBytes > 0)
					{
						var response = new Response(buffer);
						if(response.Code == StatusCode.ClientAccepted)
						{
							var content = response.Message;
							//Encoding.ASCII.GetString(buffer);
							Guid.TryParse(content, out CLIENT_ID);
							Console.WriteLine(CLIENT_ID);
							hasGot = true;
						}
						else if(response.Code == StatusCode.Fail)
						{
							Console.WriteLine($"An error occured. {response.Message}");
						}
					}
				}
			}
			catch (SocketException)
			{
				Console.WriteLine("Server has disconnected.");
				Console.ReadLine();
				Environment.Exit(0);
			}
		}

		private static void ListenForMessages()
		{
			byte[] buffer;
			int readBytes;
			try
			{
				Console.WriteLine("Receive: ");
				while (true)
				{
					buffer = new byte[_socket.Available];
					readBytes = _socket.Receive(buffer, SocketFlags.None);
					if (readBytes > 0)
					{
						var response = new Response(buffer);
						//var content = /*Encoding.ASCII.GetString(buffer)*/ response;
						if(response.Code == StatusCode.Fail)
						{
							Console.WriteLine("An error occured.");
						}
						else
						{
							if (SensorType != default)
							{
								//message format: {sensor: "", data: ""}
								dynamic responseData = JsonConvert.DeserializeObject<dynamic>(response.Message);
								string sensor = responseData.sensor;
								int data = Convert.ToInt32(responseData.data);
								if(data > 3500)
								{
									Console.WriteLine($"sensor: {sensor} - {data} -> {Scenarios[SensorType][0]}");
								}
								else
								{
									Console.WriteLine($"sensor: {sensor} - {data} -> {Scenarios[SensorType][1]}");
								}
							}
						}
					}
				}
			}
			catch (SocketException)
			{
				Console.WriteLine("A server has disconnected.");
				Console.ReadLine();
				Environment.Exit(0);
			}
		}

		#region URLS
		private static string Subscribe(int action)
		{
			Console.WriteLine("Enter topic name(s) (multiple topics should be separated with a ';' symbol.): ");
			var topicNames = Console.ReadLine().Trim();
			return $"clientId={CLIENT_ID}?action={action}?topic={topicNames}";
		}

		private static string Publish(int action)
		{
			Console.WriteLine("Enter topic name: ");
			var topicName = Console.ReadLine().Trim();

			Console.WriteLine("Enter a message: ");
			var message = Console.ReadLine().Trim();

			return $"clientId={CLIENT_ID}?action={action}?topic={topicName}?payload={message}";
		}

		private static string Unsubscribe(int action)
		{
			Console.WriteLine("Unsubscribe from topics (specify topics you want to unsubscribe from, separate them by a semicolon): ");
			var topics = Console.ReadLine().Trim();
			return $"clientId={CLIENT_ID}?action={action}?topic={topics}";
		}

		private static string GetTopicListUrl(int action) => $"clientId={CLIENT_ID}?action={action}";
		#endregion
	}
}
