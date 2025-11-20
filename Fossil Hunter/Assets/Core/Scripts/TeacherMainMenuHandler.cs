using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/// <summary>
/// Handler for lærerens main menu
/// Opretter en host/server som elever kan tilslutte når man trykker på knappen
/// </summary>
/// <author> David Gudmund Danielsen </author>
public class TeacherMainMenuHandler : MonoBehaviour
{
    #region Fields
    [SerializeField] private UIDocument _uiDocument;

    private Button _createServer;

    #endregion

    private void Awake()
    {
        var root = _uiDocument.rootVisualElement;

        _createServer = root.Q<Button>("CreateServerButton");

        if(_createServer != null)
        {
            _createServer.clicked += OnCreateServerClicked;
        }
    }

    #region Button Events
    private void OnCreateServerClicked()
    {
        NetworkManager.Singleton.StartHost();
        SceneManager.LoadScene("T_Scene");
    }
    #endregion
}
