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
public class SceneButtonManager : MonoBehaviour
{
    private Button leftBtn;
    private Button rightBtn;
    [SerializeField]
    //Kan ændres til at sætte start scenen
    private int currentScene = 2;

    private void Start()
    {
        SceneManager.LoadSceneAsync(currentScene, LoadSceneMode.Additive);
    }

    //Når UI er interegeret med køres denne kode og håndtere events
    private void OnEnable()
    {
        var doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;

        leftBtn = root.Q<Button>("LeftButton");
        leftBtn.clicked += OnLeftPressed;

        rightBtn = root.Q<Button>("RightButton");
        rightBtn.clicked += OnRightPressed;
    }
    //Når UI er færdig med at blive interegeret med
    private void OnDisable()
    {
        leftBtn.clicked -= OnLeftPressed;
        rightBtn.clicked -= OnRightPressed;
    }

    //Skifter scene til scenen over den nuværrende scene
    private void OnLeftPressed()
    {
        GetComponent<AudioSource>().Play();
        if (currentScene != 3)
        {
            SceneManager.UnloadSceneAsync(currentScene);
            currentScene++;
            SceneManager.LoadSceneAsync(currentScene, LoadSceneMode.Additive);
            Debug.Log(currentScene);
        }
        else
        {
            SceneManager.UnloadSceneAsync(currentScene);
            currentScene = 1;
            SceneManager.LoadSceneAsync(currentScene, LoadSceneMode.Additive);
            Debug.Log(currentScene);
        }
    }
    //Kan skifte scenen til værdien en under den nuværrende scene
    private void OnRightPressed()
    {
        GetComponent<AudioSource>().Play();
        if (currentScene != 1)
        {
            SceneManager.UnloadSceneAsync(currentScene);
            currentScene--;
            SceneManager.LoadSceneAsync(currentScene, LoadSceneMode.Additive);
            Debug.Log(currentScene);
        }
        else
        {
            SceneManager.UnloadSceneAsync(currentScene);
            currentScene = 3;
            SceneManager.LoadSceneAsync(currentScene, LoadSceneMode.Additive);
            Debug.Log(currentScene);
        }
    }
}
