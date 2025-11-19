using UnityEngine;
using UnityEngine.UIElements;

public class InfoPopUpManager : MonoBehaviour
{
    private static UIDocument UIDocument;
    private static SpriteRenderer renderer;
    private static bool open;
    [Header("For testing")]
    [SerializeField][Tooltip("For testing purposes.")] private FossileInfo_SO FossileInfo;

    public static bool Open { get => open; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIDocument = gameObject.GetComponent<UIDocument>();
        renderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        open = false;
        UIDocument.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// Closes the info pop-up.
    /// </summary>
    public static void CloseUI()
    {
        Debug.Log("closed UI");
        UIDocument.enabled = false;
        renderer.enabled = false;
        open = false;
    }

    /// <summary>
    /// Is cald when you need to open the info-UI.
    /// </summary>
    /// <param name="fossileInfo">The <see cref="FossileInfo_SO"/> containing the information about the fossil.</param>
    public static void OpenUI(FossileInfo_SO fossileInfo)
    {
        Debug.Log("opened UI");
        
        UIDocument.enabled = true;
        renderer.enabled = true;
        open = true;

        //opsætter et close-event når man trykker på den røde knap.
        UIDocument.rootVisualElement.Q<Button>(name: "Btn_Close").clicked += CloseUI; ;

        //formatere fossilets data og viser det.
        UIDocument.rootVisualElement.Q<Label>(name: "Lbl_InfoText").text = fossileInfo.GetInfoText();

        renderer.sprite = fossileInfo.GetSprite;
        
    }
}
