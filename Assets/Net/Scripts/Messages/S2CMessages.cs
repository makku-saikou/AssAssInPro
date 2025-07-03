using Net.Scripts.Core;

namespace Net.Scripts.Messages
{
	public static class S2CMessages
	{
		public class JoinGameResponse : INetworkMessage
		{
			public int MessageId => 2001;
			public bool Success;
			public int PlayerId;

			public void Serialize(ByteBuffer buffer)
			{
				buffer.WriteBool(Success);
				buffer.WriteInt(PlayerId);
			}

			public void Deserialize(ByteBuffer buffer)
			{
				Success = buffer.ReadBool();
				PlayerId = buffer.ReadInt();
			}
		}
	}
}
