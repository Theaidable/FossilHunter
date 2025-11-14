using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class MuseumItemManager
{
    [SerializeField] private static List<GameObject> museumFossilSpots = new List<GameObject>();
    [SerializeField] private static int entries;
    private static bool museumLoaded = false;

    /// <summary>
    /// Is called by the camera, when the musuem scene is opened.
    /// Makes the manager ready for operation.
    /// </summary>
    public static void InitializeManager()
    {
        if (!museumLoaded)
        {
            museumFossilSpots.AddRange(GameObject.FindGameObjectsWithTag("MuseumItem"));
            museumLoaded = true;
        }

        Debug.Log(museumFossilSpots.Count);
    }

    /// <summary>
    /// Updates the fossils stored in the museum.
    /// Is called after after the scene is loaded
    /// </summary>
    public static void UpdateMuseum()
    {
        foreach (FossileInfo_SO entry in PickedUpFossils.Instance.GetFossils())
        {
            UnlockFossil(entry);
        }
    }

    /// <summary>
    /// Unlocks / Adds a fossil to the museum.
    /// </summary>
    /// <param name="sprite">The fossils <see cref="Sprite"/>.</param>
    /// <param name="fossileInfo">The fossils <see cref="FossileInfo_SO"/>.</param>
    private static void UnlockFossil(FossileInfo_SO fossileInfo)
    {
        if (museumFossilSpots.Count >= entries)
        {
            museumFossilSpots[entries].GetComponent<MuseumUnlockable>().Unlock(fossileInfo);
            entries++;
        }
    }
}
