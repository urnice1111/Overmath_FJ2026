using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instancia;

    private AudioSource audioSource;

    void Awake()
    {
        instancia = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}