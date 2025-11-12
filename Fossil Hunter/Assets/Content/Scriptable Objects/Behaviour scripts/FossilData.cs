using UnityEngine;

[CreateAssetMenu(fileName = "FossilData", menuName = "Scriptable Objects/FossilData")]
public class FossilData : ScriptableObject
{
    [SerializeField]
    [Tooltip("(not implemented yet)")]
    private Sprite sprite;
    [SerializeField]
    [Tooltip("(not implemented yet)")]
    private string fossilName;
    [SerializeField]
    [Tooltip("(not implemented yet) Must be 1 or higher (the further down, the higher the number)")]
    private int foundOnLayer;

    /// <summary>
    /// (not implemented yet)
    /// </summary>
    public Sprite Sprite { get { return sprite; } }
    /// <summary>
    /// (not implemented yet)
    /// </summary>
    public string FossilName { get { return fossilName; } }
    /// <summary>
    /// (not implemented yet) Must be 1 or higher (the further down, the higher the number)
    /// </summary>
    public int FoundOnLayer { get { return foundOnLayer; } }

}
