using System;
using Unity.Netcode;
using UnityEngine;

namespace Network_Handler
{
    /// <summary>
    /// Styrer netværk for chat-funktionen
    /// </summary>
    /// <author> David Gudmund Danielsen </author>
    public class Network_Chat : NetworkBehaviour
    {
        //Fields

        //Singleton pattern
        public static Network_Chat Instance { get; private set; }

        //Event for når man modtager en besked fra server
        public event Action<string> OnMessageRecieved;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Send besked til server
        /// </summary>
        /// <param name="message"></param>
        public void SendLocalMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            ulong sender = NetworkManager.Singleton.LocalClientId;

            SendMessageToServerRpc(sender, message);
        }

        /// <summary>
        /// Server modtager beskeden fra client gennem ServerRPC
        /// og sender beskeden til alle clients
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <param name="rpcParams"></param>
        [ServerRpc(RequireOwnership = false)]
        private void SendMessageToServerRpc(ulong sender, string message, ServerRpcParams rpcParams = default)
        {
            string finalMessage = $"[Player {sender}] {message}";

            SendMessageToClientRpc(finalMessage);
        }

        /// <summary>
        /// Sender beskeden til alle clients gennem ClientRpc
        /// </summary>
        /// <param name="message"></param>
        [ClientRpc]
        private void SendMessageToClientRpc(string message)
        {
            OnMessageRecieved?.Invoke(message);
        }

        /// <summary>
        /// Debug
        /// </summary>
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            Debug.Log($"Network_Chat spawned on {(IsServer ? "SERVER" : "CLIENT")} – Owner: {OwnerClientId}");
        }
    }
}
