using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum CleaningTool { Brush, Dremel, FineBrush }
public class CleaningTools : MonoBehaviour
{
    [SerializeField]
    CleaningTool currentTool = CleaningTool.Brush;
    [Header("Brush settings")]
    [SerializeField]
    private int brushSize;
    [SerializeField]
    private LayerMask brushWorksOnLayers;
    [Header("Dremel settings")]
    [SerializeField]
    private int dremelSize;
    [SerializeField]
    private LayerMask dremelWorksOnLayers;
    [Header("Fine brush settings")]
    [SerializeField]
    private int fineBrushSize;
    [SerializeField]
    private LayerMask fineBrushWorksOnLayers;

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentTool = CleaningTool.Brush;
            Debug.Log($"switched to {currentTool}");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            currentTool = CleaningTool.Dremel;
            Debug.Log($"switched to {currentTool}");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            currentTool = CleaningTool.FineBrush;
            Debug.Log($"switched to {currentTool}");
        }
        //only try to draw & update sprite if the mouse is clicking
        if (Input.GetMouseButton(0))
        {
            switch (currentTool)
            {
                case CleaningTool.Brush:
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0, brushWorksOnLayers);
                    if (hit.collider != null)
                    {
                        Debug.Log($"hit collider {hit.collider.ToString()}");
                        if (hit.collider is Collider2D && hit.collider.gameObject.GetComponent<EraseDirt>() != null)
                        {
                            Debug.Log("brushing layer");
                            hit.collider.gameObject.GetComponent<EraseDirt>().EraserSize = brushSize;
                            hit.collider.gameObject.GetComponent<EraseDirt>().UpdateTexture(hit);
                            hit.collider.gameObject.GetComponent<EraseDirt>().Drawing = true;
                        }
                    }

                    break;
                case CleaningTool.Dremel:
                    RaycastHit2D hit2 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0, dremelWorksOnLayers);
                    if (hit2.collider != null && hit2.collider is Collider2D && hit2.collider.gameObject.GetComponent<EraseDirt>() != null)
                    {
                        hit2.collider.gameObject.GetComponent<EraseDirt>().EraserSize = dremelSize;
                        hit2.collider.gameObject.GetComponent<EraseDirt>().UpdateTexture(hit2);
                        hit2.collider.gameObject.GetComponent<EraseDirt>().Drawing = true;
                    }
                    break;
                case CleaningTool.FineBrush:
                    RaycastHit2D hit3 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0, fineBrushWorksOnLayers);

                    if (hit3.collider != null && hit3.collider is Collider2D && hit3.collider.gameObject.GetComponent<EraseDirt>() != null)
                    {
                        hit3.collider.gameObject.GetComponent<EraseDirt>().EraserSize = fineBrushSize;
                        hit3.collider.gameObject.GetComponent<EraseDirt>().UpdateTexture(hit3);
                        hit3.collider.gameObject.GetComponent<EraseDirt>().Drawing = true;
                    }
                    break;
            }


        }
    }

}
