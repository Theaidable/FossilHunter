using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/// <summary>
/// Hurtig kode til debug for at vælge om man er lærer eller elev
/// </summary>
/// /// <author> David Gudmund Danielsen </author>
public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;

    private Button _teacherButton;
    private Button _studentButton;

    private void Awake()
    {
        var root = _uiDocument.rootVisualElement;

        _teacherButton = root.Q<Button>("TeacherButton");
        _studentButton = root.Q<Button>("StudentButton");

        if (_teacherButton != null)
        {
            _teacherButton.clicked += OnTeacherClicked;
        }

        if (_studentButton != null)
        {
            _studentButton.clicked += OnStudentClicked;
        }
    }

    private void OnStudentClicked()
    {
        SceneManager.LoadScene("S_Menu");
    }

    private void OnTeacherClicked()
    {
        SceneManager.LoadScene("T_Menu");
    }
}
