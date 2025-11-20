using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/// <summary>
/// Handler for elevernes main menu
/// Opretter en client når man tilslutter sig lærerens server
/// </summary>
/// <author> David Gudmund Danielsen </author>
public class StudentMainMenuHandler : MonoBehaviour
{
    #region Fields
    [SerializeField] private UIDocument _uiDocument;

    private TextField _ipField;
    private Button _joinButton;

    #endregion

    private void Awake()
    {
        var root = _uiDocument.rootVisualElement;
        _ipField = root.Q<TextField>("IpField");
        _joinButton = root.Q<Button>("JoinServerButton");

        if(_joinButton != null)
        {
            _joinButton.clicked += OnJoinClicked;
        }
    }

    private void OnJoinClicked()
    {
        string ip = _ipField.value;

        var transport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        transport.ConnectionData.Address = ip;
        transport.ConnectionData.Port = 7777;

        NetworkManager.Singleton.StartClient();
        SceneManager.LoadScene("S_MainScene");
    }
}
