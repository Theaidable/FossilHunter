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
    private float brushSize;
    [SerializeField]
    private LayerMask worksOnLayers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) currentTool = CleaningTool.Brush; Debug.Log($"switched to{currentTool}");

        if (Input.GetKeyDown(KeyCode.S)) currentTool = CleaningTool.Dremel; Debug.Log($"switched to{currentTool}");

        if (Input.GetKeyDown(KeyCode.D)) currentTool = CleaningTool.FineBrush; Debug.Log($"switched to{currentTool}");

    }

}
