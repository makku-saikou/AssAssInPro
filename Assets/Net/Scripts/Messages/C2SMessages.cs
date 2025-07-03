using Net.Scripts.Core;

namespace Net.Scripts.Messages
{
	public class RequestJoinGame : INetworkMessage
	{
		public int MessageId => 1001;

		public void Serialize(ByteBuffer buffer) { }
		public void Deserialize(ByteBuffer buffer) { }
	}
}
