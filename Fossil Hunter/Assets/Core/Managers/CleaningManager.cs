using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using Network_Handler;

// Author - Malthe

public static class CleaningManager
{
    private static GameObject dirtyFossilPrefab;
    private static GameObject waitinglistObjectPrefab;
    private static List<GameObject> waitinglistObjects = new List<GameObject>();

    /// <summary>
    /// Initializes the Manager.
    /// </summary>
    /// <param name="dirtyFossilPrefab">A prefab.</param>
    /// <param name="waitinglistObjectPrefab">A prefab.</param>
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

    /// <summary>
    /// Is called when the cleaning scene opens.
    /// Sets up for cleaning of the fossils.
    /// </summary>
    private static void OpenManager()
    {
        List<FossileInfo_SO> foundFossils = PickedUpFossils.Instance.GetFossils();

        //spawns and positions the waitinglist.
        for (int i = 0; i < foundFossils.Count; i++)
        {
            GameObject waitinglistObject = (GameObject)GameObject.Instantiate(waitinglistObjectPrefab, SceneManager.GetSceneByName("Cleaning level"));
            waitinglistObject.GetComponent<SpriteRenderer>().sprite = foundFossils[i].GetDirtySpite;
            waitinglistObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
            waitinglistObject.transform.position = new Vector3(7, 3 - i * 1.25f, 0);
            waitinglistObjects.Add(waitinglistObject);
        }

        SpawnNextFossil();
    }

    /// <summary>
    /// Spawns the next fossil to be cleaned.
    /// Is used mainly after a fossil is cleaned.
    /// </summary>
    public static void SpawnNextFossil()
    {
        if (waitinglistObjects.Count != 0)
        {
            //removes a fossil from the waitinglist.
            GameObject.Destroy(waitinglistObjects.Last());
            waitinglistObjects.RemoveAt(waitinglistObjects.Count - 1);

            //gets the data from a picked up fossil.
            FossileInfo_SO newFossilData = PickedUpFossils.Instance.GetFossils().Last();

            //makes a new cleanable object.
            GameObject newFossil = (GameObject)GameObject.Instantiate(dirtyFossilPrefab, SceneManager.GetSceneByName("Cleaning level"));
            newFossil.GetComponent<CleanableFossil>().Initialize(newFossilData);

        }
    }

    private static void TrySendFossilCleanedMessage(FossileInfo_SO fossil)
    {
        if(fossil == null || Network_Chat.Instance == null || Network_Chat.Instance.IsSpawned == false)
        {
            return;
        }

        string fossilName = fossil.FossilType.ToString();

        string colorHex;

        switch (fossil.Kvalitet)
        {
            case Kvalitet.Dårlig: // Garbage kvalitet
                colorHex = "FF0000"; // Rød farve
                break;
            case Kvalitet.Middle: // Common
                colorHex = "B0B0B0"; // Grå farve
                break;
            case Kvalitet.God: // Rare
                colorHex = "0080FF"; // Blå farve
                break;
            case Kvalitet.Perfekt: // Epic
                colorHex = "8000FF"; // Lilla farve
                break;
            case Kvalitet.Unik: // Legendary
                colorHex = "FFD700"; // Guld farve
                break;
            default:
                colorHex = "FFFFFF";
                break;
        }

        // Selve beskeden med fossilnavn og farve
        string message = $"<i>Har fundet <color=#{colorHex}>{fossilName}</color></i>";

        // Send besked til alle
        Network_Chat.Instance.SendLocalMessage(message, false);
    }

    /// <summary>
    /// Sends a fossil to the museum, after its been cleaned.
    /// </summary>
    /// <param name="fossil">The fossil being sent to the museum.</param>
    public static void FossilCleaned(FossileInfo_SO fossil)
    {
        TrySendFossilCleanedMessage(fossil);
        PickedUpFossils.Instance.CleanFossil(fossil);
        SpawnNextFossil();
    }
}
