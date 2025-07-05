using Hmxs.Toolkit.Module.Events;

namespace Net.Scripts.Messages
{
	public static class MessageProcessing
	{
		public static void Dispatch(INetworkMessage message)
		{
			switch (message)
			{
				case S2CJoinGame joinGameResponse:
					Events.Trigger(MessageEvents.JoinGameResponse, joinGameResponse);
					break;
				case S2CGameEstablished gameEstablished:
					Events.Trigger(MessageEvents.GameEstablished, gameEstablished);
					break;
				case S2CGameStart gameStart:
					Events.Trigger(MessageEvents.GameStart, gameStart);
					break;
			}
		}
	}
}
