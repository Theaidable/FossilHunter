using Database;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI_Handlers
{
    /// <summary>
    /// Styrer UIen for vores Sign Up
    /// </summary>
    /// <author> David Gudmund Danielsen </author>
    public class UI_SignUp : MonoBehaviour
    {
        private UIDocument _signUpDocument;

        private TextField _newUsername;
        private TextField _newPassword;
        private Button _signUpButton;

        [SerializeField] private UserRole role = UserRole.Student;

        private void Awake()
        {
            _signUpDocument = GetComponent<UIDocument>();

            var root = _signUpDocument.rootVisualElement;

            _newUsername = root.Q<TextField>("NewUsername");
            _newPassword = root.Q<TextField>("NewPassword");
            _signUpButton = root.Q<Button>("SignUpButton");

            if(_signUpButton != null)
            {
                _signUpButton.clicked += OnSignUpClicked;
            }
        }

        /// <summary>
        /// Event for hvad der skal ske når man trykker på "Sign Up"
        /// </summary>
        private void OnSignUpClicked()
        {
            string username = _newUsername.value;
            string password = _newPassword.value;

            if(UserDatabase.TryCreateUser(username,password, role, out string error) == false)
            {
                Debug.LogWarning($"SignUp failed: {error}");
                return;
            }

            Debug.Log($"SignUp succes for '{username}' as {role}");
            SceneManager.LoadScene("B_Login");
        }
    }
}