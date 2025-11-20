using UnityEngine;

[CreateAssetMenu(fileName = "InfoBoxFossilPreview", menuName = "Scriptable Objects/InfoBoxFossilPreview")]
public class InfoBoxFossilPreview : ScriptableObject
{
    [SerializeField] private Sprite GetViewedSprite
    {
        get => ViewedSprite;
    }

    [SerializeField] public static Sprite ViewedSprite;

    private static InfoBoxFossilPreview Instance;

    private void OnValidate()
    {
        Instance = this;
    }
}
