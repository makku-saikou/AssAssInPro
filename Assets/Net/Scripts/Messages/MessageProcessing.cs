using Hmxs.Toolkit.Module.Events;
using UnityEngine;

namespace Net.Scripts.Messages
{
	public static class MessageProcessing
	{
		public static void Dispatch(INetworkMessage message)
		{
			switch (message)
			{
				case S2CJoinGame joinGameResponse:
					Events.Trigger(NetEvents.JoinGameResponse, joinGameResponse);
					break;
				case S2CGameEstablished gameEstablished:
					Events.Trigger(NetEvents.GameEstablished, gameEstablished);
					break;
				case S2CGameStart gameStart:
					Events.Trigger(NetEvents.GameStart, gameStart);
					break;
				case C2CPlayerInput playerInput:
					Events.Trigger(NetEvents.PlayerInput, playerInput);
					break;
				case C2CPlayerState playerState:
					Events.Trigger(NetEvents.PlayerState, playerState);
					break;
				default:
					Debug.LogWarning("Undispatched message type: " + message.GetType());
					break;
			}
		}
	}
}
