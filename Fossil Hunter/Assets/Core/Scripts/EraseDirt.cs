using System;
using Unity.VisualScripting;
using UnityEngine;

public class EraseDirt : MonoBehaviour
{
    private float ignoredOppasity;
    private const float dirtMapOppasity = 641134.9f;

    private Texture2D m_Texture;
    private Color[] m_Colors;
    SpriteRenderer spriteRend;
    Color zeroAlpha = Color.clear;
    private int eraserSize;
    private Vector2Int lastPos;
    private bool drawing = false;
    private Rect originalSpriteRect;
    private float totalColourSaturation;
    [SerializeField]
    [Range(0f, 100f)]
    private float cleanPercentage = 90;
    public int EraserSize { get { return eraserSize; } set { eraserSize = value; } }
    public bool Drawing { get { return drawing; } set { drawing = value; } }

    void Awake()
    {


        spriteRend = gameObject.GetComponent<SpriteRenderer>();

        //set the rect of the sprite
        originalSpriteRect = new Rect(spriteRend.sprite.rect.x, spriteRend.sprite.rect.y, spriteRend.sprite.rect.width, spriteRend.sprite.rect.height);
        var tex = spriteRend.sprite.texture;
        //ready the texture that will be turning into a new sprite
        m_Texture = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
        m_Texture.filterMode = FilterMode.Bilinear;
        m_Texture.wrapMode = TextureWrapMode.Clamp;
        m_Colors = tex.GetPixels();
        m_Texture.SetPixels(m_Colors);
        m_Texture.Apply();
        //render sprite to test that it matches current settings
        spriteRend.sprite = Sprite.Create(m_Texture, originalSpriteRect, new Vector2(0.5f, 0.5f));
        //determine the total opacity of the dirt
        foreach (Color c in m_Colors)
        {
            totalColourSaturation += c.a;
        }
    }

    void Update()
    {
        if (!Input.GetMouseButton(0))
        {
            drawing = false;
        }
    }

    public void UpdateTexture(RaycastHit2D hit)
    {
        //make sure we only interact within the collider bounds & at the correct mouse position
        int w = m_Texture.width;
        int h = m_Texture.height;
        var mousePos = hit.point - (Vector2)hit.collider.bounds.min;
        mousePos.x *= w / (hit.collider.bounds.size.x);
        mousePos.y *= h / (hit.collider.bounds.size.y);

        //log current mouse movement
        Vector2Int p = new Vector2Int((int)mousePos.x, (int)mousePos.y);
        Vector2Int start = new Vector2Int();
        Vector2Int end = new Vector2Int();
        if (!drawing)
            lastPos = p;
        start.x = Mathf.Clamp(Mathf.Min(p.x, lastPos.x) - eraserSize, 0, w);
        start.y = Mathf.Clamp(Mathf.Min(p.y, lastPos.y) - eraserSize, 0, h);
        end.x = Mathf.Clamp(Mathf.Max(p.x, lastPos.x) + eraserSize, 0, w);
        end.y = Mathf.Clamp(Mathf.Max(p.y, lastPos.y) + eraserSize, 0, h);
        Vector2 dir = p - lastPos;
        for (int x = start.x; x < end.x; x++)
        {
            for (int y = start.y; y < end.y; y++)
            {
                //erase pixels within eraser range of mouse movement
                Vector2 pixel = new Vector2(x, y);
                Vector2 linePos = p;
                if (drawing)
                {
                    float d = Vector2.Dot(pixel - lastPos, dir) / dir.sqrMagnitude;
                    d = Mathf.Clamp01(d);
                    linePos = Vector2.Lerp(lastPos, p, d);
                }
                if ((pixel - linePos).sqrMagnitude <= eraserSize * eraserSize)
                {
                    m_Colors[x + y * w] = zeroAlpha;
                }
            }
        }
        //apply changes
        lastPos = p;
        m_Texture.SetPixels(m_Colors);
        m_Texture.Apply();
        spriteRend.sprite = Sprite.Create(m_Texture, originalSpriteRect, new Vector2(0.5f, 0.5f));
        CheckIfClean();
    }

    void CheckIfClean()
    {
        //compare current opacity to starting opacity & check if it meets the clean percentage
        float currentColourSaturation = 0;
        foreach (var color in m_Colors)
        {
            currentColourSaturation += color.a;
        }
        if ((currentColourSaturation - ignoredOppasity) <= totalColourSaturation - (totalColourSaturation / 100 * cleanPercentage))
        {
            //play feedback & remove dirt gameobject
            Debug.Log("clean!");
            transform.parent.gameObject.GetComponent<AudioSource>().Play();
            gameObject.transform.parent.gameObject.GetComponent<CleanableFossil>().LayerCleaned();
            GameObject.Destroy(gameObject);
        }
    }

    public void UpdateIgnoredOppasity()
    {
        Texture2D fossilTexture = gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>().sprite.texture;
        float fossilOppasity = 0;
        foreach (var color in fossilTexture.GetPixels())
        {
            fossilOppasity += color.a;
        }

        ignoredOppasity = dirtMapOppasity - fossilOppasity;
        Debug.Log($"{ignoredOppasity} | {totalColourSaturation}");
        totalColourSaturation -= ignoredOppasity;

/*        //gets smallest height and width
        int h = Math.Min(fossilTexture.height, dirtTexture.height);
        int w = Math.Min(fossilTexture.width, dirtTexture.width);

        //makes new texture
        Texture2D textureTemp = new Texture2D(dirtTexture.width, dirtTexture.height, TextureFormat.ARGB32, false);
        textureTemp.filterMode = FilterMode.Bilinear;
        textureTemp.wrapMode = TextureWrapMode.Clamp;*/


    }
}
