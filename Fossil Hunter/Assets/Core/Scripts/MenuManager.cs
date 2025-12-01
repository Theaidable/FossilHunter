using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/// <summary>
/// Tænkt til at styre alle knapperne, dette bliver en form for UI styrning
/// </summary>
/// <summary>
/// Tænkt til at styre alle knapperne, dette bliver en form for UI styrning
/// Af yours truly, Echo
/// </summary>
public class MenuManager : MonoBehaviour
{
    private Button leftBtn;
    private Button rightBtn;
    private Button exitBtn;
    [SerializeField]
    //Kan ændres til at sætte start scenen
    public static int currentScene = 1;

    private void Start()
    {
        SceneManager.LoadSceneAsync(currentScene, LoadSceneMode.Additive);
    }
    private void Update()
    {

    }
  

    //Når UI er interegeret med køres denne kode og håndtere events
    private void OnEnable()
    {
        var doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;

        exitBtn = root.Q<Button>("ExitButton");
        exitBtn.clicked += OnExitButtonPressed;

        //leftBtn = root.Q<Button>("LeftButton");
        //leftBtn.clicked += OnLeftPressed;

        //rightBtn = root.Q<Button>("RightButton");
        //rightBtn.clicked += OnRightPressed;
    }
    //Når UI er færdig med at blive interegeret med
    private void OnDisable()
    {
        //leftBtn.clicked -= OnLeftPressed;
        //rightBtn.clicked -= OnRightPressed;
    }

    private void OnExitButtonPressed()
    {
        SceneManager.LoadSceneAsync("StartScreen", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(currentScene);
    }


}
