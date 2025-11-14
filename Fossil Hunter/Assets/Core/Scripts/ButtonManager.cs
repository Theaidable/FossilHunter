using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/// <summary>
/// Tænkt til at styre alle knapperne, dette bliver en form for UI styrning
/// </summary>
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

    //Skifter scene til digging level, skal nok optimeres senere
    private void OnLeftPressed()
    {
        SceneManager.LoadSceneAsync("Scenes/Levels - Minigames/Digging level 1");
    }
    //Skal senere kunne skifte til en anden scene, fikser senere
    private void OnRightPressed()
    {
        Debug.Log("Right Pressed");
    }
}
