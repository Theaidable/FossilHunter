using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ButtonManager : MonoBehaviour
{
    private Button leftBtn;
    private Button rightBtn;

    private void OnEnable()
    {
        var doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;

        leftBtn = root.Q<Button>("LeftButton");
        leftBtn.clicked += OnLeftPressed;

        rightBtn = root.Q<Button>("RightButton");
        rightBtn.clicked += OnRightPressed;
    }

    private void OnDisable()
    {
        leftBtn.clicked -= OnLeftPressed;
        rightBtn.clicked -= OnRightPressed;
    }

    private void OnLeftPressed()
    {
        SceneManager.LoadSceneAsync("Scenes/Levels - Minigames/Digging level 1");
    }

    private void OnRightPressed()
    {
        Debug.Log("Right Pressed");
    }
}
