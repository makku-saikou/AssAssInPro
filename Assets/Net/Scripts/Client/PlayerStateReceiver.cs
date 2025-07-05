using Hmxs.Toolkit.Module.Events;
using Net.Scripts.Messages;
using UnityEngine;

namespace Net.Scripts.Client
{
    public class PlayerStateReceiver : MonoBehaviour
    {
        private int _playerId = 0;
        private Transform _player;
        private Vector2 _targetPosition;
        private Quaternion _targetRotation;
        private Vector2 _targetScale;
        private float _lerpSpeed = 10f;
        
        public void Init(Transform player, int playerID, float lerpSpeed = 10f)
        {
            if (player == null)
            {
                Debug.LogError("Player cannot be null");
                return;
            }
            _playerId = playerID;
            _player = player;
            _lerpSpeed = lerpSpeed;
        }

        private void OnEnable()
        {
            Events.AddListener<C2CPlayerState>(NetEvents.PlayerState, PlayerStateCallback);
        }
        
        private void OnDisable()
        {
            Events.RemoveListener<C2CPlayerState>(NetEvents.PlayerState, PlayerStateCallback);
        }

        private void Update()
        {
            UpdateTransform();
        }

        private void UpdateTransform()
        {
            if (Vector3.SqrMagnitude(_player.position - new Vector3(_targetPosition.x, _targetPosition.y, _player.position.z)) > 0.01f)
                _player.position = Vector3.Lerp(_player.position, _targetPosition, Time.deltaTime * _lerpSpeed); 
            // _player.rotation = Quaternion.Lerp(_player.rotation, _targetRotation, Time.deltaTime * _lerpSpeed);
            // _player.localScale = Vector3.Lerp(_player.localScale, _targetScale, Time.deltaTime * _lerpSpeed);
        }

        private void PlayerStateCallback(C2CPlayerState msg)
        {
            if (msg.PlayerId != _playerId) return;
            _targetPosition = msg.Position;
            // _targetRotation = msg.Rotation;
            // _targetScale = msg.Scale;
            // _player.position = msg.Position;
            _player.rotation = msg.Rotation;
            _player.localScale = msg.Scale;
            Debug.Log($"Player {_playerId} state updated: Position={_targetPosition}, Rotation={_targetRotation}, Scale={_targetScale}");
        }
    }
}