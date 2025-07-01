using System;
using UnityEngine;

namespace Net.Scripts.Core
{
	public static class ByteBufferExtensions
	{
		#region Vector

		public static ByteBuffer WriteVector2(this ByteBuffer buffer, Vector2 value)
		{
			if (buffer == null) throw new ArgumentNullException(nameof(buffer));
			return buffer.WriteFloat(value.x).WriteFloat(value.y);
		}

		public static Vector2 ReadVector2(this ByteBuffer buffer)
		{
			if (buffer == null) throw new ArgumentNullException(nameof(buffer));
			var x = buffer.ReadFloat();
			var y = buffer.ReadFloat();
			return new Vector2(x, y);
		}

		public static ByteBuffer WriteVector3(this ByteBuffer buffer, Vector3 value)
		{
			if (buffer == null) throw new ArgumentNullException(nameof(buffer));
			return buffer.WriteFloat(value.x).WriteFloat(value.y).WriteFloat(value.z);
		}

		public static Vector3 ReadVector3(this ByteBuffer buffer)
		{
			if (buffer == null) throw new ArgumentNullException(nameof(buffer));
			var x = buffer.ReadFloat();
			var y = buffer.ReadFloat();
			var z = buffer.ReadFloat();
			return new Vector3(x, y, z);
		}

		#endregion


		#region Quaternion

		public static ByteBuffer WriteQuaternion(this ByteBuffer buffer, Quaternion value)
		{
			if (buffer == null) throw new ArgumentNullException(nameof(buffer));
			return buffer.WriteFloat(value.x).WriteFloat(value.y).WriteFloat(value.z).WriteFloat(value.w);
		}

		public static Quaternion ReadQuaternion(this ByteBuffer buffer)
		{
			if (buffer == null) throw new ArgumentNullException(nameof(buffer));
			var x = buffer.ReadFloat();
			var y = buffer.ReadFloat();
			var z = buffer.ReadFloat();
			var w = buffer.ReadFloat();
			return new Quaternion(x, y, z, w);
		}

		#endregion
	}
}
