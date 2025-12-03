using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartMenuManager : MonoBehaviour
{
    private Button sandBtn;
    private Button rockBtn;
    private Button museumBtn;
    private Button cleanerBtn;

    private void OnEnable()
    {
        var doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;

        sandBtn = root.Q<Button>("SandBeach");
        sandBtn.clicked += OnSandBeachPressed;

        rockBtn = root.Q<Button>("RockBeach");
        rockBtn.clicked += OnStoneBeachPressed;

        museumBtn = root.Q<Button>("Museum");
        museumBtn.clicked += OnMuseumPressed;

        cleanerBtn = root.Q<Button>("Cleaning");
        cleanerBtn.clicked += OnCleaningPressed;
    }
    private void OnDisable()
    {
        sandBtn.clicked -= OnSandBeachPressed;
        rockBtn.clicked -= OnStoneBeachPressed;
        museumBtn.clicked -= OnMuseumPressed;
        cleanerBtn.clicked -= OnCleaningPressed;
    }
    
    private void OnSandBeachPressed()
    {
        Debug.Log("Pls");
        SceneManager.LoadSceneAsync("Digging level 1", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("S_StartScene");
        MenuManager.currentScene = 4;
        Debug.Log(MenuManager.currentScene);
    }

    private void OnStoneBeachPressed()
    {
        SceneManager.LoadSceneAsync("Digging level 2", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("S_StartScene");
        MenuManager.currentScene = 5;
    }

    private void OnMuseumPressed()
    {
        SceneManager.LoadSceneAsync("S_Museum", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("S_StartScene");
        MenuManager.currentScene = 3;
    }

    private void OnCleaningPressed()
    {
        SceneManager.LoadSceneAsync("Cleaning level", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("S_StartScene");
        MenuManager.currentScene = 2;
    }
}
