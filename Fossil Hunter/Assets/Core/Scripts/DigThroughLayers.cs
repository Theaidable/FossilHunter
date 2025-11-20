using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DigThroughLayers : MonoBehaviour
{
    LayerMask layerMask;
    [SerializeField]
    [Tooltip("The layer(s) that the click should not interact with")]
    string[] layerMasks;
    Camera mainCamera;
    [SerializeField]
    float holeSize = 2;
    [SerializeField]
    Sprite holeSprite;
    ParticleSystem particles;
    ParticleSystem.EmitParams emitParams;


    void Awake()
    {
        layerMask = ~LayerMask.GetMask(layerMasks);
        mainCamera = Camera.main;
        particles = GetComponent<ParticleSystem>();
        emitParams = new ParticleSystem.EmitParams();
        emitParams.applyShapeToPosition = true;
        emitParams.startSize = 0.5f;

    }


    void Update()
    {
        //only check for raycast collision when the left mouse button is clicked

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0, layerMask);

            GetComponent<SFXManager>().DigSound();

            emitParams.position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            particles.Emit(emitParams, 15);

            //if the raycast hit anything in the non-masked layers, check for the colliders
            bool passThrough = false;
            foreach (RaycastHit2D hitCollider in hits)
            {
                Debug.Log($"hit! {hitCollider.collider.gameObject.name}");
                //if we've hit a sprite mask, aka a hole, pass through the layer onto the next
                if (hitCollider.collider.gameObject.TryGetComponent<SpriteMask>(out SpriteMask mask))
                {
                    Debug.Log("it's a sprite mask, moving on");
                    passThrough = true;
                }
                //turn off the boolean so we check the next layer instead of passing through
                else if (passThrough && hitCollider.collider.gameObject.tag != "Bottom Layer" && hitCollider.collider.gameObject.tag != "Fossil")
                {
                    passThrough = false;
                }
                else
                {
                    //if it's the last layer without a hole
                    Debug.Log("hit a dead end");
                    //if there are more layers underneath, make a hole (create a new spritemask) (not very efficient)
                    if (hitCollider.collider.gameObject.tag != "Bottom Layer" && hitCollider.collider.gameObject.GetComponent<PickupableFossil>() == null)
                    {
                        var v3 = Input.mousePosition;
                        v3 = mainCamera.ScreenToWorldPoint(v3);
                        GameObject hole = new GameObject();
                        hole.transform.position = new Vector3(v3.x, v3.y, 0);
                        hole.layer = hitCollider.collider.gameObject.layer;
                        SpriteMask sm = hole.AddComponent<SpriteMask>();
                        sm.sprite = holeSprite;
                        sm.isCustomRangeActive = true;
                        sm.frontSortingLayerID = SortingLayer.NameToID(LayerMask.LayerToName(hitCollider.collider.gameObject.layer));
                        sm.backSortingLayerID = SortingLayer.NameToID(LayerMask.LayerToName(hitCollider.collider.gameObject.layer + 1));
                        hole.transform.localScale = new Vector2(holeSize, holeSize);
                        hole.AddComponent<CircleCollider2D>();
                    }
                    //if we're clicking on a fossil, don't make a hole, instead pick up the fossil
                    else if (hitCollider.collider.gameObject.GetComponent<PickupableFossil>() != null)
                    {
                        GetComponent<SFXManager>().PickUpSound();
                        Debug.Log($"Picked up a {hitCollider.collider.gameObject.GetComponent<PickupableFossil>().Data.FossilType}");
                        hitCollider.collider.gameObject.GetComponent<PickupableFossil>().PickUp();
                    }
                    passThrough = false;
                    break;
                }
            }
            if (hits.Length < 1)
            {
                //if it didn't hit a collider
                Debug.Log("did not hit");
            }
        }

    }
}
