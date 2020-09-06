using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Server
{
	[Serializable]
	internal class Topic
	{
		public Guid TopicId { get; set; }
		public string Name { get; set; }
		public ICollection<string> Messages { get; set; } = new List<string>();

		public byte[] ToBytes()
		{
			BinaryFormatter bf = new BinaryFormatter();
			using var ms = new MemoryStream();
			bf.Serialize(ms, this);
			return ms.ToArray();
		}
	}
}
