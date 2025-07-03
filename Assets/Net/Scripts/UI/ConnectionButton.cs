using System;
using Hmxs.Toolkit.Flow.Timer;
using Net.Scripts.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Net.Scripts.UI
{
	[RequireComponent(typeof(Button))]
	public class ConnectionButton : MonoBehaviour
	{
		[SerializeField] private Color connectingColor;
		[SerializeField] private Color connectedColor;
		[SerializeField] private float autoConnectInterval = 60f;
		[SerializeField] private float connectTimeout = 3f;

		private Button _button;
		private Image _image;

		private void Start()
		{
			TcpClientManager.Instance.Connect();
			_button = GetComponent<Button>();
			_button.onClick.AddListener(() =>
			{
				if (TcpClientManager.Instance.IsConnected) return;
				TcpClientManager.Instance.Connect();
				_button.interactable = false;
				Timer.Register(connectTimeout, () =>
				{
					_button.interactable = true;
					if (TcpClientManager.Instance.IsConnected) return;
					Debug.LogWarning("Connection timed out. Please try again.");
				});
			});
			Timer.Register(autoConnectInterval, () =>
			{
				if (TcpClientManager.Instance.IsConnected) return;
				TcpClientManager.Instance.Connect();
			}, isLooped: true);
		}

		private void Update()
		{
			_button.image.color =
				TcpClientManager.Instance.IsConnected ?
				connectedColor :
				connectingColor;
		}
	}
}
