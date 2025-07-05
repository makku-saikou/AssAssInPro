using System;
using Hmxs.Toolkit.Module.Events;
using Net.Scripts.Messages;
using Pditine.Player;
using UnityEngine;

namespace Net.Scripts.Client
{
    public class PlayerInputReceiver : MonoBehaviour
    {
        private NetPlayerController _player;

        public void Init(NetPlayerController player)
        {
            if (player == null)
            {
                Debug.LogError("PlayerController is null");
                return;
            }
            _player = player;
        }

        private void OnEnable()
        {
            Events.AddListener<C2CPlayerInput>(NetEvents.PlayerInput, PlayerInputCallback);
        }
        
        private void OnDisable()
        {
            Events.RemoveListener<C2CPlayerInput>(NetEvents.PlayerInput, PlayerInputCallback);
        }

        private void PlayerInputCallback(C2CPlayerInput msg)
        {
            if (msg.PlayerId != _player.ID) return;
            _player.Dash(msg.DashDirection, msg.DashStrength);

        }
    }
}