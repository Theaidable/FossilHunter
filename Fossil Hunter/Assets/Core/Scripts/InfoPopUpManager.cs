using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.UIElements;

public class InfoPopUpManager : MonoBehaviour
{
    private Button closeButton;
    private Label infoText;
    [SerializeField] private FossileInfo_SO FossileInfo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        infoText = gameObject.GetComponent<UIDocument>().rootVisualElement.Q<Label>(name: "Lbl_InfoText");
        closeButton = gameObject.GetComponent<UIDocument>().rootVisualElement.Q<Button>(name: "Btn_Close");
        closeButton.clicked += CloseUI;
        //closeButton.clicked += ()=>{ OpenUI(FossileInfo); };
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// Closes the info pop-up
    /// </summary>
    public void CloseUI()
    {
        Debug.Log("closed UI");
        gameObject.SetActive(false); 
    }

    /// <summary>
    /// Is cald when you need to open the info-UI
    /// </summary>
    /// <param name="fossileInfo">The SO containing the information about the fossil</param>
    public void OpenUI(FossileInfo_SO fossileInfo)
    {
        Debug.Log("opened UI");

        //formatere infoet til infoboxen
        string finalText = "";
        finalText += $"{fossileInfo.FossilType}\t\tKvalitet: {fossileInfo.Kvalitet}\n\n";
        finalText += $"Alder: {fossileInfo.Age} mio. år\n";
        finalText += $"{fossileInfo.InfoText}";

        infoText.text = finalText;
        gameObject.SetActive(true);

    }
}
