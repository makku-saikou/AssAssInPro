using Net.Scripts.Core;
using UnityEngine;

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
	
	public class TestMessage2 : INetworkMessage
	{
		public int MessageId => 1001;
		public int i = 0;
		public void Serialize(ByteBuffer buffer)
		{
			buffer.WriteInt(i);
		}

		public void Deserialize(ByteBuffer buffer)
		{
			i = buffer.ReadInt();
		}
	}
	
	[CreateAssetMenu(fileName = "TestMessage0", menuName = "Net/TestMessage0")]
	public class TestMessage0 : ScriptableObject ,INetworkMessage
	{
		public int MessageId => 1002;
		public int i = 1;
		public void Serialize(ByteBuffer buffer)
		{
			buffer.WriteInt(i);
		}

		public void Deserialize(ByteBuffer buffer)
		{
			 i =  buffer.ReadInt();
		}
	}
	
	public class TestMessage1 : INetworkMessage
	{
		public int MessageId => 1003;
		public void Serialize(ByteBuffer buffer)
		{
		}

		public void Deserialize(ByteBuffer buffer)
		{
		}
	}
}
