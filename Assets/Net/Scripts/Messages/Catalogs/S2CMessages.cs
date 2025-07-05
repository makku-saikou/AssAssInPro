using Net.Scripts.Core;

namespace Net.Scripts.Messages
{
	public class S2CJoinGame : INetworkMessage
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

	public class S2CGameEstablished : INetworkMessage
	{
		public int MessageId => 2002;
		public void Serialize(ByteBuffer buffer) { }
		public void Deserialize(ByteBuffer buffer) { }
	}

	public class S2CGameStart : INetworkMessage
	{
		public int MessageId => 2003;
		public void Serialize(ByteBuffer buffer) { }
		public void Deserialize(ByteBuffer buffer) { }
	}

	public class S2CGameOver : INetworkMessage
	{
		public int MessageId => 2004;

		public void Serialize(ByteBuffer buffer) { }

		public void Deserialize(ByteBuffer buffer) { }
	}
}
