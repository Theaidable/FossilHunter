using Unity.VisualScripting;
using UnityEngine;

// Author - Malthe

public class CleanableFossil : MonoBehaviour
{
    private int layersCleaned = 0;
    [SerializeField] private int totalLayers;

    private FossileInfo_SO fossilData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(FossileInfo_SO fossilData)
    {
        this.fossilData = fossilData;
        GetComponent<SpriteRenderer>().sprite = this.fossilData.GetSprite;
        GetComponent<SpriteMask>().sprite = this.fossilData.GetSprite;

        gameObject.transform.GetChild(0).gameObject.GetComponent<EraseDirt>().UpdateIgnoredOppasity(); 
        gameObject.transform.GetChild(1).gameObject.GetComponent<EraseDirt>().UpdateIgnoredOppasity();
        gameObject.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite = this.fossilData.GetDirtySpite;
    }

    public async void LayerCleaned()
    {
        layersCleaned++;

        if (layersCleaned == totalLayers)
        {
            Debug.Log("All layers cleaned, (1 sec delay)");

            await Awaitable.WaitForSecondsAsync(1);

            CleaningManager.FossilCleaned(fossilData);
            GameObject.Destroy(gameObject);
        }
    }
}
