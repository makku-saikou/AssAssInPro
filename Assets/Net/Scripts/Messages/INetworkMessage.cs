using Net.Scripts.Core;

namespace Net.Scripts.Messages
{
	public interface INetworkMessage
	{
		int MessageId { get; }
		void Serialize(ByteBuffer buffer);
		void Deserialize(ByteBuffer buffer);
	}
}
