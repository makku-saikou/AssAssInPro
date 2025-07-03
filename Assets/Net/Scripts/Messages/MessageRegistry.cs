using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Net.Scripts.Messages
{
	public static class MessageRegistry
	{
		private static readonly Dictionary<int, Func<INetworkMessage>> Registry = new();

		[RuntimeInitializeOnLoadMethod]
		private static void Initialize()
		{
			Registry.Clear();

			Assembly assembly = Assembly.GetExecutingAssembly();
			var messageTypes = assembly.GetTypes().Where(
				t => typeof(INetworkMessage).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract
			);

			var enumerable = messageTypes.ToList();
			Debug.Log($"[MessageRegistry] Found {enumerable.Count()} message types to register.");

			foreach (var type in enumerable)
			{
				try
				{
					if (Activator.CreateInstance(type) is not INetworkMessage tempInstance)
					{
						Debug.LogWarning($"[MessageRegistry] Could not create instance of type {type.Name}. Skipping.");
						continue;
					}

					int messageId = tempInstance.MessageId;
					if (Registry.TryGetValue(messageId, out var messageConstructor))
					{
						Debug.LogWarning(
							$"[MessageRegistry] Duplicate Message ID '{messageId}' found for types {type.Name} and {messageConstructor.Method.DeclaringType?.Name}. Overwriting.");
					}
					Registry[messageId] = () => Activator.CreateInstance(type) as INetworkMessage;
					Debug.Log($"[MessageRegistry] Registered message type '{type.Name}' with ID '{messageId}'.");
				}
				catch (Exception ex)
				{
					Debug.LogError(
						$"[MessageRegistry] Failed to register message type {type.Name}. Error: {ex.Message}");
				}
			}
		}

		public static INetworkMessage CreateMessage(int messageId)
		{
			if (Registry.TryGetValue(messageId, out var messageConstructor))
				return messageConstructor();
			Debug.LogError($"[MessageRegistry] No message factory found for ID: {messageId}");
			return null;
		}
	}
}
