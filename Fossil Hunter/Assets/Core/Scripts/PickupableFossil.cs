using UnityEngine;

public class PickupableFossil : MonoBehaviour
{
    [SerializeField]
    private FossileInfo_SO data;

    public FossileInfo_SO Data { get => data; set { if (data == null) data = value; } }

    /// <summary>
    /// When the fossil is clicked on, send the data to the PickedUpFossils Scriptable Object & delete this gameobject
    /// </summary>
    public void PickUp()
    {
        Data.Found = true;
        PickedUpFossils.Instance.AddFossil(data);
        string fossils = "";
        foreach (FossileInfo_SO fossil in PickedUpFossils.Instance.GetFossils())
        {
            fossils += fossil.FossilType+", ";
        }
        Debug.Log($"All fossils: {fossils}");
        Destroy(gameObject);
    }
}
