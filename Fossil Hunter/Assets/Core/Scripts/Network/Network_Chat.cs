using System;
using Unity.Netcode;

namespace Network_Handler
{
    public class Network_Chat : NetworkBehaviour
    {
        public static Network_Chat Instance { get; private set; }

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

        public void SendLocalMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            ulong sender = NetworkManager.Singleton.LocalClientId;

            SendMessageToServerRpc(sender, message);
        }

        [ServerRpc(InvokePermission = RpcInvokePermission.Everyone)]
        private void SendMessageToServerRpc(ulong sender, string message, ServerRpcParams rpcParams = default)
        {
            string finalMessage = $"[Player {sender}] {message}";

            SendMessageToClientRpc(finalMessage);
        }

        [ClientRpc]
        private void SendMessageToClientRpc(string message)
        {
            OnMessageRecieved?.Invoke(message);
        }


    }
}
