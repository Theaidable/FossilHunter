using System.Net;
using System.Net.Sockets;
using Unity.Netcode;
using Unity.Netcode.Editor;
using UnityEngine;
using UnityEngine.UIElements;

public class TeacherMainSceneHandler : MonoBehaviour
{
    [SerializeField] private ChatNetwork chatNetworkPrefab;
    [SerializeField] private UIDocument overlayDocument;

    private Label _ipLabel;
    private Label _playersLabel;

    private void Awake()
    {
        if (overlayDocument != null)
        {
            var root = overlayDocument.rootVisualElement;
            _ipLabel = root.Q<Label>("IpText");
            _playersLabel = root.Q<Label>("PlayerText");
        }
    }


    private void Start()
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer)
        {
            // Spawn ChatNetwork
            var obj = Instantiate(chatNetworkPrefab);
            obj.GetComponent<NetworkObject>().Spawn();

            // Sæt IP-tekst
            if (_ipLabel != null)
            {
                _ipLabel.text = "Server IP: " + GetLocalIPAddress();
            }

            // Subscribe til client events
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientChanged;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientChanged;

            UpdatePlayersLabel();
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientChanged;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientChanged;
        }
    }

    private void OnClientChanged(ulong _)
    {
        UpdatePlayersLabel();
    }

    private void UpdatePlayersLabel()
    {
        if (_playersLabel == null || NetworkManager.Singleton == null)
        {
            return;
        }

        int total = NetworkManager.Singleton.ConnectedClients.Count;
        int students = Mathf.Max(0, total - 1); // fjern læreren selv

        _playersLabel.text = $"Elever tilsluttet: {students}";
    }

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
        catch { }

        return "IP ikke fundet";
    }
}
