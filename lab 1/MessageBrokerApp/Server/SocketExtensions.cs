using System.Net.Sockets;
using System.Reflection;

namespace Server
{
	public static class SocketExtensions
	{
		public static bool IsDead(this Socket s)
		{
			BindingFlags bfIsDisposed = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetProperty;
			PropertyInfo field = s.GetType().GetProperty("CleanedUp", bfIsDisposed);

			return (bool) field.GetValue(s, null);
		}
	}
}
