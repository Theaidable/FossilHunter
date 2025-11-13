using UnityEngine;
using System.Collections.Generic;

public static class MuseumItemManager
{
    [SerializeField] private static List<GameObject> museumEntries = new List<GameObject>();
    [SerializeField] private static int entries;

    /// <summary>
    /// Unlocks / Adds a fossil to the museum.
    /// </summary>
    /// <param name="sprite">The fossils <see cref="Sprite"/>.</param>
    /// <param name="fossileInfo">The fossils <see cref="FossileInfo_SO"/>.</param>
    public static void UnlockFossil(Sprite sprite, FossileInfo_SO fossileInfo)
    {
        if (museumEntries.Count >= entries)
        {
            museumEntries[entries].GetComponent<MuseumUnlockable>().Unlock(sprite, fossileInfo);
            entries++;
        }

    }
}
