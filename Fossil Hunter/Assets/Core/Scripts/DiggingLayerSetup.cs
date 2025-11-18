using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LayerSetup : MonoBehaviour
{
    [SerializeField]
    [Range(1, 10)]
    private int totalLayers;
    [Header("Layer 1")]
    [SerializeField] private Sprite layer1Sprite;
    [SerializeField] private List<FossileInfo_SO> fossilsOnLayer1;
    [Header("Layer 2")]
    [SerializeField] private Sprite layer2Sprite;
    [SerializeField] private List<FossileInfo_SO> fossilsOnLayer2;
    [Header("Layer 3")]
    [SerializeField] private Sprite layer3Sprite;
    [SerializeField] private List<FossileInfo_SO> fossilsOnLayer3;
    [Header("Layer 4")]
    [SerializeField] private Sprite layer4Sprite;
    [SerializeField] private List<FossileInfo_SO> fossilsOnLayer4;
    [Header("Layer 5")]
    [SerializeField] private Sprite layer5Sprite;
    [SerializeField] private List<FossileInfo_SO> fossilsOnLayer5;
    [Header("Layer 6")]
    [SerializeField] private Sprite layer6Sprite;
    [SerializeField] private List<FossileInfo_SO> fossilsOnLayer6;
    [Header("Layer 7")]
    [SerializeField] private Sprite layer7Sprite;
    [SerializeField] private List<FossileInfo_SO> fossilsOnLayer7;
    [Header("Layer 8")]
    [SerializeField] private Sprite layer8Sprite;
    [SerializeField] private List<FossileInfo_SO> fossilsOnLayer8;
    [Header("Layer 9")]
    [SerializeField] private Sprite layer9Sprite;
    [SerializeField] private List<FossileInfo_SO> fossilsOnLayer9;
    [Header("Layer 10")]
    [SerializeField] private Sprite layer10Sprite;
    [SerializeField] private List<FossileInfo_SO> fossilsOnLayer10;

    public int TotalLayers
    {
        get => totalLayers;
        set
        {
            if (value > 0 && value < 11)
            {
                totalLayers = value;
            }
        }
    }

    void Awake()
    {
        Sprite[] layerSprites = { layer1Sprite, layer2Sprite, layer3Sprite, layer4Sprite, layer5Sprite, layer6Sprite, layer7Sprite, layer8Sprite, layer9Sprite, layer10Sprite };
        List<FossileInfo_SO>[] fossilsOnLayers = { fossilsOnLayer1, fossilsOnLayer2, fossilsOnLayer3, fossilsOnLayer4, fossilsOnLayer5, fossilsOnLayer6, fossilsOnLayer7, fossilsOnLayer8, fossilsOnLayer9, fossilsOnLayer10 };
        for (int i = 0; i < totalLayers; i++)
        {
            GameObject newLayer = new GameObject();
            newLayer.name = $"Ground layer {(i + 1).ToString()}";
            SpriteRenderer sr = newLayer.AddComponent<SpriteRenderer>();
            sr.sprite = layerSprites[i];;
            sr.sortingLayerID = SortingLayer.NameToID(SortingLayer.layers[10-i].name);
            sr.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            newLayer.AddComponent<BoxCollider2D>();
            newLayer.transform.position = new Vector3(0, 0, i);
            if (i == (totalLayers - 1)) newLayer.tag = "Bottom Layer";
        }
    }
}
/* rewrite this later to make the editor more clean to look at by dynamically adding and removing layer fields
[CustomEditor(typeof(LayerSetup))]
public class LayerSetupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var ownScript = target as LayerSetup;

        if (ownScript.TotalLayers > 1)
        {
            ownScript.
        }
        
        ownScript.flag = GUILayout.Toggle(ownScript.flag, "Flag");

        if (ownScript.flag)
            ownScript.i = EditorGUILayout.IntSlider("I field:", ownScript.i, 1, 100);
        
    }
}*/
