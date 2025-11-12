using UnityEngine;

public class PickupableFossil : MonoBehaviour
{
    [SerializeField]
    private FossilData data;

    public FossilData Data {  get { return data; } }

    /// <summary>
    /// When the fossil is clicked on, send the data to the PickedUpFossils Scriptable Object & delete this gameobject
    /// </summary>
    public void PickUp()
    {
        PickedUpFossils.Instance.AddFossil(data);
        string fossils = "";
        foreach (FossilData fossil in PickedUpFossils.Instance.GetFossils())
        {
            fossils += fossil.FossilName+", ";
        }
        Debug.Log($"All fossils: {fossils}");
        Destroy(gameObject);
    }
}
