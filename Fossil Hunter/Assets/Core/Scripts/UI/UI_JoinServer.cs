using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI_Handlers
{
    public class UI_JoinServer : MonoBehaviour
    {
        private UIDocument _joinServerDocument;

        private TextField _ipField;
        private Button _joinServerButton;

        private void Awake()
        {
            _joinServerDocument = GetComponent<UIDocument>();

            var root = _joinServerDocument.rootVisualElement;

            _ipField = root.Q<TextField>("IpField");
            _joinServerButton = root.Q<Button>("JoinServerButton");
        }

        private void OnEnable()
        {
            if(_joinServerButton != null)
            {
                _joinServerButton.clicked += OnJoinServerClicked;
            }

            if(NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
            }
        }

        private void OnDisable()
        {
            if (_joinServerButton != null)
            {
                _joinServerButton.clicked -= OnJoinServerClicked;
            }

            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
            }
        }

        private void OnJoinServerClicked()
        {
            if(NetworkManager.Singleton != null)
            {
                string ip = _ipField?.value;

                var transport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
                transport.ConnectionData.Address = ip;
                transport.ConnectionData.Port = 7777;

                NetworkManager.Singleton.StartClient();

                _joinServerButton.SetEnabled(false);

            }
            else
            {
                _ipField.value = "Ingen forbindelse til server";
            }
        }

        private void OnClientConnected(ulong clientId)
        {
            if (clientId != NetworkManager.Singleton.LocalClientId)
            {
                return;
            }

            SceneManager.LoadScene("S_MainScene");
        }

        private void OnClientDisconnected(ulong clientId)
        {
            if (clientId != NetworkManager.Singleton.LocalClientId)
            {
                return;
            }

            _joinServerButton.SetEnabled(true);
            _ipField.value = string.Empty;
        }
    }
}