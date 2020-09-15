using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//codes: Client Registration - 0; Success - 1; Error - 2;
namespace Data
{
	[Serializable]
	public class Response
	{
		public string Message { get; private set; }
		public StatusCode Code { get; private set; }

		public Response(string message, StatusCode code)
		{
			Message = message;
			Code = code;
		}

		public Response(byte[] bytes)
		{
			var bf = new BinaryFormatter();
			using var ms = new MemoryStream(bytes);
			var resp = bf.Deserialize(ms) as Response;

			Message = resp.Message;
			Code = resp.Code;
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
}
