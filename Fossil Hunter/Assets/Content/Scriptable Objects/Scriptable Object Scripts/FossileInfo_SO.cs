using UnityEngine;

[CreateAssetMenu(fileName = "FossileInfo_SO", menuName = "Scriptable Objects/FossileInfo_SO")]
public class FossileInfo_SO : ScriptableObject
{
    [SerializeField] public int Age;
    [SerializeField] public FossilType FossilType;
    [SerializeField] public Kvalitet Kvalitet;
    [SerializeField] public string InfoText;
}
