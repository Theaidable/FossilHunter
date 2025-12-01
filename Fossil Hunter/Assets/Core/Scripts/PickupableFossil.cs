using UnityEngine;

public class PickupableFossil : MonoBehaviour
{
    [SerializeField] private FossileInfo_SO data;
    [SerializeField] private ParticleSystem pickupEffectPrefab;

    public FossileInfo_SO Data { get => data; set { if (data == null) data = value; } }

    /// <summary>
    /// When the fossil is clicked on, send the data to the PickedUpFossils Scriptable Object,
    /// play particle effect and delete this gameobject
    /// </summary>
    public void PickUp()
    {
        // Spawn and play the particle effect
        if (pickupEffectPrefab != null)
        {
            var effect = Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
            effect.Play();

            // Cleanup the particle effect after it has finished playing
            var main = effect.main;
            Destroy(effect.gameObject, main.duration + main.startLifetime.constantMax);
        }

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
