using System;
using Hmxs.Scripts;
using MoreMountains.Feedbacks;
using Pditine.Audio;
using Pditine.Data;
using Pditine.Data.Player;
using Pditine.Player.Ass;
using Pditine.Player.Thorn;
using PurpleFlowerCore;
using PurpleFlowerCore.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pditine.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region 属性

        [SerializeField] private int id;
        public int ID => id;

        //todo: 这数值系统太抽象了,需要重新设计一下
        
        [HideInInspector] public float speedCoefficientMulAdjustment = 1;
        [HideInInspector] public float speedCoefficientAddAdjustment = 0;
        
        public float SpeedCoefficient =>
            _theThorn.Data.SpeedCoefficient * speedCoefficientMulAdjustment + speedCoefficientAddAdjustment;

        [HideInInspector] public float frictionMulAdjustment = 1;
        [HideInInspector] public float frictionAddAdjustment = 0;

        private float _friction;

        public float Friction => _friction * frictionMulAdjustment +
                                 frictionAddAdjustment;

        [HideInInspector] public float weightMulAdjustment = 1;
        [HideInInspector] public float weightAddAdjustment = 0;

        public float Weight => (_theAss.Data.Weight + _theThorn.Data.Weight) * weightMulAdjustment +
                               weightAddAdjustment;
        
        [HideInInspector] public float energyMulAdjustment = 1;
        [HideInInspector] public float energyAddAdjustment = 0;
        private float Energy => _theAss.Data.Energy * energyMulAdjustment + energyAddAdjustment;

        [HideInInspector] public float hpMulAdjustment = 1;
        [HideInInspector] public int hpAddAdjustment = 0;
        public float HP => (int)(_theAss.Data.HP * hpMulAdjustment + hpAddAdjustment);

        [HideInInspector] public float atkMulAdjustment = 1;
        [HideInInspector] public int atkAddAdjustment = 0;
        public int ATK => (int)(_theThorn.Data.ATK * atkMulAdjustment + atkAddAdjustment);

        [Inspectable]private float _currentHP;
        public float CurrentHP => _currentHP;

        [Inspectable]private float _currentEnergy;
        public float CurrentEnergy => _currentEnergy;
        
        protected bool _chargeDone;
        protected bool ChargeDone => _chargeDone;

        [Inspectable]protected float _currentBattery;
        public float CurrentBattery => _currentBattery;
        
        protected float EnergyRecoverSpeed => Data.energyRecoverSpeed;
        protected float Battery => Data.battery;
        protected float RotateSpeed => Data.rotateSpeed;
        
        [HideInInspector][Inspectable] public float CurrentSpeed;
        protected Vector2 InputDirection;
        [ReadOnly] protected Vector2 _currentDirection;

        public Vector2 CurrentDirection
        {
            get => _currentDirection;
            set
            {
                OnChangeCurrentDirection?.Invoke(value.normalized);
                _currentDirection = value.normalized;
            }
        }

        private float _changingTime;

        //todo: 这数值系统太抽象了,需要重新设计一下
        #endregion

        #region 引用

        private ThornBase _theThorn;
        public ThornBase TheThorn => _theThorn;
        private AssBase _theAss;
        public AssBase TheAss => _theAss;
        private InputHandler _inputHandler;
        public InputHandler InputHandler
        {
            get
            {
                if (!_inputHandler)
                {
                    _inputHandler= id == 1 ? PlayerManager.Instance.Handler1 : PlayerManager.Instance.Handler2;
                }
                return _inputHandler;
            }
            set => _inputHandler = value;
        }
            

        /// <summary>
        /// 用于操作表现上的玩家Scale和Rotation等变化,考虑封装
        /// </summary>
        [SerializeField]private Transform entity;
        public Transform Entity => entity;
        
        // 运行时不会改变的玩家数据，为空时使用默认数据
        [SerializeField]private PlayerData data;

        public PlayerData Data
        {
            get
            {
                if (data == null)
                    data = DataManager.Instance.DefaultPlayerData;
                return data;
            }
        }

        #endregion

        #region 其他变量
        
        [ReadOnly] public bool canMove;
        [ReadOnly] public bool isInvincible;
        protected bool _isPause;
        public bool IsPause => _isPause;

        //todo:重构对scale的操作
        [HideInInspector] public float targetScale;

        //todo:临时添加的标记
        [SerializeField] protected bool isAI;
        public bool IsAI => isAI;
        
        protected float RecoverCD => Data.recoverCD;

        protected float _currentRecoverCD;
        
        #endregion

        #region 事件

        // public event Action<float> OnChangeCD;
        
        /// <summary>
        /// 当前剩余能量 最大能量 电池能量
        /// </summary>
        public event Action<float,float,float> OnChangeEnergy;
        public event Action<float> OnChanging;
        public event Action OnEndChanging;
        public event Action<float> OnTryChangeHP; // 血量变化量
        public event Action<float, int> OnChangeHP; // 当前血量 玩家id
        public event Action OnDestroyed;
        public Action<Vector3> OnChangeCurrentDirection;

        
        #endregion

        #region 特效

        [SerializeField] private PlayerVFX vfx;
        public PlayerVFX VFX => vfx;
        
        #endregion

        private void OnEnable()
        {
            EventSystem.AddEventListener("Pause", Pause);
            EventSystem.AddEventListener("UnPause", UnPause);
        }

        private void OnDisable()
        {
            EventSystem.RemoveEventListener("Pause", Pause);
            EventSystem.RemoveEventListener("UnPause", UnPause);
        }

        private void Pause() => _isPause = true;
        private void UnPause() => _isPause = false;

        private void FixedUpdate()
        {
            UpdateTransform();
            ReduceSpeed();
        }

        private void Update()
        {
            // UpdateCD();
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
            if (InputHandler is null) return;
            if (InputHandler.Dash) Dash();
            if (InputHandler.Charge) 
                Charge(); 
            else 
                RecoverEnergy();
            if (InputHandler.Direction.sqrMagnitude != 0) ChangeDirection(InputHandler.Direction);
        }

        public void Init(ThornBase theThorn, AssBase theAss, PlayerData playerData = null)
        {
            if (playerData != null)
            {
                data = playerData;
            }
            _currentDirection = transform.right;
            targetScale = transform.localScale.x;
            _theAss = theAss;
            _theThorn = theThorn;
            _currentHP = HP;
            _friction = Weight;
            _currentEnergy = Energy;
            vfx.Init(this);
            OnInit();
        }

        protected virtual void OnInit() { }

        public virtual void ChangeDirection(Vector3 direction)
        {
            if (!canMove) return;
            if (_isPause) return;
            if (InputHandler.Device == Device.Mouse || InputHandler.Device == Device.TouchScreen)
            {
                InputDirection = (Camera.main.ScreenToWorldPoint(direction) - transform.position).normalized;
                InputDirection = InputDirection.normalized; // LJH:奇怪的bug
            }
            else
                InputDirection = direction.normalized;

            OnChangeCurrentDirection?.Invoke(InputDirection);
        }

        protected virtual void Charge()
        {
            if (!canMove) return;
            if (_isPause) return;
            //todo: 消耗速度?
            if (_chargeDone) return;
            if (_currentEnergy <= 0) _currentEnergy = 0;
            if (_currentBattery >= Battery)
            {
                VFX[VFXName.Charging].Stop();
                VFX[VFXName.ChargeDone].Play();
                _chargeDone = true;
                OnEndChanging?.Invoke();
                _currentBattery = Mathf.Min(Battery, _currentBattery);
                
            }
            else
            {
                VFX[VFXName.Charging].Play();
                _currentEnergy -= Time.deltaTime * EnergyRecoverSpeed * 1.5f;
                if(_currentEnergy < 0) 
                    _currentEnergy = 0;
                else
                    _currentBattery += Time.deltaTime * EnergyRecoverSpeed * 1.5f;
                OnChanging?.Invoke(_currentBattery);
            }
            OnChangeEnergy?.Invoke(_currentEnergy, Energy, _currentBattery);
        }

        public virtual void Dash()
        {
            VFX[VFXName.ChargeDone].Stop();
            VFX[VFXName.Charging].Stop();
            if (!canMove) return;
            if (_isPause) return;
            _chargeDone = false;
            AAIAudioManager.Instance.PlayEffect("加速音效");
            _currentDirection = InputDirection;
            CurrentSpeed = CalculateSpeed();
            _currentBattery = 0;
            _currentRecoverCD = RecoverCD;
        }
        
        protected void RecoverEnergy()
        {
            _currentRecoverCD -= Time.deltaTime;
            if (_currentRecoverCD > 0) return;
            _currentEnergy += Time.deltaTime * EnergyRecoverSpeed;
            if (_currentEnergy >= Energy) _currentEnergy = Energy;
            OnChangeEnergy?.Invoke(_currentEnergy, Energy, _currentBattery);
        }

        private void ReduceSpeed()
        {
            CurrentSpeed -= Friction * Time.deltaTime;
            if (CurrentSpeed <= 0) CurrentSpeed = 0;
        }

        private void UpdateTransform()
        {
            transform.position += (Vector3)_currentDirection * (CurrentSpeed * Time.deltaTime);
            transform.right = Vector3.Lerp(transform.right, _currentDirection, RotateSpeed);
        }
        
        protected virtual float CalculateSpeed()
        {
            return _currentBattery * SpeedCoefficient * 0.05f;
        }

        public void ChangeHP(float delta)
        {
            if (delta < 0 && isInvincible)
            {
                OnTryChangeHP?.Invoke(delta);
                return;
            }
            OnTryChangeHP?.Invoke(delta);
            _currentHP += delta;
            if (_currentHP > HP) _currentHP = HP;
            OnChangeHP?.Invoke(_currentHP, ID);
        }

        public void BeDestroy()
        {
            canMove = false;

            DelayUtility.Delay(0.5f, () =>
            {
                VFX[VFXName.Dead].Play();
                _theThorn.gameObject.SetActive(false);
                _theAss.gameObject.SetActive(false);
            });

            OnDestroyed?.Invoke();
        }

        public void ResetProperty()
        {
            //todo:这数值系统太抽象了,需要重新设计一下
            speedCoefficientMulAdjustment = 1;
            atkMulAdjustment = 1;
            weightMulAdjustment = 1;
            energyMulAdjustment = 1;
            frictionMulAdjustment = 1;
            hpMulAdjustment = 1;

            atkAddAdjustment = 0;
            frictionAddAdjustment = 0;
            hpAddAdjustment = 0;
            speedCoefficientAddAdjustment = 0;
            weightAddAdjustment = 0;
            energyAddAdjustment = 0;
        }
        
    }
}