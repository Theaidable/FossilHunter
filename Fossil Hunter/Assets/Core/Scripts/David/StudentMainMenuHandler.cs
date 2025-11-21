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
    }

    private void OnEnable()
    {
        if (_joinButton != null)
        {
            _joinButton.clicked += OnJoinClicked;
        }

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
    }

    private void OnDisable()
    {
        if(_joinButton != null)
        {
            _joinButton.clicked -= OnJoinClicked;
        }

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }

    private void OnJoinClicked()
    {
        //Fejlsikring
        if(NetworkManager.Singleton == null)
        {
            return;
        }

        string ip = _ipField?.value;

        if (string.IsNullOrEmpty(ip))
        {
            return;
        }

        var transport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        transport.ConnectionData.Address = ip;
        transport.ConnectionData.Port = 7777;

        NetworkManager.Singleton.StartClient();

        _joinButton.SetEnabled(false);

    }

    private void OnClientConnected(ulong clientId)
    {
        if(clientId != NetworkManager.Singleton.LocalClientId)
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

        _joinButton.SetEnabled(true);
        _ipField.value = string.Empty;
    }
}
