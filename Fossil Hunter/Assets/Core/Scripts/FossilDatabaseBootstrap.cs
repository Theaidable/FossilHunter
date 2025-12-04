using UnityEngine;

public class FossilDatabaseBootstrap : MonoBehaviour
{
    [SerializeField] private FossileInfo_SO fossilDatabase;

    private void Awake()
    {
        if (fossilDatabase == null)
        {
            Debug.LogError("FossilDatabaseBootstrap: fossilDatabase mangler i inspector!");
            return;
        }

        FossileInfo_SO.RegisterDatabase(fossilDatabase);
    }
}
