using UnityEngine;
using UnityEngine.SceneManagement;

public class DiggingLevelManager : MonoBehaviour
{
    int scene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scene = Random.Range(1, 3);
        SceneManager.LoadSceneAsync($"Digging level {scene}", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
