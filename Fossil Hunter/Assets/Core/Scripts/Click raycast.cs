using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class Clickraycast : MonoBehaviour
{
    LayerMask layerMask;
    [SerializeField]
    [Tooltip("The layer(s) that the click should not interact with")]
    string[] layerMasks;
    Camera mainCamera;

    void Awake()
    {
        layerMask = ~LayerMask.GetMask(layerMasks);
        mainCamera = Camera.main;
    }

    void Update()
    {
        //only check for raycast collision when the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition),Vector2.zero,0,layerMask);
            //if the raycast hit anything in the non-masked layers
            if (hit.collider!=null)
            {
                Debug.Log($"hit! {hit.collider.gameObject.name}");
            }
            //if it didn't
            else
            {
                Debug.Log("did not hit");
            }
        }

    }
}
