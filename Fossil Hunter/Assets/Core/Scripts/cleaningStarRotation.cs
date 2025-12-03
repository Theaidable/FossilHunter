using UnityEngine;

// Author - Malthe

public class cleaningStarRotation : MonoBehaviour
{
    [SerializeField][Range(1, 2)] private float scaleIncreaseMultiplier;
    [SerializeField][Range(0, 1)] private float startingScale;
    [SerializeField][Range(0, 0.5f)] private float rotationSpeed;
    [SerializeField][Range(1, 10)] private float rotationSpeedMultiplierWhenScaling;
    [SerializeField][Range(0, 20)] private float FossilShakeRange;
    [SerializeField][Range(0, 1)] private float FossilShakeSpeed;
    private float currentRotation = 0;
    private Transform childTransform;
    private Sprite childSprite;

    public float CurrentRotation
    {
        get => currentRotation;
        set
        {
            currentRotation = value;
            if (currentRotation > Mathf.PI * 2)
            {
                //currentRotation -= Mathf.PI * 2;
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.transform.localScale = new Vector3(startingScale, startingScale, startingScale);
        childTransform = gameObject.transform.GetChild(0);
        childTransform.gameObject.GetComponent<SpriteRenderer>().sprite = childSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.localScale.x < 1)
        {
            gameObject.transform.localScale *= scaleIncreaseMultiplier;
        }
        else
        {
            gameObject.transform.localScale = Vector3.one;
            rotationSpeedMultiplierWhenScaling = 1;
        }


        CurrentRotation += rotationSpeed * rotationSpeedMultiplierWhenScaling;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, CurrentRotation);
        childTransform.rotation = Quaternion.Euler(0, 0, Mathf.Cos((CurrentRotation/rotationSpeedMultiplierWhenScaling) * FossilShakeSpeed) * FossilShakeRange);
    }

    public void SetSprite(Sprite sprite)
    {
        if (childTransform != null)
        {
            childTransform.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
            //Debug.Log($"sprite set: {childTransform.gameObject.GetComponent<SpriteRenderer>().sprite.name}");
        }
        else
        {
            childSprite = sprite;
        }
    }
}
