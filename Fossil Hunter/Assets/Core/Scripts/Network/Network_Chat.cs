using Session;
using System;
using System.Collections.Generic;
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

        //Event for når man modtager en besked fra server
        public event Action<string> OnMessageRecieved;

        //Liste for at gemme beskeder så nye clients kan se dem
        private readonly List<string> chatHistory = new List<string>();

        //Singleton
        private static Network_Chat instance;
        public static Network_Chat Instance {  get { return instance; } }

        private Network_Chat() { }

        /// <summary>
        /// Send besked til server
        /// </summary>
        /// <param name="message"></param>
        public void SendLocalMessage(string message, bool toTeacherOnly)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            string username = SessionData.Username;

            SendMessageToServerRpc(username, message, toTeacherOnly);
        }

        public void RequestHistoryFromServer()
        {
            if(IsClient == true)
            {
                Debug.Log("History from server has been requested");

                RequestHistoryServerRpc();
            }
        }

        /// <summary>
        /// Server modtager beskeden fra client gennem ServerRPC
        /// og sender beskeden til alle clients
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <param name="rpcParams"></param>
        [ServerRpc(RequireOwnership = false)]
        private void SendMessageToServerRpc(string senderUsername, string message, bool toTeacherOnly, ServerRpcParams rpcParams = default)
        {
            string prefix = toTeacherOnly ? "[Privat] " : "";
            string finalMessage = $"{prefix}[{senderUsername}] {message}";

            if(toTeacherOnly == true)
            {
                ulong teacherId = NetworkManager.ServerClientId;

                var rpcParamsToTeacherAndSender = new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new[] { teacherId }
                    }
                };

                SendMessageToClientRpc(finalMessage, rpcParamsToTeacherAndSender);
            }
            else
            {

                chatHistory.Add(finalMessage);
                SendMessageToClientRpc(finalMessage);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void RequestHistoryServerRpc(ServerRpcParams rpcParams = default)
        {
            ulong clientId = rpcParams.Receive.SenderClientId;

            var rpcParamsToClient = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new[] { clientId }
                }
            };

            foreach(var message in chatHistory)
            {
                SendMessageToClientRpc(message, rpcParamsToClient);
            }

            Debug.Log("History from server has been requested through RPC");
        }

        /// <summary>
        /// Sender beskeden til alle clients gennem ClientRpc
        /// </summary>
        /// <param name="message"></param>
        [ClientRpc]
        private void SendMessageToClientRpc(string message, ClientRpcParams rpcParams = default)
        {
            OnMessageRecieved?.Invoke(message);
        }

        /// <summary>
        /// Subscribe til events ved network spawn
        /// </summary>
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            DontDestroyOnLoad(gameObject);

            Debug.Log($"Network_Chat spawned on {(IsServer ? "SERVER" : "CLIENT")} – Owner: {OwnerClientId}");
        }
    }
}
