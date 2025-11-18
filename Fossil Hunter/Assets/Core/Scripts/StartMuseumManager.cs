using UnityEngine;

public class StartMuseumManager : MonoBehaviour
{
    [SerializeField] private GameObject museumItemPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MuseumItemManager.InitializeManager(museumItemPrefab);
    }


    // Update is called once per frame
    void Update()
    {
        
    }



}
