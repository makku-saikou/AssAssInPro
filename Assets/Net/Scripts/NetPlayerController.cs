using System;
using Hmxs.Scripts;
using Pditine.Audio;
using Pditine.Data;
using Pditine.Data.Player;
using Pditine.Player.Ass;
using Pditine.Player.Thorn;
using PurpleFlowerCore;
using PurpleFlowerCore.Utility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Pditine.Player
{
    public class NetPlayerController : PlayerController
    {
        public event Action<Vector2, float> OnDash; 
        
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
            // _currentDirection = InputDirection;
            // CurrentSpeed = CalculateSpeed();
            _currentBattery = 0;
            _currentRecoverCD = RecoverCD;
            OnDash?.Invoke(CurrentDirection, CurrentSpeed);
        }
    }
}