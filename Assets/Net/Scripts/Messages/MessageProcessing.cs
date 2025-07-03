using Hmxs.Toolkit.Module.Events;

namespace Net.Scripts.Messages
{
	public static class MessageProcessing
	{
		public static void Dispatch(INetworkMessage message)
		{
			switch (message)
			{
				case ResponseJoinGame joinGameResponse:
					Events.Trigger(MessageEvents.JoinGameResponse, joinGameResponse);
					break;
			}
		}
	}
}
