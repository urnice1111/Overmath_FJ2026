// // This script plays a sound when the button "Jugar" is pressed
// using UnityEngine;

// public class ButtonSound : MonoBehaviour
// {
//     public AudioSource audioSource;
//     public AudioClip clickSound;

//     public void PlaySound()
//     {
//         Debug.Log("CLICK SONIDO");
//         audioSource.PlayOneShot(clickSound);
//     }
// }

using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioClip clickSound;

    public void PlaySound()
    {
        AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
    }
}