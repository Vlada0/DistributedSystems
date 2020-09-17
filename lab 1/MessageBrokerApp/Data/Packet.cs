using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Data
{
	[Serializable]
	public class Packet
	{
		public Guid ClientId { get; private set; }
		public ClientAction Action { get; private set; }
		public string[] SensorTypes { get; private set; }
		public int? Data { get;private set; }

		public Packet(Guid clientId, ClientAction action, string[] sensors, int? data)
		{
			ClientId = clientId;
			Action = action;
			SensorTypes = sensors;
			Data = data;
		}

		public Packet(byte[] packetBytes)
		{
			var bf = new BinaryFormatter();
			using var ms = new MemoryStream(packetBytes);
			Packet p = bf.Deserialize(ms) as Packet;

			ClientId = p.ClientId;
			Action = p.Action;
			SensorTypes = p.SensorTypes;
			Data = p.Data;
		}

		public byte[] ToBytes()
		{
			var bf = new BinaryFormatter();
			using var ms = new MemoryStream();
			bf.Serialize(ms, this);
			byte[] bytes = ms.ToArray();

			return bytes;
		}
	}

	public enum PacketType
	{
		Registration = 1,
		Interaction
	}
}
