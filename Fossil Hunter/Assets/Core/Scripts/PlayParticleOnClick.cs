using UnityEngine;

public class PlayParticleOnClick : MonoBehaviour
{
    public ParticleSystem particleEffect;

    private void OnMouseDown()
    {
        if (particleEffect != null)
        {
            particleEffect.Play();
        }
    }
}
