using System;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Formålet med dette script er at håndtere UI til chat-funktionen
/// </summary>
/// <author> David Gudmund Danielsen </author>
public class ChatUIHandler : MonoBehaviour, IChatUI
{
    #region Fields
    [SerializeField] private UIDocument _uiDocument;
    private TextField _inputField;
    private Button _sendButton;
    private Button _closeButton;
    private ScrollView _messageScroll;

    [SerializeField] private bool startHidden = false;
    #endregion

    private void Awake()
    {
        var root = _uiDocument.rootVisualElement;

        _messageScroll = root.Q<ScrollView>("MessageScroll");
        _inputField = root.Q<TextField>("InputField");
        _sendButton = root.Q<Button>("SendButton");
        _closeButton = root.Q<Button>("CloseButton");

        if(startHidden == true)
        {
            gameObject.SetActive(false);
        }

        Debug.Log($"SendButton found: {_sendButton != null}");
        Debug.Log($"CloseButton found: {_closeButton != null}");
    }

    private void OnEnable()
    {
        if (_sendButton != null)
        {
            _sendButton.clicked += OnSendClicked;
        }

        if (_closeButton != null)
        {
            _closeButton.clicked += OnCloseClicked;
        }

        if (ChatNetwork.Instance != null)
        {
            ChatNetwork.Instance.RegisterChatUI(this);
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

        if (ChatNetwork.Instance != null)
        {
            ChatNetwork.Instance.UnRegisterChatUI(this);
        }
    }

    #region Helpers

    #region Button Events
    /// <summary>
    /// Event for hvad der skal ske når man trykker på Send knappen
    /// </summary>
    private void OnSendClicked()
    {
        //Fejlsikring
        if(ChatNetwork.Instance == null || _inputField == null)
        {
            return;
        }

        //Sæt en string til den værdi (Tekst) man skriver i input
        string text = _inputField.value;

        //Send beskeden til server
        ChatNetwork.Instance.SendLocalMessage(text);
        Debug.Log("Message has been send");

        //Clear ens besked (returnere inputfield til empty)
        _inputField.value = string.Empty;
    }

    /// <summary>
    /// Event for hvad der skal ske når man trykker på close knappen
    /// </summary>
    private void OnCloseClicked()
    {
        StudenChatOpener opener = FindFirstObjectByType<StudenChatOpener>();
        
        if (opener != null)
        {
            opener.EnableCollider();
        }

        //Luk Chatten
        gameObject.SetActive(false);
    }
    #endregion

    /// <summary>
    /// Interface implentation
    /// </summary>
    /// <param name="message"></param>
    public void AddMessage(string message)
    {
        //Fejlsikring
        if(_messageScroll == null)
        {
            return;
        }

        //Tilføj besked til UI'en (inde i MessageScroll) via en ny label
        var label = new Label(message);
        _messageScroll.Add(label);

        //Scroll ned til den nyeste besked
        _messageScroll.ScrollTo(label);
    }

    #endregion
}
