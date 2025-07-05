using System;
using Hmxs.Toolkit.Flow.Timer;
using Hmxs.Toolkit.Module.Events;
using Mirror.Examples.MultipleMatch;
using Net.Scripts.Core;
using Net.Scripts.Messages;
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
		[SerializeField] private float loadingTimeout = 10f;
		[SerializeField] [ReadOnly] private bool isJoiningGame;

		private void OnEnable()
		{
			Events.AddListener<S2CJoinGame>(MessageEvents.JoinGameResponse, S2CJoinGameCallback);
			Events.AddListener<S2CGameEstablished>(MessageEvents.GameEstablished, S2CGameEstablishedCallback);
			Events.AddListener<S2CGameStart>(MessageEvents.GameStart, S2CGameStartCallback);
		}

		private void OnDisable()
		{
			Events.RemoveListener<S2CJoinGame>(MessageEvents.JoinGameResponse, S2CJoinGameCallback);
			Events.RemoveListener<S2CGameEstablished>(MessageEvents.GameEstablished, S2CGameEstablishedCallback);
			Events.RemoveListener<S2CGameStart>(MessageEvents.GameStart, S2CGameStartCallback);
		}

		private void Start() => joinGameButton.onClick.AddListener(JoinGame);

		private void JoinGame()
		{
			if (!TcpClientManager.Instance.IsConnected || isJoiningGame) return;

			Debug.Log("Try joining game...");
			var joinGameMessage = new RequestJoinGame();
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
				Debug.LogWarning("Failed to join game.");
				return;
			}
			isJoiningGame = true;
			loadingMask.SetActive(false);
			joinGameButton.gameObject.SetActive(false);
			joinGameButton.interactable = true;
			if (s2C.PlayerId == 1) leftMask.SetActive(false);
			else rightMask.SetActive(false);
		}

		private void S2CGameEstablishedCallback(S2CGameEstablished s2C)
		{
			Debug.Log("Game established.");
			leftMask.SetActive(false);
			rightMask.SetActive(false);
		}

		private void S2CGameStartCallback(S2CGameStart s2C)
		{
			Debug.Log("Game started.");
			SceneSystem.LoadScene(10);
			// Additional logic for game start can be added here
		}
	}
}
