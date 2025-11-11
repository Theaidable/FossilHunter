using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "FossileInfo_SO", menuName = "Scriptable Objects/FossileInfo_SO")]
public class FossileInfo_SO : ScriptableObject
{
    private string infoText;

    [SerializeField] public int Age;
    [SerializeField] public FossilType FossilType;
    [SerializeField] public Kvalitet Kvalitet;
    [SerializeField]
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
                    case FossilType.Amonite:
                        return "amoniter er seje";
                    case FossilType.Søpindsvin:
                        return "der her er et søpindsvin";
                    case FossilType.Vettelys:
                        return "tf even is a vettelys";
                    case FossilType.HajTand:
                        return "tooth of shork";
                    default:
                        return "couldn't find a type, if you see this; somebody fucked up.";
                }
                
            }
            else
            {
                return infoText;
            }
        }

        set
        {
            infoText = value;
        }
    }
}
