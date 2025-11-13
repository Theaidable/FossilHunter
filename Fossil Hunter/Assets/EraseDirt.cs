using UnityEngine;

public class EraseDirt : MonoBehaviour
{
    private Texture2D m_Texture;
    private Color[] m_Colors;
    RaycastHit2D hit;
    SpriteRenderer spriteRend;
    Color zeroAlpha = Color.clear;
    [SerializeField]
    private int eraserSize;
    private Vector2Int lastPos;
    private bool Drawing = false;
    private Rect originalSpriteRect;
    void Start()
    {
        spriteRend = gameObject.GetComponent<SpriteRenderer>();
        originalSpriteRect = new Rect(spriteRend.sprite.rect.x, spriteRend.sprite.rect.y, spriteRend.sprite.rect.width / 2.56f, spriteRend.sprite.rect.height / 2.56f);
        var tex = spriteRend.sprite.texture;
        m_Texture = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);

        m_Texture.filterMode = FilterMode.Bilinear;
        m_Texture.wrapMode = TextureWrapMode.Clamp;
        m_Colors = tex.GetPixels();
        m_Texture.SetPixels(m_Colors);
        m_Texture.Apply();
        spriteRend.sprite = Sprite.Create(m_Texture, originalSpriteRect, new Vector2(0.5f, 0.5f));
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider == GetComponent<Collider2D>())
            {
                UpdateTexture();
                Drawing = true;
            }
        }
        else
            Drawing = false;
    }

    public void UpdateTexture()
    {
        int w = m_Texture.width;
        int h = m_Texture.height;
        var mousePos = hit.point - (Vector2)hit.collider.bounds.min;
        mousePos.x *= w / (hit.collider.bounds.size.x * 2.56f);
        mousePos.y *= h / (hit.collider.bounds.size.y * 2.56f);
        Vector2Int p = new Vector2Int((int)mousePos.x, (int)mousePos.y);
        Vector2Int start = new Vector2Int();
        Vector2Int end = new Vector2Int();
        if (!Drawing)
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
                Vector2 pixel = new Vector2(x, y);
                Vector2 linePos = p;
                if (Drawing)
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
        lastPos = p;
        m_Texture.SetPixels(m_Colors);
        m_Texture.Apply();
        spriteRend.sprite = Sprite.Create(m_Texture, originalSpriteRect, new Vector2(0.5f, 0.5f));
    }
}
