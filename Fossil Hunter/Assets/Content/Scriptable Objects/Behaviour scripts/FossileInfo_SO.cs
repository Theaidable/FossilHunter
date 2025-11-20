using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "FossileInfo_SO", menuName = "Scriptable Objects/FossileInfo_SO")]
public class FossileInfo_SO : ScriptableObject
{
    [SerializeField][Tooltip("The is the sprites for every type. There should only one SO with a list of sprites.")] private List<Sprite> FossilSprites = new List<Sprite>();

    [SerializeField][Tooltip("The fact text about the fossil.\nLeave empty for the fossil's deafult tekst.")] private string infoText;
    [SerializeField][Tooltip("The fossils sprite.\nLeave empty for the fossil's deafult sprite.")] private Sprite sprite;

    [SerializeField][Tooltip("The age of the fossil in mio. years.")] public int Age;
    [SerializeField][Tooltip("The type of fossil.")] public FossilType FossilType;
    [SerializeField][Tooltip("The quality of the fossil.")] public Kvalitet Kvalitet;

    private static FossileInfo_SO Instance;

    // når værdierne fra inspectoren er givet
    private void OnValidate()
    {
        // kun ét instance er blevet givet spritesne, hvis det er den her; bliver den det static 'Instance' som du andre referere til.
        if (FossilSprites.Count != 0)
        {
            Instance = this;
        }        
    }

    public Sprite GetSprite
    {
        get
        {
            // return custom value if one is set
            if (sprite == null)
            {
                // returns a string based on the type of fossil
                return Instance.FossilSprites[(int)FossilType];
            }
            else
            {
                return sprite;
            }

        }
    }

    public string InfoText
    {
        get
        {
            // return custom value if one is set
            if (infoText == "")
            {
                // returns a string based on the type of fossil
                switch (FossilType)
                {
                    case FossilType.Ammonit:
                        return "amoniter er seje";
                    case FossilType.Søpindsvin:
                        return "der her er et søpindsvin";
                    case FossilType.Vettelys:
                        return "tf even is a vettelys";
                    case FossilType.HajTand:
                        return "tooth of shork";
                    default:
                        return "?";
                }

            }
            else
            {
                return infoText;
            }
        }
    }

    public string GetInfoText()
    {
        // makes the values "?" if not unlock/ shouldn't be accessed.
        string fossilType = (int)FossilType == 0 ? "???\t" : $"{FossilType}";
        string kvalitet = (int)Kvalitet == 0 ? "???\t\t" : $"{Kvalitet}";
        string age = (Age == 0 ? "???\t" : $"{Age} mio. år");

        // formats the text
        string finalText = "";
        finalText += $"{fossilType}\t\t";
        finalText += $"Kvalitet: {kvalitet}\n\n";
        finalText += $"Alder: {age}\n";
        finalText += $"{InfoText}";

        return finalText;
    }



    public static FossileInfo_SO GetRandomizedData()
    {
        FossileInfo_SO DataSO = CreateInstance<FossileInfo_SO>();
        DataSO.Age = Random.Range(5, 500);
        DataSO.FossilType = (FossilType)Random.Range(1, 5);
        
        //giver fossilet en kvalitet
        float quality = Random.value;
        if (quality <= 0.5f) //50%
        {
            DataSO.Kvalitet = Kvalitet.Dårlig;
        }
        else if (quality <= 0.75f) //25%
        {
            DataSO.Kvalitet = Kvalitet.Middle;
        }
        else if (quality <= 0.9f) //15%
        {
            DataSO.Kvalitet = Kvalitet.God;
        }
        else //10%
        {
            DataSO.Kvalitet = Kvalitet.Perfekt;
        }
        
        return DataSO;
    }

}
