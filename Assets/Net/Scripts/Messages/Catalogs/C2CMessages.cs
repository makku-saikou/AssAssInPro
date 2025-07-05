using Net.Scripts.Core;
using UnityEngine;

namespace Net.Scripts.Messages
{
	public class C2CPlayerInput : INetworkMessage
	{
		public int MessageId => 3001;

		public int PlayerId;

		public Vector2 DashDirection;

		public float DashStrength;

		public void Serialize(ByteBuffer buffer)
		{
			buffer.WriteInt(PlayerId);
			buffer.WriteVector2(DashDirection);
			buffer.WriteFloat(DashStrength);
		}

		public void Deserialize(ByteBuffer buffer)
		{
			PlayerId = buffer.ReadInt();
			DashDirection = buffer.ReadVector2();
			DashStrength = buffer.ReadFloat();
		}
	}

	public class C2CPlayerState : INetworkMessage
	{
		public int MessageId => 3002;

		public int PlayerId;

		public Vector2 Position;

		public Quaternion Rotation;

		public Vector2 Scale;

		public void Serialize(ByteBuffer buffer)
		{
			buffer.WriteInt(PlayerId);
			buffer.WriteVector2(Position);
			buffer.WriteQuaternion(Rotation);
			buffer.WriteVector2(Scale);
		}

		public void Deserialize(ByteBuffer buffer)
		{
			PlayerId = buffer.ReadInt();
			Position = buffer.ReadVector2();
			Rotation = buffer.ReadQuaternion();
			Scale = buffer.ReadVector2();
		}
	}

	public class C2CPlayerHP : INetworkMessage
	{
		public int MessageId => 3003;
		public int PlayerId;
		public float CurrentHP;
		public void Serialize(ByteBuffer buffer)
		{
			buffer.WriteInt(PlayerId);
			buffer.WriteFloat(CurrentHP);
		}

		public void Deserialize(ByteBuffer buffer)
		{
			PlayerId = buffer.ReadInt();
			CurrentHP = buffer.ReadFloat();
		}
	}
}
