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
					Debug.Log($"{playerState.PlayerId}, {playerState.Position}, {playerState.Rotation}, {playerState.Scale}");
					Events.Trigger(NetEvents.PlayerState, playerState);
					break;
				case C2CPlayerHP playerHP:
					Events.Trigger(NetEvents.PlayerHP, playerHP);
					break;
				default:
					Debug.LogWarning("Undispatched message type: " + message.GetType());
					break;
			}
		}
	}
}
