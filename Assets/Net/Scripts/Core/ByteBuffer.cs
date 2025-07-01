using System;
using System.IO;
using System.Text;

namespace Net.Scripts.Core
{
	public class ByteBuffer : IDisposable
	{
		private readonly MemoryStream _stream;
		private readonly BinaryWriter _writer;
		private readonly BinaryReader _reader;

		public ByteBuffer()
		{
			_stream = new MemoryStream(1024);
			_writer = new BinaryWriter(_stream, Encoding.UTF8);
			_reader = new BinaryReader(_stream, Encoding.UTF8);
		}


		#region API

		/// <summary>
		/// Load data into the buffer from a byte array.
		/// </summary>
		/// <param name="data"> source bytes </param>
		public void LoadData(byte[] data)
		{
			_stream.SetLength(0);
			_stream.Write(data, 0, data.Length);
			_stream.Position = 0;
		}

		/// <summary>
		/// Clear the stream and reset the position to the beginning.
		/// </summary>
		public void Clear()
		{
			_stream.SetLength(0);
			_stream.Position = 0;
		}

		/// <summary>
		/// Prepares the buffer for writing a packet by reserving 4 bytes for the length prefix.
		/// </summary>
		public void BeginPacket()
		{
			Clear();
			_writer.Write(0);
		}

		/// <summary>
		/// Finalizes the packet by writing the correct message length into the reserved space.
		/// </summary>
		/// <returns></returns>
		public ArraySegment<byte> EndPacket()
		{
			long packetLength = _stream.Position; // overall length
			int messageDataLength = (int)packetLength - 4; // message length
			_stream.Position = 0; // reset position to the beginning of the stream
			_writer.Write(messageDataLength); // write the length prefix
			_stream.Position = packetLength;
			return new ArraySegment<byte>(_stream.GetBuffer(), 0, (int)packetLength);
		}

		/// <summary>
		/// Converts the ByteBuffer's internal stream to a byte array, with a 4-byte integer length prefix.
		/// </summary>
		/// <returns>byte array with length prefix</returns>
		public ArraySegment<byte> ToByteSegmentWithLength()
		{
			byte[] payload = _stream.ToArray();
			Clear();
			_writer.Write(payload.Length);
			_writer.Write(payload);
			return new ArraySegment<byte>(_stream.GetBuffer(), 0, (int)_stream.Length);
		}

		public void Dispose()
		{
			_stream?.Dispose();
			_writer?.Dispose();
			_reader?.Dispose();
		}

		#endregion


		#region WRITE METHODS

		public ByteBuffer Write(byte[] data, int offset, int count)
		{
			_writer.Write(data, offset, count);
			return this;
		}

		public ByteBuffer WriteBool(bool value)
		{
			_writer.Write(value);
			return this;
		}

		public ByteBuffer WriteInt(int value)
		{
			_writer.Write(value);
			return this;
		}

		public ByteBuffer WriteFloat(float value)
		{
			_writer.Write(value);
			return this;
		}

		public ByteBuffer WriteString(string value)
		{
			byte[] stringBytes = Encoding.UTF8.GetBytes(value);
			_writer.Write(stringBytes.Length);
			_writer.Write(stringBytes);
			return this;
		}

		#endregion


		#region READ METHODS

		public bool ReadBool() => _reader.ReadBoolean();

		public int ReadInt() => _reader.ReadInt32();

		public float ReadFloat() => _reader.ReadSingle();

		public string ReadString()
		{
			int length = _reader.ReadInt32();
			byte[] stringBytes = _reader.ReadBytes(length);
			return Encoding.UTF8.GetString(stringBytes);
		}

		#endregion
	}
}
