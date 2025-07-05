using System;
using Pditine.Audio;
using UnityEngine;

namespace Pditine.Player
{
    public class NetPlayerController : PlayerController
    {
        public void Dash(Vector2 direction, float speed)
        {
            VFX[VFXName.ChargeDone].Stop();
            VFX[VFXName.Charging].Stop();
            if (!canMove) return;
            if (_isPause) return;
            _chargeDone = false;
            AAIAudioManager.Instance.PlayEffect("加速音效");
            _currentDirection = direction;
            CurrentSpeed = speed;
            _currentBattery = 0;
            _currentRecoverCD = RecoverCD;
        }

        public override void Dash()
        {
            VFX[VFXName.ChargeDone].Stop();
            VFX[VFXName.Charging].Stop();
            if (!canMove) return;
            if (_isPause) return;
            _chargeDone = false;
            AAIAudioManager.Instance.PlayEffect("加速音效");
            if (ID == 1)
            {
                _currentDirection = InputDirection;
                CurrentSpeed = CalculateSpeed();
            }
            _currentBattery = 0;
            _currentRecoverCD = RecoverCD;
        }
    }
}