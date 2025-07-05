using Net.Scripts.Core;
using Net.Scripts.Messages;
using Pditine.Data;
using UnityEngine;

namespace Net.Scripts.Client
{
    public class PlayerStateSender : MonoBehaviour
    {
        private Transform _player;
        private int _playerID;

        private void LateUpdate()
        {
            var msg = new C2CPlayerState
            {
                PlayerId = _playerID,
                Position = _player.position,
                Rotation = _player.rotation,
                Scale = _player.localScale
            };
            TcpClientManager.Instance.SendMessage(msg);
            Debug.Log($"PlayerStateSender: Sending player state for Player ID {_playerID} at position {_player.position}");
        }

        public void Init(Transform newPlayer, int playerID)
        {
            if (newPlayer == null)
            {
                Debug.LogError("New player transform is null");
                return;
            }

            _playerID = playerID;
            _player = newPlayer;
        }
    }
}