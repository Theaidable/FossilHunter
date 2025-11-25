using UnityEngine;

/// <summary>
/// Holder på en masse audioclips, og kan kaldes til at spille lyde igennem audiosource. Med dette script kan vi, i stedet for at lave syvtusinde audiosources, bare have det her script der holder
/// på alle audioclipsene. Spare lidt på energien, bare husk at kalde den her og ikke audiosourcen når du har brug for en sfx. Kan godt gå at bare bruge audiosource til objekter der alligevel ikke har
/// mange forskellige lyde, og måske kun har den ene.
/// Af Echo :3
/// </summary>
public class SFXManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip pickUp;
    public AudioClip dig;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Når man graver
    /// </summary>
    public void DigSound()
    {
        audioSource.pitch = Random.Range(0.8f, 1f);
        audioSource.PlayOneShot(dig, 0.5f);
    }

    /// <summary>
    /// Når man samler noget op
    /// </summary>
    public void PickUpSound()
    {
        audioSource.PlayOneShot(pickUp, 1);
    }
}
