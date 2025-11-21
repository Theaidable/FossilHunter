using Network_Handler;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI_Handlers
{
    /// <summary>
    /// Universel UI handler for chat-funktionen
    /// Sørg for at have korrekt navngivning
    /// </summary>
    /// <author> David Gudmund Danielsen </author>
    public class UI_Chat : MonoBehaviour
    {
        //Fields

        //Reference til UIDocument
        [SerializeField] private UIDocument _uiDocument;

        //Reference til alt INDE i UIDocumentet
        private TextField _inputField;
        private Button _sendButton;
        private Button _closeButton;
        private ScrollView _messageScroll;
        private VisualElement _root;

        private BoxCollider2D _collider;

        [SerializeField] private bool startHidden = false;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            _root = _uiDocument.rootVisualElement;

            if (startHidden == true)
            {
                //_uiDocument.enabled = false;
                _root.style.display = DisplayStyle.None;
            }
            else
            {
                GetOverlay();
            }
        }

        private void Start()
        {
            //Subscribe til network objekt for at sende besked
            Network_Chat.Instance.OnMessageRecieved += OnMessageReceived;
        }

        /// <summary>
        /// Event for når man klikker på objektets collider
        /// </summary>
        private void OnMouseDown()
        {
            if(_root != null)
            {
                _root.style.display = DisplayStyle.Flex;

                if (_collider != null)
                {
                    _collider.enabled = false;
                }

                GetOverlay();
                
                Network_Chat.Instance.OnMessageRecieved += OnMessageReceived;
            }
        }

        /// <summary>
        /// Få fat i UIDocumentets layout
        /// </summary>
        private void GetOverlay()
        {
            _root = _uiDocument.rootVisualElement;
            _inputField = _root.Q<TextField>("InputField");
            _sendButton = _root.Q<Button>("SendButton");
            _closeButton = _root.Q<Button>("CloseButton");
            _messageScroll = _root.Q<ScrollView>("MessageScroll");

            //Subscribe events
            if(_sendButton != null)
            {
                _sendButton.clicked += OnSendClicked;
            }
            
            if(_closeButton != null)
            {
                _closeButton.clicked += OnCloseClicked;
            }
        }

        /// <summary>
        /// Hvad der sker når man trykker "Send"
        /// Send en besked til serveren
        /// </summary>
        private void OnSendClicked()
        {
            if (_inputField != null)
            {
                var message = _inputField?.value;
                Network_Chat.Instance.SendLocalMessage(message);
                _inputField.value = string.Empty;
            }
        }

        /// <summary>
        /// Hvad der sker når man modtager en besked fra serveren
        /// </summary>
        /// <param name="message"></param>
        private void OnMessageReceived(string message)
        {
            if(_messageScroll != null)
            {
                var label = new Label(message);
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

            if (Network_Chat.Instance != null)
            {
                Network_Chat.Instance.OnMessageRecieved -= OnMessageReceived;
            }

            //Luk chatten
            _root.style.display = DisplayStyle.None;

            //Enable chat collider igen, så man kan åbne den
            _collider.enabled = true;
        }
    }
}
