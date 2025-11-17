using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public class TempSceneManager : MonoBehaviour
{
    int currentScene = 3;
    

    void Start()
    {
       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) == true)
        {
            if (currentScene != 0)
            {
                currentScene--;
                SceneManager.LoadSceneAsync(currentScene);
                Debug.Log(currentScene);
            }
            else
            {
                currentScene = 3;
                SceneManager.LoadSceneAsync(currentScene);
                Debug.Log(currentScene);
            }
        }
        if (Input.GetKeyDown(KeyCode.J) == true)
        {
            SceneManager.LoadSceneAsync(1);
        }
        if (Input.GetKeyDown(KeyCode.S) == true)
        {
            Debug.Log(SceneManager.sceneCount);
        }
    }
}
