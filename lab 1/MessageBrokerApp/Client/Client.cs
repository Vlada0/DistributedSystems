using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client
{
	class Client
	{
		public static Socket _socket;
		const int port = 4242;
		static Guid CLIENT_ID = Guid.Empty;
		//private const int BUFFER_SIZE = 2048;
		//private static readonly byte[] buffer = new byte[BUFFER_SIZE];

		static void Main(string[] args)
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
					_socket.Connect(ipEndpoint);
					connected = true;
					var thread = new Thread(GetClientId);
					thread.Start();
				}
				catch (Exception)
				{  
					Console.WriteLine($"Could not connect to host.");
					connected = false;
					Thread.Sleep(2000);
				}
			}

			while (true)
			{
				var nl = Environment.NewLine;
				Console.WriteLine($"Select an action you would like to perform:{nl}1.Publish{nl}2.Subscribe{nl}3.Unsubscribe");
				int.TryParse(Console.ReadLine(), out var input);
				switch (input)
				{
					case 1:
						var requestUrl = Publish();
						var bytesToSend = Encoding.Default.GetBytes(requestUrl);
						_socket.Send(bytesToSend);
						break;
					case 2:
						var url = Subscribe();
						var bytes = Encoding.ASCII.GetBytes(url);
						_socket.Send(bytes, 0, bytes.Length, SocketFlags.None);
						new Thread(ListenForMessages).Start();
						break;
					case 3:
						var link = Unsubscribe();
						var buffer = Encoding.ASCII.GetBytes(link);
						_socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
						break;
					default:
						break;
				}
			}
		}

		

		static void GetClientId()
		{
			byte[] buffer;
			int readBytes;
			var hasGot = false;
			try
			{
				while (!hasGot)
				{
					buffer = new byte[_socket.Available];
					readBytes = _socket.Receive(buffer, SocketFlags.None);
					if (readBytes > 0)
					{
						var content = Encoding.ASCII.GetString(buffer);
						Guid.TryParse(content, out CLIENT_ID);
						Console.WriteLine(CLIENT_ID);
						hasGot = true;
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
				while (true)
				{
					buffer = new byte[_socket.Available];
					readBytes = _socket.Receive(buffer, SocketFlags.None);
					if (readBytes > 0)
					{
						var content = Encoding.ASCII.GetString(buffer);
						Console.WriteLine($"Receive: {content}");
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

		private static string Subscribe()
		{
			Console.WriteLine("Enter topic name(s) (multiple topics should be separated with a ';' symbol.): ");
			var topicNames = Console.ReadLine().Trim();
			return $"clientId={CLIENT_ID}?action=2?topic={topicNames}";
		}

		private static string Publish()
		{
			Console.WriteLine("Enter topic name: ");
			var topicName = Console.ReadLine().Trim();

			Console.WriteLine("Enter a message: ");
			var message = Console.ReadLine().Trim();

			return $"clientId={CLIENT_ID}?action=1?topic={topicName}?payload={message}";
		}

		private static string Unsubscribe()
		{
			Console.WriteLine("Unsubscribe from topics (specify topics you want to unsubscribe from, separate them by a semicolon): ");
			var topics = Console.ReadLine().Trim();
			return $"clientId={CLIENT_ID}?action=3?topic={topics}";
		}
	}
}
