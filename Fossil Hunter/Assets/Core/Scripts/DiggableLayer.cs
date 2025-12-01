using UnityEditor;
using UnityEngine;

/// <summary>
/// Bliver automatisk sat på hvert lag i DiggingScene og sørger for at laget kan udviskes og opdatere sin sprite
/// primært lavet af emma, med lidt kode fra nettet
/// </summary>
public class DiggableLayer : MonoBehaviour
{
    private Texture2D m_Texture;
    private Color[] m_Colors;
    SpriteRenderer spriteRend;
    Color zeroAlpha = Color.clear;
    private int eraserSize;
    private Vector2Int lastPos;
    private Rect originalSpriteRect;
    public int EraserSize { get { return eraserSize; } set { eraserSize = value; } }

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
    }

    void Update()
    {

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
        start.x = Mathf.Clamp(Mathf.Min(p.x) - eraserSize, 0, w);
        start.y = Mathf.Clamp(Mathf.Min(p.y) - eraserSize, 0, h);
        end.x = Mathf.Clamp(Mathf.Max(p.x) + eraserSize, 0, w);
        end.y = Mathf.Clamp(Mathf.Max(p.y) + eraserSize, 0, h);
        Vector2 dir = p - lastPos;
        for (int x = start.x; x < end.x; x++)
        {
            for (int y = start.y; y < end.y; y++)
            {
                //erase pixels within eraser range of mouse movement
                Vector2 pixel = new Vector2(x, y);
                Vector2 linePos = p;

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
    }
    public bool HasHoleAtPoint(RaycastHit2D hit, float allowedCover)
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
        start.x = Mathf.Clamp(Mathf.Min(p.x) - eraserSize, 0, w);
        start.y = Mathf.Clamp(Mathf.Min(p.y) - eraserSize, 0, h);
        end.x = Mathf.Clamp(Mathf.Max(p.x) + eraserSize, 0, w);
        end.y = Mathf.Clamp(Mathf.Max(p.y) + eraserSize, 0, h);
        Vector2 dir = p - lastPos;
        float totalColourInRadius = 0;
        float totalPossibleColourInRadius = 0;
        for (int x = start.x; x < end.x; x++)
        {
            for (int y = start.y; y < end.y; y++)
            {
                //erase pixels within eraser range of mouse movement
                Vector2 pixel = new Vector2(x, y);
                Vector2 linePos = p;

                if ((pixel - linePos).sqrMagnitude <= eraserSize * eraserSize)
                {
                    totalPossibleColourInRadius += 1;
                    totalColourInRadius += 1 - m_Colors[x + y * w].a;
                }
            }
        }
        //answer
        float result = 100 / totalPossibleColourInRadius * totalColourInRadius;
        if (result < allowedCover) { return false; }
        else { return true; }
    }
}
