using UnityEngine;

// Author - Malthe

public class StartMuseumManager : MonoBehaviour
{

    [Header("Cleaning Manager")]
    [SerializeField] private GameObject dirtyFossilPrefab;
    [SerializeField] private GameObject WaitlistObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MuseumItemManager.InitializeManager();
        CleaningManager.InitializeManager(dirtyFossilPrefab, WaitlistObject);
    }


    // Update is called once per frame
    void Update()
    {
        
    }



}
