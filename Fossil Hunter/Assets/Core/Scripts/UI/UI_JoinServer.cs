using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI_Handlers
{
    /// <summary>
    /// Styrer UIen inde i JoinServer Scenen
    /// </summary>
    /// <author> David Gudmund Danielsen </author>
    public class UI_JoinServer : MonoBehaviour
    {
        //Fields

        //Reference til UIDocument
        private UIDocument _joinServerDocument;

        //Reference til alt INDE i UIDocumentet
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

        /// <summary>
        /// Hvad der sker når man trykker på "Join Server" knappen
        /// </summary>
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

        /// <summary>
        /// Hvad der sker når client forbindes til serveren
        /// Skift til Student Main Scene
        /// </summary>
        /// <param name="clientId"></param>
        private void OnClientConnected(ulong clientId)
        {
            if (clientId != NetworkManager.Singleton.LocalClientId)
            {
                return;
            }

            SceneManager.LoadScene("S_MainScene");
        }

        /// <summary>
        /// Hvad der sker når Client disconnecter
        /// Gå tilbage til første hovedmenu
        /// </summary>
        /// <param name="clientId"></param>
        private void OnClientDisconnected(ulong clientId)
        {
            if (clientId != NetworkManager.Singleton.LocalClientId)
            {
                return;
            }

            SceneManager.LoadScene("B_MainMenu");
        }
    }
}