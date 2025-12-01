using Database;
using Session;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI_Handlers
{
    /// <summary>
    /// Styrer UIen inde i vores Log In
    /// </summary>
    /// <author> David Gudmund Danielsen </author>
    public class UI_LogIn : MonoBehaviour
    {
        //Fields

        //Reference til UIDocumentet
        private UIDocument _logInDocument;

        //Reference til alt INDE i UIDocumentet
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

            //Opret test brugere
            UserDatabase.EnsureTestUsers();
        }

        /// <summary>
        /// Hvad der sker når man trykker på "Log In"
        /// </summary>
        private void OnLogInClicked()
        {
            string username = _username.value;
            string password = _password.value;

            if (UserDatabase.TryValidateUser(username, password, out var user, out string error) == false)
            {
                Debug.LogWarning($"Login failed: {error}");
                //Man kan vise fejl i UI
                return;
            }

            Debug.Log($"Login success: {user.Username} ({user.Role})");
            SessionData.Username = user.Username;

            switch (user.Role)
            {
                case UserRole.Teacher:
                    // Teacher: start host + gå til lærerens main scene
                    NetworkManager.Singleton.StartHost();
                    SceneManager.LoadScene("T_MainScene");
                    break;

                case UserRole.Student:
                    // Student: gå til "join server" flow
                    SceneManager.LoadScene("S_JoinServer");
                    break;
            }
        }
    }
}
