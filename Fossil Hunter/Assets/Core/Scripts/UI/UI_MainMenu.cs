using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI_Handlers
{
    /// <summary>
    /// Styrer UIen inde i vores Main Menu
    /// </summary>
    /// <author> David Gudmund Danielsen </author>
    public class UI_MainMenu : MonoBehaviour
    {
        //Fields

        //Reference til UIDocumentet
        private UIDocument _mainMenuDocument;

        //Buttons i UIDocument
        private Button buttonLogIn;
        private Button buttonSignUp;

        private void Awake()
        {
            _mainMenuDocument = GetComponent<UIDocument>();

            var root = _mainMenuDocument.rootVisualElement;

            buttonLogIn = root.Q<Button>("LogInButton");
            buttonSignUp = root.Q<Button>("SignUpButton");


        }

        /// <summary>
        /// Subscribe til button events
        /// </summary>
        private void OnEnable()
        {
            buttonLogIn.clicked += OnLogInClicked;
            buttonSignUp.clicked += OnSignUpClicked;
        }

        /// <summary>
        /// Unsubscribe events
        /// </summary>
        private void OnDisable()
        {
            buttonLogIn.clicked -= OnLogInClicked;
            buttonSignUp.clicked -= OnSignUpClicked;
        }

        /// <summary>
        /// Hvad der sker når man trykker "Log In"
        /// </summary>
        private void OnLogInClicked()
        {
            SceneManager.LoadScene("B_LogIn");
        }

        /// <summary>
        /// Hvad der sker når man trykker "Sign Up"
        /// </summary>
        private void OnSignUpClicked()
        {
            SceneManager.LoadScene("B_SignUp");
        }
    }
}
