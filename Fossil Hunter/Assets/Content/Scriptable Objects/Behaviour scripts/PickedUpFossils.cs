using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PickedUpFossils", menuName = "Scriptable Objects/PickedUpFossils")]
public class PickedUpFossils : ScriptableObject
{
    [SerializeField]
    [Tooltip("Fossils that have been picked up are stored here")]
    private List<FossileInfo_SO> fossilsPickedUp = new List<FossileInfo_SO>();
    private static PickedUpFossils instance;

    //make this a singleton
    public static PickedUpFossils Instance
    {
        get 
        { if (instance == null)
            {
                instance = ScriptableObject.CreateInstance<PickedUpFossils>();
            }
        return instance;
        }
    }

    /// <summary>
    /// private constructor, feel free to add things
    /// </summary>
    private PickedUpFossils()
    {

    }

    /// <summary>
    /// Add a single fossilData to the list of fossils in inventory (can have duplicates)
    /// </summary>
    /// <param name="fossil">The fossilData to add</param>
    public void AddFossil(FossileInfo_SO fossil)
    {
        fossilsPickedUp.Add(fossil);
    }
    /// <summary>
    /// Get a list of all fossils in inventory
    /// </summary>
    /// <returns>Generic List of all given fossilData</returns>
    public List<FossileInfo_SO> GetFossils()
    {
        return fossilsPickedUp;
    }
    /// <summary>
    /// Removes singular fossilData from the inventory
    /// </summary>
    /// <param name="fossil">The fossilData to remove</param>
    public void RemoveFossil(FossileInfo_SO fossil)
    {
        if(fossilsPickedUp.Contains(fossil)) fossilsPickedUp.Remove(fossil);
    }
    /// <summary>
    /// Clear the entire fossil list
    /// </summary>
    public void ClearFossils()
    {
        fossilsPickedUp.Clear();
    }
}
