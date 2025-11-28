using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

// Author - Malthe

public static class CleaningManager
{
    private static GameObject dirtyFossilPrefab;
    private static GameObject waitinglistObjectPrefab;
    private static List<GameObject> waitinglistObjects = new List<GameObject>();

    public static void InitializeManager(GameObject dirtyFossilPrefab, GameObject waitinglistObjectPrefab)
    {
        CleaningManager.dirtyFossilPrefab = dirtyFossilPrefab;
        CleaningManager.waitinglistObjectPrefab = waitinglistObjectPrefab;

        // sets up an event to update the museum when it's switched the that scene.
        SceneManager.sceneLoaded += (scene, loadMode) =>
        {
            if (scene.name.Contains("Cleaning"))
            {
                OpenManager();
            }
        };
    }


    private static void OpenManager()
    {
        int foundFossils = PickedUpFossils.Instance.GetFossils().Count;

        for (int i = 0; i < foundFossils; i++)
        {
            GameObject waitinglistObject = (GameObject)GameObject.Instantiate(waitinglistObjectPrefab, SceneManager.GetSceneByName("Cleaning level"));
            waitinglistObject.transform.position = new Vector3(5, 4 + i * 1.5f, 0);
            waitinglistObjects.Add(waitinglistObject);
        }

        SpawnNextFossil();
    }

    public static void SpawnNextFossil()
    {
        if (waitinglistObjects.Count != 0)
        {
            GameObject.Destroy(waitinglistObjects.Last());
            waitinglistObjects.RemoveAt(waitinglistObjects.Count - 1);

            FossileInfo_SO newFossilData = PickedUpFossils.Instance.GetFossils().Last();

            GameObject newFossil = (GameObject)GameObject.Instantiate(dirtyFossilPrefab, SceneManager.GetSceneByName("Cleaning level"));
            newFossil.GetComponent<CleanableFossil>().Initialize(newFossilData);

        }
    }

    public static void FossilCleaned(FossileInfo_SO fossil)
    {
        PickedUpFossils.Instance.CleanFossil(fossil);
        SpawnNextFossil();
    }
}
