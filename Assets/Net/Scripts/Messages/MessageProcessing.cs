using Hmxs.Toolkit.Module.Events;

namespace Net.Scripts.Messages
{
	public static class MessageProcessing
	{
		public static void Dispatch(INetworkMessage message)
		{
			switch (message)
			{
				case S2CMessages.JoinGameResponse joinGameResponse:
					Events.Trigger(MessageEvents.JoinGameResponse, joinGameResponse);
					break;
			}
		}
	}
}
