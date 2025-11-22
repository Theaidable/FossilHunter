using UnityEngine;
using UnityEngine.UIElements;

public class ToolBtnHandler : MonoBehaviour
{
    private Button brushBtn;
    private Button dremelBtn;
    private Button fineBrushBtn;
    private CleaningTools tools;
    void OnEnable()
    {
        tools=GameObject.Find("CleaningToolSystem").GetComponent<CleaningTools>();
        var doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;

        brushBtn = root.Q<Button>("BrushToolBtn");
        brushBtn.clicked += BrushBtnPressed;

        dremelBtn = root.Q<Button>("DremelToolBtn");
        dremelBtn.clicked += DremelBtnPressed;

        fineBrushBtn = root.Q<Button>("FineBrushToolBtn");
        fineBrushBtn.clicked += FineBrushBtnPressed;
    }

    private void BrushBtnPressed()
    {
        tools.SwitchTool(CleaningTool.Brush);
    }
    private void DremelBtnPressed()
    {
        tools.SwitchTool(CleaningTool.Dremel);
    }

    private void FineBrushBtnPressed()
    {
        tools.SwitchTool(CleaningTool.FineBrush);
    }
}
