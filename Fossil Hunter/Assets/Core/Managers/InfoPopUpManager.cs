using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

public class InfoPopUpManager : MonoBehaviour
{
    private static UIDocument UIDocument;
    private static bool open;
    private static AudioSource Audio;
    [Header("For testing")]
    [SerializeField][Tooltip("For testing purposes.")] private FossileInfo_SO FossileInfo;

    public static bool Open { get => open; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIDocument = gameObject.GetComponent<UIDocument>();
        Audio = gameObject.GetComponent<AudioSource>();

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
        Audio.Play();
        Debug.Log("closed UI");
        UIDocument.enabled = false;
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
        open = true;

        //opsætter et close-event når man trykker på den røde knap.
        UIDocument.rootVisualElement.Q<Button>(name: "Btn_Close").clicked += CloseUI; ;

        //formatere fossilets data og viser det.
        UIDocument.rootVisualElement.Q<Label>(name: "Lbl_InfoText").text = fossileInfo.GetInfoText();

        //henter fossil sprite og asigner det til UI spriten.
        InfoBoxFossilPreview.ViewedSprite = fossileInfo.GetSprite;

    }
}
