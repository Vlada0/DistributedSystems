using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Server
{
	internal class Topic
	{
		public Guid TopicId { get; set; }
		public string Name { get; set; }
		public ICollection<string> Messages { get; set; } = new List<string>();
	}
}
