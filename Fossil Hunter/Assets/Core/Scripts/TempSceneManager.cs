using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public class TempSceneManager : MonoBehaviour
{

    // Update is called once per frame

    void Start()
    {
       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) == true)
        {
            SceneManager.LoadSceneAsync("Scenes/SampleScene");
            Debug.Log("Beep");
        }
        if (Input.GetKeyDown(KeyCode.J) == true)
        {
            SceneManager.LoadSceneAsync("Scenes/Levels - Minigames/Digging level 1");
        }
        if (Input.GetKeyDown(KeyCode.S) == true)
        {
            Debug.Log(SceneManager.sceneCount);
        }
    }
}
