using Hmxs.Scripts;
using Net.Scripts.Client;
using Pditine.Data;
using Pditine.Player;
using Pditine.Player.Ass;
using Pditine.Player.Thorn;
using Pditine.Scripts.Data.DatePassing;
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
            }
            else
            {
                player1.AddComponent<PlayerStateReceiver>().Init(player1.transform, 1);
                player2.AddComponent<PlayerStateReceiver>().Init(player2.transform, 2);
                player2.AddComponent<PlayerInputSender>().Init(player2);
                var inputHandler = player2.AddComponent<MouseInputHandler>();
                player2.InputHandler = inputHandler;
            }
            //
            // BuffManager.Instance.Init(player1,player2);
            // UIManager.Instance.Init(player1,player2);
            
            // startEffect.PlayFeedbacks();
            // DelayUtility.Delay(4.7f,()=>
            // {
            //     PlayerCanMove(true);
            //     PlayerManager.Instance.SwitchMap("GamePlay");
            // });
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
    }
}