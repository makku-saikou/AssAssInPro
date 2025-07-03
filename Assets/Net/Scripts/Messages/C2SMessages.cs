using Net.Scripts.Core;

namespace Net.Scripts.Messages
{
	public static class C2SMessages
	{
		public class JoinGameRequest : INetworkMessage
		{
			public int MessageId => 1001;

			public void Serialize(ByteBuffer buffer) { }
			public void Deserialize(ByteBuffer buffer) { }
		}
	}
}
