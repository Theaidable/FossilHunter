using UnityEngine;

public class MuseumUnlockable : MonoBehaviour
{
    [SerializeField] private bool isUnlocked;
    [SerializeField] private FossileInfo_SO fossileInfo;
    private Collider2D collider;

    public bool IsUnlocked
    {
        get => isUnlocked;
        set
        {
            // makes it a silouet if it isn't unlocked
            isUnlocked = value;
            gameObject.GetComponent<SpriteRenderer>().color = (IsUnlocked ? Color.white : Color.black);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // makes it a silouet if it isn't unlocked
        gameObject.GetComponent<SpriteRenderer>().color = (IsUnlocked ? Color.white : Color.black);
        collider = gameObject.GetComponent<CircleCollider2D>();

        // giver en standard-værdi hvis ingen er givet.
        if (fossileInfo is null)
        {
            fossileInfo = ScriptableObject.CreateInstance<FossileInfo_SO>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //only check for raycast collision when the left mouse button is clicked and the pop-up is closed
        if (Input.GetMouseButtonDown(0) & !InfoPopUpManager.Open)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            //if the raycast hit anything in the non-masked layers
            if (hit.collider == collider)
            {
                InfoPopUpManager.OpenUI(fossileInfo);
            }
        }

    }

    /// <summary>
    /// Unlocks this Fossil in the museum.
    /// </summary>
    /// <param name="sprite">The Fossils <see cref="Sprite"/>.</param>
    /// <param name="fossileInfo">The Fossils <see cref="FossileInfo_SO"/>.</param>
    public void Unlock(FossileInfo_SO fossileInfo)
    {
        // gives object fossil data and its toggles unlocked state
        this.fossileInfo = fossileInfo;
        gameObject.GetComponent<SpriteRenderer>().sprite = fossileInfo.GetSprite;
        IsUnlocked = !IsUnlocked;
    }
}
