using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using Unity.VisualScripting;

//Author - Malthe

public static class MuseumItemManager
{
    // fossil placement index, fossil data
    private static Dictionary<int, FossileInfo_SO> museumFossildata = new Dictionary<int, FossileInfo_SO>();

    // fossil placement value, fossil object
    private static List<GameObject> museumFossilPlacements = new List<GameObject>();
    private static bool museumLoaded = false;

    /// <summary>
    /// Is called by the camera, when the musuem scene is opened.
    /// Makes the manager ready for operation.
    /// </summary>
    public static void InitializeManager()
    {
        // preps the fossil siloets
        if (!museumLoaded)
        {
            museumLoaded = true;

            // sets up an event to update the museum when it's switched the that scene.
            SceneManager.sceneLoaded += (scene, loadMode) =>
            {
                if (scene.name.Contains("Museum"))
                {
                    UpdateFossilObjects();
                    UpdateMuseum();
                    UpdateFossilData();
                    Debug.Log("musuem updated");
                }
            };
        }
    }



    /// <summary>
    /// Gathers and inserts the fossils to the museum.
    /// Is called after after the scene is loaded
    /// </summary>
    public static void UpdateMuseum()
    {

        foreach (FossileInfo_SO entry in PickedUpFossils.Instance.GetCleanedFossils())
        {
            UnlockFossil(entry);
        }
        PickedUpFossils.Instance.ClearCleanFossils();
    }

    private static void UpdateFossilData()
    {
        // gives the data to the right objects
        foreach (KeyValuePair<int, FossileInfo_SO> SO in museumFossildata)
        {
            museumFossilPlacements[SO.Key].GetComponent<MuseumUnlockable>().Unlock(SO.Value);

        }
    }

    /// <summary>
    /// Unlocks / Adds a fossil to the museum.
    /// </summary>
    /// <param name="sprite">The fossils <see cref="Sprite"/>.</param>
    /// <param name="fossileInfo">The fossils <see cref="FossileInfo_SO"/>.</param>
    private static void UnlockFossil(FossileInfo_SO fossileInfo)
    {
        // hvis der stadig er plads i museet.
        if (museumFossilPlacements.Count >= museumFossildata.Count)
        {
            //checks if there are any fossils of the same type.
            int entryID = 0;
            bool hasFossilOfSameType = museumFossildata.Any((entry) =>
            {
                entryID = entry.Key;
                return entry.Value.FossilType == fossileInfo.FossilType & entry.Value.Kvalitet != Kvalitet.Unik & fossileInfo.Kvalitet != Kvalitet.Unik;
            });

            
            if (hasFossilOfSameType)
            {
                if ((int)museumFossildata[entryID].Kvalitet < (int)fossileInfo.Kvalitet)
                {
                    museumFossildata[entryID] = fossileInfo;
                }
            }
            else // hvis du har fundet en ny type eller et unikt fossil.
            {
                museumFossildata.Add(museumFossildata.Count, fossileInfo);
            }
        }
    }

    /// <summary>
    /// Updates the fossil object. fx after scene change.
    /// </summary>
    private static void UpdateFossilObjects()
    {
        museumFossilPlacements.Clear();

        // finds the objects in the scene
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("MuseumItem"))
        {
            museumFossilPlacements.Add(go);
        }

        // sorts them acording to position (origo first i think)
        museumFossilPlacements.Sort((go1, go2) =>
        {
            Vector3 pos1 = go1.transform.position;
            Vector3 pos2 = go2.transform.position;

            return (int)(pos1.x + pos1.y * 10) - (int)(pos2.x + pos2.y * 10);
        });
    }
}
