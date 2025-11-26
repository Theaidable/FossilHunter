using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Fungerer sammen med CleaningToolSysten (Unity UI) til at gøre knapperne deri funktionelle
/// primært lavet af emma
/// </summary>
public class ToolBtnHandler : MonoBehaviour
{
    private Button brushBtn;
    private Button dremelBtn;
    private Button fineBrushBtn;
    private CleaningTools tools;
    private Background brushSprite;
    private Background dremelSprite;
    private Background fineBrushSprite;
    [SerializeField]
    private Color btnTintColor = new Color(0.5380028f, 0.5583875f, 0.735849f);
    void OnEnable()
    {
        tools = GameObject.Find("CleaningToolSystem").GetComponent<CleaningTools>();
        var doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;

        brushBtn = root.Q<Button>("BrushToolBtn");
        brushBtn.clicked += BrushBtnPressed;
        brushSprite = brushBtn.iconImage;

        dremelBtn = root.Q<Button>("DremelToolBtn");
        dremelBtn.clicked += DremelBtnPressed;
        dremelSprite = dremelBtn.iconImage;

        fineBrushBtn = root.Q<Button>("FineBrushToolBtn");
        fineBrushBtn.clicked += FineBrushBtnPressed;
        fineBrushSprite = fineBrushBtn.iconImage;
    }

    private void BrushBtnPressed()
    {
        tools.SwitchTool(CleaningTool.Brush);
        brushBtn.style.unityBackgroundImageTintColor = btnTintColor;
        dremelBtn.style.unityBackgroundImageTintColor = Color.white;
        fineBrushBtn.style.unityBackgroundImageTintColor = Color.white;
        brushBtn.iconImage = null;
        dremelBtn.iconImage = dremelSprite;
        fineBrushBtn.iconImage= fineBrushSprite;
    }
    private void DremelBtnPressed()
    {
        tools.SwitchTool(CleaningTool.Dremel);
        dremelBtn.style.unityBackgroundImageTintColor = btnTintColor;
        brushBtn.style.unityBackgroundImageTintColor = Color.white;
        fineBrushBtn.style.unityBackgroundImageTintColor = Color.white;
        dremelBtn.iconImage = null;
        brushBtn.iconImage = brushSprite;
        fineBrushBtn.iconImage = fineBrushSprite;
    }

    private void FineBrushBtnPressed()
    {
        tools.SwitchTool(CleaningTool.FineBrush);
        fineBrushBtn.style.unityBackgroundImageTintColor = btnTintColor;
        brushBtn.style.unityBackgroundImageTintColor = Color.white;
        dremelBtn.style.unityBackgroundImageTintColor = Color.white;
        fineBrushBtn.iconImage = null;
        brushBtn.iconImage = brushSprite;
        dremelBtn.iconImage = dremelSprite;
    }
}
