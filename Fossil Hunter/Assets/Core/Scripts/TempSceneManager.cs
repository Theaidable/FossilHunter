using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

/// <summary>
/// TempSceneManager er et testing script, den er der primært så at man kan skifte scener selv hvis knapperne ikke fungere. Fjernes nok eventually
/// </summary>
public class TempSceneManager : MonoBehaviour
{
    int currentScene = 1;

    private void Start()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.K) == true)
        {
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
        if (Input.GetKeyDown(KeyCode.J) == true)
        {
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
    }
}
