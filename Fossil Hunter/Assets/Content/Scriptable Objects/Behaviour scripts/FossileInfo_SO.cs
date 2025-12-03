
using UnityEngine;
using System.Collections.Generic;

//Author - Malthe

[CreateAssetMenu(fileName = "FossileInfo_SO", menuName = "Scriptable Objects/FossileInfo_SO")]
public class FossileInfo_SO : ScriptableObject
{
    [SerializeField][Tooltip("This is the sprites for uncleaned fossil. There should only one SO with a list of sprites.")] private List<Sprite> DirtySprites = new List<Sprite>();
    [SerializeField][Tooltip("This is the sprites for every type. There should only one SO with a list of sprites.")] private List<Sprite> FossilSprites = new List<Sprite>();

    [SerializeField][Tooltip("The fact text about the fossil.\nLeave empty for the fossil's deafult tekst.")] private string infoText;
    [SerializeField][Tooltip("The fossils sprite.\nLeave empty for the fossil's deafult sprite.")] private Sprite sprite;

    [SerializeField][Tooltip("The age of the fossil in mio. years.")] public int Age;
    [SerializeField][Tooltip("The type of fossil.")] public FossilType FossilType;
    [SerializeField][Tooltip("The quality of the fossil.\nChoose 'Unik' for a one of one fossil.")] public Kvalitet Kvalitet;

    private static FossileInfo_SO Instance;
    private Sprite dirtySpite;
    private bool found = false;

    private void OnEnable()
    {
        // kun ét instance er blevet givet spritesne, hvis det er den her; bliver den det static 'Instance' som du andre referere til.
        if (FossilSprites.Count != 0 || DirtySprites.Count != 0)
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Shows whether or not the fossil has been found.
    /// Is only used for unik fossils.
    /// </summary>
    public bool Found
    {
        get => found;
        set
        {
            found = value;
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

    public Sprite GetDirtySpite
    {
        get
        {
            if (dirtySpite == null)
            {
                int index = Random.Range(0, Instance.DirtySprites.Count);
                dirtySpite = Instance.DirtySprites[index];
            }
            return dirtySpite;
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
                    case FossilType.None:
                        return "?";
                    case FossilType.Ammonit:
                        return "Ammoniter er en gruppe af uddøde blæksprutter, der levede fra midten\naf Palæozoikum til kridttiden. Dyrene havde skaller, der kan findes\nsom forsteninger.";
                    case FossilType.Søpindsvin:
                        return "perioden fra den sene Kridttid til den tidlige del af\nPaleocæn - tidsrummet for 71-61 mio. år siden, hvor langt de\nfleste forstenede søpindsvin levede.";
                    case FossilType.Vettelys:
                        return "Vættelys er den fossile skal fra belemnitter (blæksprutter), der levede\ni kridttiden. Vættelys hører til de almindelige forsteninger.";
                    case FossilType.HajTand:
                        return "Små tænder på nogle få millimeter er mest almindelige. Som regel er det\ntændernes centrale spids, som er bevaret som fossil.\nTænder med rødder er derimod meget sjældne.";
                    case FossilType.Rav:
                        return "Rav antages at være dannet for ca. 57-35 mio. år siden i skove i\nØstersøområdet og herfra at være skyllet ud i havet i Oligocæn.";
                    default:
                        return "Fantastisk Fund";
                }

            }
            else
            {
                return infoText;
            }
        }
    }

    /* Denne metode er kun i editor
    // når værdierne fra inspectoren er givet
    private void OnValidate()
    {
        // kun ét instance er blevet givet spritesne, hvis det er den her; bliver den det static 'Instance' som du andre referere til.
        if (FossilSprites.Count != 0)
        {
            Instance = this;
        }
    }
    */

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

    /// <summary>
    /// Makes a randomized <see cref="FossileInfo_SO"/>.
    /// </summary>
    /// <param name="type">The type of fossil. Set to <see cref="FossilType.None"/> for a random type.</param>
    /// <returns>The randomized <see cref="FossileInfo_SO"/> instance.</returns>
    public static FossileInfo_SO GetRandomizedData(FossilType type = FossilType.None)
    {
        FossileInfo_SO DataSO = CreateInstance<FossileInfo_SO>();
        DataSO.infoText = "";

        // if type isn't set, set a random one.
        DataSO.FossilType = (type == FossilType.None) ? (FossilType)Random.Range(1, 6) : type;

        // give a random age.
        DataSO.Age = GetRandomizedAge(DataSO.FossilType);

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


    /// <summary>
    /// Makes a randomized age, that is fitting for the fossiltype.
    /// This randomization is based on bellcurves.
    /// </summary>
    /// <param name="type">The <see cref="FossilType"/> to make an age for.</param>
    /// <returns>A random age for the <see cref="FossilType"/>, in mio. years.</returns>
    private static int GetRandomizedAge(FossilType type)
    {
        switch (type)
        {
            case FossilType.Ammonit: //65.5 - 410 mio år (midt palæzoikum til kridt tiden)
                return Random.Range(33, 206) + Random.Range(33, 206);
            case FossilType.Søpindsvin: // 62 - 70 mio år (sen kridt tiden til tidlige danien)
                return Random.Range(31, 46) + Random.Range(31, 46);
            case FossilType.Vettelys: //65.5 - 145.5 mio år (kridt tiden)
                return Random.Range(33, 74) + Random.Range(33, 74);
            case FossilType.HajTand: //61 - 65 mio år (danien, findes i danien limestone/kalksten)
                return Random.Range(31, 34) + Random.Range(30, 33);
            case FossilType.Rav: //35 - 57 mio år (kom hertil omkring oligocene)
                return Random.Range(17, 28) + Random.Range(18, 29);
            default:
                return 0;
        }
    }

}
