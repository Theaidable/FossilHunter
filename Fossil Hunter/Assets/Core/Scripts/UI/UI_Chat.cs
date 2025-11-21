using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI_Handlers
{
    public class UI_Chat : MonoBehaviour
    {
        //Valgte UIDocument med en chat I
        [SerializeField] private UIDocument _uiDocument;

        private TextField _inputField;
        private Button _sendButton;
        private Button _closeButton;
        private ScrollView _messageScroll;

        private BoxCollider2D _collider;

        [SerializeField] private bool startHidden = false;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();

            if (startHidden == true)
            {
                _uiDocument.enabled = false;
            }
        }

        /// <summary>
        /// Event for når man klikker på objektets collider
        /// </summary>
        private void OnMouseDown()
        {
            if (_uiDocument != null)
            {
                _uiDocument.enabled = true;

                if (_collider != null)
                {
                    _collider.enabled = false;
                }

                GetOverlay();
            }
        }

        /// <summary>
        /// Få fat i UIDocumentets layout
        /// </summary>
        private void GetOverlay()
        {
            var root = _uiDocument.rootVisualElement;

            _inputField = root.Q<TextField>("InputField");
            _sendButton = root.Q<Button>("SendButton");
            _closeButton = root.Q<Button>("CloseButton");
            _messageScroll = root.Q<ScrollView>("MessageScroll");

            //Subscribe events
            _sendButton.clicked += OnSendClicked;
            _closeButton.clicked += OnCloseClicked;
        }

        /// <summary>
        /// Hvad der sker når man trykker "Send"
        /// Send en besked til serveren
        /// </summary>
        private void OnSendClicked()
        {
            Debug.Log("Send Button has been clicked");

            if(_inputField != null)
            {
                var message = _inputField?.value;

                string sender = $"Player {NetworkManager.Singleton.LocalClientId}";

                SendMessageToServer(sender, message);
            }
        }

        [ServerRpc]
        private void SendMessageToServer(string sender, string message, ServerRpcParams rpcParams = default)
        {
            string finalMessage = $"[{sender}] {message}";
            BroadcastMessageToAllClients(finalMessage);
        }

        [ClientRpc]
        private void BroadcastMessageToAllClients(string message)
        {
            if(_messageScroll != null)
            {
                Label label = new Label(message);
                _messageScroll.Add(label);

                _messageScroll.ScrollTo(label);
            }
        }

        /// <summary>
        /// Hvad der sker når man trykker "Close"
        /// Luk chatten
        /// </summary>
        private void OnCloseClicked()
        {
            //Unsubscribe events
            if (_sendButton != null)
            {
                _sendButton.clicked -= OnSendClicked;
            }

            if (_closeButton != null)
            {
                _closeButton.clicked -= OnCloseClicked;
            }

            //Luk Chatten
            _uiDocument.enabled = false;

            //Enable chat collider igen, så man kan åbne den
            _collider.enabled = true;
        }
    }
}
