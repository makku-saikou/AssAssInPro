using System;
using System.Linq;
using Hmxs.Scripts;
using Hmxs.Toolkit.Module.Events;
using MoreMountains.Feedbacks;
using Net.Scripts.Client;
using Net.Scripts.Core;
using Net.Scripts.Messages;
using Pditine.Data;
using Pditine.GamePlay.UI;
using Pditine.Player;
using Pditine.Player.Ass;
using Pditine.Player.Thorn;
using Pditine.Scripts.Data.DatePassing;
using PurpleFlowerCore;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEngine;

namespace Net.Scripts
{
    public class NetGameManager : MonoBehaviour
    {
        private PassingData PassingData => DataManager.Instance.PassingData;
        public int ClientID => PassingData.netPlayerID;
        [SerializeField] private NetPlayerController player1;
        [SerializeField] private NetPlayerController player2;
        [SerializeField] protected MMF_Player startEffect;
        public void Start()
        {
            Init();
        }
        
        protected virtual void Init()
        {
            //组装玩家
            CreatePlayer(0,0,player1);
            CreatePlayer(0,0,player2);
            player1.canMove = true;
            player2.canMove = true;
            
            if (ClientID == 1)
            {
                player1.AddComponent<PlayerStateSender>().Init(player1.transform, 1);
                var inputHandler = player1.AddComponent<MouseInputHandler>();
                player1.InputHandler = inputHandler;
                player2.AddComponent<PlayerStateSender>().Init(player2.transform, 2);
                player2.AddComponent<PlayerInputReceiver>().Init(player2);
                player1.OnChangeHP += PlayerHPSendCallback;
                player2.OnChangeHP += PlayerHPSendCallback;
            }
            else
            {
                player1.GetComponents<Collider2D>().ForEach(c => c.enabled = false);
                player2.GetComponents<Collider2D>().ForEach(c => c.enabled = false);
                
                player1.AddComponent<PlayerStateReceiver>().Init(player1.transform, 1);
                player2.AddComponent<PlayerStateReceiver>().Init(player2.transform, 2);
                player2.AddComponent<PlayerInputSender>().Init(player2);
                var inputHandler = player2.AddComponent<MouseInputHandler>();
                player2.InputHandler = inputHandler;
            }
            //
            // BuffManager.Instance.Init(player1,player2);
            UIManager.Instance.Init(player1,player2);
            
            startEffect.PlayFeedbacks();
            player1.OnChangeHP += CheckPlayerDead;
            player2.OnChangeHP += CheckPlayerDead;
            // DelayUtility.Delay(4.7f,()=>
            // {
            //     PlayerCanMove(true);
            //     PlayerManager.Instance.SwitchMap("GamePlay");
            // });
        }

        private void OnEnable()
        {
            Events.AddListener<S2CGameOver>(NetEvents.GameOver, GameOverCallback);
            if(ClientID == 2)
                Events.AddListener<C2CPlayerHP>(NetEvents.PlayerHP,  PlayerHPReceiveCallback); 
        }
        
        private void OnDisable()
        {
            if(ClientID == 2)
                Events.RemoveListener<C2CPlayerHP>(NetEvents.PlayerHP, PlayerHPReceiveCallback);
        }

        private void PlayerHPSendCallback(float currentHP, int playerID)
        {
            var msg = new C2CPlayerHP
            {
                PlayerId = playerID,
                CurrentHP = currentHP
            };
            TcpClientManager.Instance.SendMessage(msg);
        }
        
        private void PlayerHPReceiveCallback(C2CPlayerHP msg)
        {
            if (msg.PlayerId == 1)
            {
                player1.SetHP(msg.CurrentHP);
            }
            else if (msg.PlayerId == 2)
            {
                player2.SetHP(msg.CurrentHP);
            }
        }
        
        protected virtual void CreatePlayer(int assID,int thornID,PlayerController thePlayer)
        {
            AssBase theAss = Instantiate(DataManager.Instance.GetAssData(assID).Prototype).GetComponent<AssBase>();
            ThornBase theThorn = Instantiate(DataManager.Instance.GetThornData(thornID).Prototype).GetComponent<ThornBase>();
            Debug.Log(thePlayer);
            Debug.Log(theThorn);
            thePlayer.Init(theThorn,theAss); // 保证先初始化PlayerController
            theAss.Init(thePlayer);
            theThorn.Init(thePlayer);
        }

        private void CheckPlayerDead(float hp, int playerID)
        {
            if (hp > 0) return;
            if (ClientID != 1) return;
            TcpClientManager.Instance.SendMessage(new C2SGameOver());
        }

        private void GameOverCallback(S2CGameOver _)
        {
            SceneSystem.LoadScene(0);
        }
    }
}
