using Net.Scripts.Core;
using Net.Scripts.Messages;
using Pditine.Player;
using UnityEngine;

namespace Net.Scripts.Client
{
    public class PlayerInputSender : MonoBehaviour
    {
        private PlayerController _player;

        public void Init(PlayerController player)
        {
            if (!player)
            {
                Debug.LogError("PlayerController is null");
                return;
            }
            _player = player;
            _player.OnDash += PlayerInputCallback;
        }

        // private void OnEnable()
        // {
        //     Debug.Log(_player);
        // }
        //
        // private void OnDisable()
        // {
        //     _player.OnDash -= PlayerInputCallback;
        // }

        private void PlayerInputCallback(Vector2 direction, float speed)
        {
            var msg = new C2CPlayerInput
            {
                PlayerId = _player.ID,
                DashDirection = direction,
                DashStrength = speed
            };
            TcpClientManager.Instance.SendMessage(msg);
        }
    }
}