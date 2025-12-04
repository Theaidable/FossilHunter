using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// Author - Malthe

public class CleanableFossil : MonoBehaviour
{
    private int layersCleaned = 0;
    [SerializeField] private int totalLayers;
    [SerializeField] private GameObject displayStarPrefab;
    [SerializeField] private float displayStarTime;

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
        gameObject.transform.GetChild(2).gameObject.GetComponent<EraseDirt>().UpdateTotalSaturation();
    }

    public async void LayerCleaned()
    {
        layersCleaned++;

        if (layersCleaned == totalLayers)
        {
            Debug.Log($"All layers cleaned, play star animation for {displayStarTime} sec");

            // makes star
            GameObject star = (GameObject)GameObject.Instantiate(displayStarPrefab, SceneManager.GetSceneByName("Cleaning level"));
            star.GetComponent<cleaningStarRotation>().SetSprite(fossilData.GetSprite);

            // removes cleaned object
            GameObject.Destroy(gameObject);

            // waits for a little bit
            await Awaitable.WaitForSecondsAsync(displayStarTime);

            // removes animation
            GameObject.Destroy(star);

            // Tell manager to cycle to next fossil
            CleaningManager.FossilCleaned(fossilData);
        }
    }
}
