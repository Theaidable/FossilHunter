using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using static UnityEngine.ParticleSystem;

public enum CleaningTool { None, Brush, Dremel, FineBrush }
/// <summary>
/// Sets up and keeps track of the various tools in the cleaning scene, and keeps track of which tool is in use
/// primarily made by emma
/// </summary>
public class CleaningTools : MonoBehaviour
{
    [SerializeField]
    float tempParticleAdjustment;
    [SerializeField]
    CleaningTool currentTool = CleaningTool.None;
    [Space]
    [Header("Brush settings")]
    [SerializeField]
    private int brushSize;
    [SerializeField]
    private LayerMask brushWorksOnLayers;
    [SerializeField]
    private AudioResource brushUseSound;
    [SerializeField]
    private Texture2D brushSprite;
    [Header("Dremel settings")]
    [SerializeField]
    private int dremelSize;
    [SerializeField]
    private LayerMask dremelWorksOnLayers;
    [SerializeField]
    private AudioResource dremelUseSound;
    [SerializeField]
    private Texture2D dremelSprite;
    [Header("Fine brush settings")]
    [SerializeField]
    private int fineBrushSize;
    [SerializeField]
    private LayerMask fineBrushWorksOnLayers;
    [SerializeField]
    private AudioResource fineBrushUseSound;
    [SerializeField]
    private Texture2D fineBrushSprite;

    private HashSet<Collider2D> cleaningColliders = new HashSet<Collider2D>();
    private ParticleSystem particles;
    ParticleSystem.EmitParams emitParams;

    void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        emitParams = new ParticleSystem.EmitParams();
        emitParams.applyShapeToPosition = true;
        emitParams.startSize = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        //only try to draw & update sprite if the mouse is clicking
        if (Input.GetMouseButton(0))
        {
            emitParams.position = Camera.main.ScreenToWorldPoint(Input.mousePosition)*tempParticleAdjustment;
            switch (currentTool)
            {
                case CleaningTool.Brush:
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0, brushWorksOnLayers);
                    if (hit.collider != null && hit.collider is Collider2D && hit.collider.gameObject.GetComponent<EraseDirt>() != null)
                    {
                        hit.collider.gameObject.GetComponent<EraseDirt>().EraserSize = brushSize;
                        hit.collider.gameObject.GetComponent<EraseDirt>().UpdateTexture(hit);
                        hit.collider.gameObject.GetComponent<EraseDirt>().Drawing = true;
                        if (!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Play();
                        cleaningColliders.Add(hit.collider);
                        particles.Emit(emitParams, 1);
                    }

                    break;
                case CleaningTool.Dremel:
                    RaycastHit2D hit2 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0, dremelWorksOnLayers);
                    if (hit2.collider != null && hit2.collider is Collider2D && hit2.collider.gameObject.GetComponent<EraseDirt>() != null)
                    {
                        hit2.collider.gameObject.GetComponent<EraseDirt>().EraserSize = dremelSize;
                        hit2.collider.gameObject.GetComponent<EraseDirt>().UpdateTexture(hit2);
                        hit2.collider.gameObject.GetComponent<EraseDirt>().Drawing = true;
                        if (!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Play();
                        cleaningColliders.Add(hit2.collider);
                    }
                    break;
                case CleaningTool.FineBrush:
                    RaycastHit2D hit3 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0, fineBrushWorksOnLayers);

                    if (hit3.collider != null && hit3.collider is Collider2D && hit3.collider.gameObject.GetComponent<EraseDirt>() != null)
                    {
                        hit3.collider.gameObject.GetComponent<EraseDirt>().EraserSize = fineBrushSize;
                        hit3.collider.gameObject.GetComponent<EraseDirt>().UpdateTexture(hit3);
                        hit3.collider.gameObject.GetComponent<EraseDirt>().Drawing = true;
                        if (!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Play();
                        cleaningColliders.Add(hit3.collider);
                        particles.Emit(emitParams, 1);
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Stop();
            //upkeep with colliders
            List<Collider2D> removeColliders = null;
            foreach (Collider2D collider in cleaningColliders)
            {
                if (collider != null) collider.gameObject.GetComponent<EraseDirt>().Drawing = false;
                else
                {
                    if (removeColliders == null) removeColliders = new List<Collider2D>();
                    removeColliders.Add(collider);
                }
            }
            if (removeColliders != null)
            { foreach (Collider2D collider in removeColliders) cleaningColliders.Remove(collider); }
        }
    }
    public void SwitchTool(CleaningTool tool)
    {
        if (currentTool != tool)
        {
            switch (tool)
            {
                case CleaningTool.Brush:
                    currentTool = CleaningTool.Brush;
                    if (brushSprite != null) Cursor.SetCursor(brushSprite, Vector2.zero, CursorMode.Auto);
                    if (brushUseSound != null) GetComponent<AudioSource>().resource = brushUseSound;
                    Debug.Log($"switched to {currentTool}");
                    break;
                case CleaningTool.Dremel:
                    currentTool = CleaningTool.Dremel;
                    if (dremelSprite != null) Cursor.SetCursor(dremelSprite, Vector2.zero, CursorMode.Auto);
                    if (dremelUseSound != null) GetComponent<AudioSource>().resource = dremelUseSound;
                    Debug.Log($"switched to {currentTool}");
                    break;
                case CleaningTool.FineBrush:
                    currentTool = CleaningTool.FineBrush;
                    if (fineBrushSprite != null) Cursor.SetCursor(fineBrushSprite, Vector2.zero, CursorMode.Auto);
                    if (fineBrushUseSound != null) GetComponent<AudioSource>().resource = fineBrushUseSound;
                    Debug.Log($"switched to {currentTool}");
                    break;
                default:
                    break;
            }
        }
    }
}


