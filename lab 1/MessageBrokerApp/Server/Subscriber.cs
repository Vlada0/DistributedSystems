using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
	internal class Subscriber
	{
		public Guid SubscriberId { get; set; }
		public ICollection<Guid> TopicIds { get; set; } = new List<Guid>();
	}
}
