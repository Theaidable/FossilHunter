using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI_Handlers
{
    /// <summary>
    /// Styrer UIen inde i vores Log In
    /// </summary>
    public class UI_LogIn : MonoBehaviour
    {
        //Reference til UIDocumentet
        private UIDocument _logInDocument;

        //Reference til INDE i UIDocumentet
        private TextField _username;
        private TextField _password;
        private Button _logInButton;

        private void Awake()
        {
            _logInDocument = GetComponent<UIDocument>();

            var root = _logInDocument.rootVisualElement;

            _username = root.Q<TextField>("Username");
            _password = root.Q<TextField>("Password");
            _logInButton = root.Q<Button>("LogInButton");

            _logInButton.clicked += OnLogInClicked;
        }

        /// <summary>
        /// Hvad der sker når man trykker på "Log In"
        /// </summary>
        private void OnLogInClicked()
        {
            //Her kan vi skrive selve login koden, som hvor username og password skal sammenlignes med databasen
            //Databasen skal laves
            //Husk at password skal hases og saltes

            //Nem løsning nu for at komme videre til chat-funktionen

            if(_username.value == "Teacher" && _password.value != string.Empty)
            {
                NetworkManager.Singleton.StartHost();
                SceneManager.LoadScene("T_MainScene");
            }
            else if( _username.value == "Student" && _password.value != string.Empty)
            {
                NetworkManager.Singleton.StartClient();
                SceneManager.LoadScene("S_MainScene");
            }
            else
            {
                return;
            }
        }
    }
}
