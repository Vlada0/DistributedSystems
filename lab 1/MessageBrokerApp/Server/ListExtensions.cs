using System.Collections.Generic;

namespace Server
{
	public static class ListExtensions
	{
		public static void RemoveRange<T>(this IList<T> src, IEnumerable<T> rangeToRemove)
		{
			foreach (var item in rangeToRemove)
			{
				src.Remove(item);
			}
		}
	}
}
