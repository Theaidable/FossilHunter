using System.Runtime.CompilerServices;
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

        [SerializeField] private bool startHidden = false;

        private void Awake()
        {
            var root = _uiDocument.rootVisualElement;

            _inputField = root.Q<TextField>("InputField");
            _sendButton = root.Q<Button>("SendButton");
            _closeButton = root.Q<Button>("CloseButton");
            _messageScroll = root.Q<ScrollView>("MessageScroll");
        }

        private void Start()
        {
            if (startHidden == true)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            if(_sendButton != null)
            {
                _sendButton.clicked += OnSendClicked;
            }

            if(_closeButton != null)
            {
                _closeButton.clicked += OnCloseClicked;
            }
        }

        private void OnDisable()
        {
            if (_sendButton != null)
            {
                _sendButton.clicked -= OnSendClicked;
            }

            if (_closeButton != null)
            {
                _closeButton.clicked -= OnCloseClicked;
            }
        }

        private void OnSendClicked()
        {

        }

        private void OnCloseClicked()
        {
            //Luk Chatten
            gameObject.SetActive(false);
        }
    }
}
