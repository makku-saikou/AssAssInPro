using Hmxs.Toolkit.Flow.Timer;
using Hmxs.Toolkit.Module.Events;
using Net.Scripts.Core;
using Net.Scripts.Messages;
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

		private void Start()
		{
			joinGameButton.onClick.AddListener(JoinGame);
			Events.AddListener<S2CMessages.JoinGameResponse>(MessageEvents.JoinGameResponse, HandleJoinGameResponse);
		}

		private void JoinGame()
		{
			if (!TcpClientManager.Instance.IsConnected || isJoiningGame) return;

			Debug.Log("Try joining game...");
			var joinGameMessage = new C2SMessages.JoinGameRequest();
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

		private void HandleJoinGameResponse(S2CMessages.JoinGameResponse response)
		{
			if (!response.Success)
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
			if (response.PlayerId == 0) leftMask.SetActive(false);
			else rightMask.SetActive(false);
		}
	}
}
