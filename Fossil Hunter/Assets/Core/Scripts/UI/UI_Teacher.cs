using Network_Handler;
using System;
using System.Net;
using System.Net.Sockets;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UIElements;


namespace UI_Handlers
{
    /// <summary>
    /// Styrer UIen inde i vores Teacher Scene
    /// </summary>
    public class UI_Teacher : MonoBehaviour
    {
        [SerializeField] private Network_Chat NetworkChatPrefab;

        private UIDocument _teacherSceneDocument;

        private Label _ipLabel;
        private Label _playersLabel;

        private void Awake()
        {
            _teacherSceneDocument = GetComponent<UIDocument>();

            var root = _teacherSceneDocument.rootVisualElement;

            _ipLabel = root.Q<Label>("IpLabel");
            _playersLabel = root.Q<Label>("PlayersLabel");
        }

        private void Start()
        {
            if (NetworkManager.Singleton != null)
            {
                // Subscribe til client events
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientChanged;
                NetworkManager.Singleton.OnClientDisconnectCallback += OnClientChanged;
            }

            if (NetworkManager.Singleton.IsServer == true)
            {
                var obj = Instantiate(NetworkChatPrefab);
                obj.GetComponent<NetworkObject>().Spawn();
            }

            // Sæt IP-tekst
            if (_ipLabel != null)
            {
                _ipLabel.text = "Server IP: " + GetLocalIPAddress();
            }

            UpdatePlayersLabel();
        }

        private void OnDestroy()
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientChanged;
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientChanged;
            }
        }

        /// <summary>
        /// Opdatere PlayersLabel når en client forbinder eller forlader
        /// </summary>
        /// <param name="_"></param>
        private void OnClientChanged(ulong _)
        {
            UpdatePlayersLabel();
        }

        /// <summary>
        /// Skriv antallet af elever tilsluttet serveren
        /// </summary>
        private void UpdatePlayersLabel()
        {
            // Sæt nummer af players
            if (_playersLabel != null)
            {
                if (NetworkManager.Singleton != null)
                {
                    int total = NetworkManager.Singleton.ConnectedClients.Count;
                    int students = Mathf.Max(0, total - 1);

                    _playersLabel.text = $"Players: {students}";
                }
                else
                {
                    _playersLabel.text = $"Players: Server ikke sat op";
                }
            }
        }

        /// <summary>
        /// Få PC'ens lokale IP adresse og skriv det i feltet
        /// </summary>
        /// <returns></returns>
        private string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            catch 
            {
                return "IP ikke fundet";
            }

            return "IP ikke fundet";
        }
    }
}