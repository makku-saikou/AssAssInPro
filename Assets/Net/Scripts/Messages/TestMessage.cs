using Net.Scripts.Core;

namespace Net.Scripts.Messages
{
	public class TestMessage : INetworkMessage
	{
		public int MessageId => 1005;

		public int IntMessage = 114514;
		public string StringMessage = "114541";
		public float FloatMessage = 114.514f;

		public void Serialize(ByteBuffer buffer)
		{
			buffer.WriteInt(IntMessage);
			buffer.WriteString(StringMessage);
			buffer.WriteFloat(FloatMessage);
		}

		public void Deserialize(ByteBuffer buffer)
		{
			IntMessage = buffer.ReadInt();
			StringMessage = buffer.ReadString();
			FloatMessage = buffer.ReadFloat();
		}
	}
}
