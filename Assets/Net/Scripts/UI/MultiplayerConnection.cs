using Hmxs.Toolkit.Flow.Timer;
using Hmxs.Toolkit.Module.Events;
using Net.Scripts.Core;
using Net.Scripts.Messages;
using Pditine.Data;
using PurpleFlowerCore;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Net.Scripts.UI
{
	public class MultiplayerConnection : MonoBehaviour
	{
		[SerializeField] private Button joinGameButton;
		[SerializeField] private GameObject leftMask;
		[SerializeField] private GameObject rightMask;
		[SerializeField] private GameObject loadingMask;
		[SerializeField] private GameObject player1Text;
		[SerializeField] private GameObject player2Text;
		[SerializeField] private float loadingTimeout = 10f;
		[SerializeField] [ReadOnly] private bool isJoiningGame;

		private void OnEnable()
		{
			Events.AddListener<S2CJoinGame>(NetEvents.JoinGameResponse, S2CJoinGameCallback);
			Events.AddListener<S2CGameEstablished>(NetEvents.GameEstablished, S2CGameEstablishedCallback);
			Events.AddListener<S2CGameStart>(NetEvents.GameStart, S2CGameStartCallback);
		}

		private void OnDisable()
		{
			Events.RemoveListener<S2CJoinGame>(NetEvents.JoinGameResponse, S2CJoinGameCallback);
			Events.RemoveListener<S2CGameEstablished>(NetEvents.GameEstablished, S2CGameEstablishedCallback);
			Events.RemoveListener<S2CGameStart>(NetEvents.GameStart, S2CGameStartCallback);
		}

		private void Start() => joinGameButton.onClick.AddListener(JoinGame);

		private void JoinGame()
		{
			if (!TcpClientManager.Instance.IsConnected || isJoiningGame) return;

			Debug.Log("Try joining game...");
			var joinGameMessage = new C2SJoinGame();
			Debug.Log(joinGameMessage.MessageId);
			TcpClientManager.Instance.SendMessage(joinGameMessage);

			loadingMask.SetActive(true);
			joinGameButton.interactable = false;
			Timer.Register(loadingTimeout, () =>
			{
				if (isJoiningGame) return;
				loadingMask.SetActive(false);
				joinGameButton.interactable = true;
			});
		}

		private void S2CJoinGameCallback(S2CJoinGame s2C)
		{
			if (!s2C.Success)
			{
				isJoiningGame = false;
				loadingMask.SetActive(false);
				joinGameButton.gameObject.SetActive(true);
				joinGameButton.interactable = true;
				leftMask.SetActive(true);
				rightMask.SetActive(true);
				player1Text.SetActive(false);
				player2Text.SetActive(false);
				Debug.LogWarning("Failed to join game.");
				return;
			}
			isJoiningGame = true;
			loadingMask.SetActive(false);
			joinGameButton.gameObject.SetActive(false);
			joinGameButton.interactable = true;
			if (s2C.PlayerId == 1)
			{
				leftMask.SetActive(false);
				player1Text.SetActive(true);
			}
			else
			{
				rightMask.SetActive(false);
				player2Text.SetActive(true);
			}
			DataManager.Instance.PassingData.netPlayerID = s2C.PlayerId;
		}

		private void S2CGameEstablishedCallback(S2CGameEstablished s2C)
		{
			Debug.Log("Game established.");
			leftMask.SetActive(false);
			rightMask.SetActive(false);
			player1Text.SetActive(true);
			player2Text.SetActive(true);
		}

		private void S2CGameStartCallback(S2CGameStart s2C)
		{
			Debug.Log("Game started.");
			SceneSystem.LoadScene(10);
			// Additional logic for game start can be added here
		}
	}
}
