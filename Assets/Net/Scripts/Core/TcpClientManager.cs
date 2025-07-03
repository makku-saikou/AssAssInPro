using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Hmxs.Toolkit.Base.Singleton;
using Net.Scripts.Messages;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Net.Scripts.Core
{
	public class TcpClientManager : SingletonMono<TcpClientManager>
	{
		[SerializeField] private string serverIp = "154.8.159.99";
		[SerializeField] private int serverPort = 8888;

		[Title("Events")] public UnityEvent onConnected;
		public UnityEvent onDisconnected;

		private TcpClient _client;
		private NetworkStream _stream;
		private CancellationTokenSource _cancellationTokenSource;

		private readonly ConcurrentQueue<INetworkMessage> _sendQueue = new();
		private readonly ConcurrentQueue<INetworkMessage> _receiveQueue = new();

		public bool IsConnected => _client is { Connected: true };

		#region Unity Callbacks

		private void Update()
		{
			while (_receiveQueue.TryDequeue(out var message))
				ProcessReceivedMessage(message);
		}

		protected override void OnApplicationQuit()
		{
			base.OnApplicationQuit();
			Disconnect();
		}

		#endregion


		#region Connection

		[Button]
		public async void Connect()
		{
			try
			{
				if (IsConnected) return;

				_client = new TcpClient { NoDelay = true };
				_cancellationTokenSource = new CancellationTokenSource();

				try
				{
					await _client.ConnectAsync(serverIp, serverPort);
					_stream = _client.GetStream();
					Debug.Log($"Connected to server at {serverIp}:{serverPort}");
					_ = Task.Run(() => ReceiveLoopAsync(_cancellationTokenSource.Token));
					_ = Task.Run(() => SendLoopAsync(_cancellationTokenSource.Token));
					onConnected?.Invoke();
				}
				catch (Exception e)
				{
					Debug.LogError($"Failed to connect: {e.Message}");
					HandleDisconnection();
				}
			}
			catch (Exception e)
			{
				Debug.LogError($"Error during connection: {e.Message}");
				HandleDisconnection();
			}
		}

		[Button]
		public void Disconnect()
		{
			if (!IsConnected) return;
			HandleDisconnection();
		}

		private void HandleDisconnection()
		{
			_cancellationTokenSource?.Cancel();
			_stream?.Close();
			_client?.Close();
			_cancellationTokenSource = null;
			_stream = null;
			_client = null;
			while (_sendQueue.TryDequeue(out _)) { }
			while (_receiveQueue.TryDequeue(out _)) { }
			Debug.Log("Disconnected from server.");
			onDisconnected?.Invoke();
		}

		#endregion


		#region Send and Receive Loops

		private async Task SendLoopAsync(CancellationToken token)
		{
			while (!token.IsCancellationRequested)
			{
				if (_sendQueue.TryDequeue(out var message))
				{
					ByteBuffer buffer = ByteBufferPool.Instance.Get();
					try
					{
						buffer.BeginPacket(); // reserve space for length prefix
						buffer.WriteInt(message.MessageId); // write message ID
						message.Serialize(buffer); // write message data
						ArraySegment<byte> dataToSend = buffer.EndPacket(); // write length prefix
						await _stream.WriteAsync(dataToSend.Array, dataToSend.Offset, dataToSend.Count, token);
						Debug.Log($"Sent message ID: {message.MessageId}, Length: {dataToSend.Count}");
					}
					catch (Exception e)
					{
						Debug.LogError($"Send error: {e.Message}");
						HandleDisconnection();
						break;
					}
					finally
					{
						ByteBufferPool.Instance.Release(buffer);
					}
				}
				else
				{
					await Task.Delay(10, token); // wait a bit before checking the queue again
				}
			}
		}

		private async Task ReceiveLoopAsync(CancellationToken token)
		{
			var lengthBuffer = new byte[4];
			while (!token.IsCancellationRequested)
			{
				try
				{
					await ReadExactlyAsync(lengthBuffer, 0, 4, token); // read length
					int messageLength = BitConverter.ToInt32(lengthBuffer, 0);
					Debug.Log("receive bytes:" + messageLength);
					var messageBuffer = new byte[messageLength];
					await ReadExactlyAsync(messageBuffer, 0, messageLength, token); // read message based on length
					// deserialize message
					ByteBuffer buffer = ByteBufferPool.Instance.Get();
					try
					{
						buffer.LoadData(messageBuffer);
						int messageId = buffer.ReadInt();
						INetworkMessage message = MessageRegistry.CreateMessage(messageId);
						if (message != null)
						{
							message.Deserialize(buffer);
							_receiveQueue.Enqueue(message); // add to process queue
						}
						else
							Debug.LogWarning($"Unknown message ID: {messageId}");
					}
					finally
					{
						ByteBufferPool.Instance.Release(buffer);
					}
				}
				catch (Exception e)
				{
					Debug.LogError($"Receive error: {e.Message}");
					HandleDisconnection();
					break;
				}
			}
		}

		private async Task ReadExactlyAsync(byte[] buffer, int offset, int count, CancellationToken token)
		{
			int bytesRead = 0;
			while (bytesRead < count)
			{
				int read = await _stream.ReadAsync(buffer, offset + bytesRead, count - bytesRead, token);
				if (read == 0) throw new Exception("Connection closed by remote host.");
				bytesRead += read;
			}
		}

		#endregion


		#region API

		[Button]
		public void SendMessage(INetworkMessage message)
		{
			if (!IsConnected)
			{
				Debug.LogWarning("Not connected. Message not sent.");
				return;
			}
			_sendQueue.Enqueue(message);
		}

		[Button]
		public void SendMessage(int messageId)
		{
			INetworkMessage message = MessageRegistry.CreateMessage(messageId);
			if (message == null)
			{
				Debug.LogWarning($"No message found with ID {messageId}");
				return;
			}
			SendMessage(message);
		}

		#endregion


		#region Message Processing

		private static void ProcessReceivedMessage(INetworkMessage message)
		{
			MessageProcessing.Dispatch(message);
		}

		#endregion
	}
}
