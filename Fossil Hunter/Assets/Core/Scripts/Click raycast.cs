using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class Clickraycast : MonoBehaviour
{
    LayerMask layerMask;
    [SerializeField]
    [Tooltip("The layer(s) that the click should not interact with")]
    string[] layerHits;
    Camera mainCamera;

    void Awake()
    {
        layerMask = ~LayerMask.GetMask(layerHits);
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition),Vector2.zero,0,layerMask);
            if (hit.collider!=null)
            {
                Debug.Log($"hit! {hit.collider.gameObject.name}");
            }
            else
            {
                Debug.Log("did not hit");
            }
        }

    }
}
