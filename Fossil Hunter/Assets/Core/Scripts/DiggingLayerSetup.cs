using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class LayerSetup : MonoBehaviour
{
    [SerializeField]
    GameObject newFossilPrefab;
    //these will hopefully be shortened
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
        //get postion & size of the gameobject designating diggable area (said gameobject will be removed in first update)
        Vector2 digSpace = new Vector2(GameObject.Find("DiggingArea").GetComponent<BoxCollider2D>().size.x * GameObject.Find("DiggingArea").transform.localScale.x / 2, GameObject.Find("DiggingArea").GetComponent<BoxCollider2D>().size.y * GameObject.Find("DiggingArea").transform.localScale.y / 2);
        Vector2 digSpaceCenter = new Vector2(GameObject.Find("DiggingArea").transform.position.x, GameObject.Find("DiggingArea").transform.position.y);
        newFossilPrefab = Resources.Load("Prefabs/PickUppableFossil_Prefab") as GameObject;
        //put fields in some arrays so we can create a for loop
        Sprite[] layerSprites = { layer1Sprite, layer2Sprite, layer3Sprite, layer4Sprite, layer5Sprite, layer6Sprite, layer7Sprite, layer8Sprite, layer9Sprite, layer10Sprite };
        List<FossileInfo_SO>[] fossilsOnLayers = { fossilsOnLayer1, fossilsOnLayer2, fossilsOnLayer3, fossilsOnLayer4, fossilsOnLayer5, fossilsOnLayer6, fossilsOnLayer7, fossilsOnLayer8, fossilsOnLayer9, fossilsOnLayer10 };
        //create as many new earth layers as specified from total layers & fields
        for (int i = 0; i < totalLayers; i++)
        {
            GameObject newLayer = new GameObject();
            newLayer.name = $"Ground layer {i + 1}";
            SpriteRenderer sr = newLayer.AddComponent<SpriteRenderer>();
            sr.sortingLayerID = SortingLayer.layers[10 - i].id;
            sr.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            sr.sprite = layerSprites[i];
            if (layerSprites[i] == null) Debug.LogError($"ERROR: Layer {i} does not have a sprite!");
            newLayer.AddComponent<BoxCollider2D>();
            newLayer.transform.position = new Vector3(0, 0, i);
            if (i == (totalLayers - 1)) newLayer.tag = "Bottom Layer";

            //set up each fossil on this layer
            foreach (FossileInfo_SO fossil in fossilsOnLayers[i])
            {
                GameObject newFossil = Instantiate(newFossilPrefab, new Vector3(UnityEngine.Random.Range(-digSpace.x/2+digSpaceCenter.x, digSpace.x / 2 + digSpaceCenter.x), UnityEngine.Random.Range(-digSpace.y / 2 + digSpaceCenter.y, digSpace.y / 2 + digSpaceCenter.y), newLayer.transform.position.z - 0.5f), Quaternion.identity) as GameObject;
                newFossil.name = fossil.FossilType.ToString();
                newFossil.GetComponent<SpriteRenderer>().sprite = fossil.GetSprite;
                newFossil.GetComponent<PickupableFossil>().Data = fossil;
                newFossil.GetComponent<SpriteRenderer>().sortingLayerID = sr.sortingLayerID;
                newFossil.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
        }
    }
}
[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    // save the dictionary to lists
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // load dictionary from lists
    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
            throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

        for (int i = 0; i < keys.Count; i++)
            this.Add(keys[i], values[i]);
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
