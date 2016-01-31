using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class GameHandler : MonoBehaviour
{
    [Header("Audio clips")]
    public AudioClip Soundtrack;
    public AudioClip RitualMusic;

    // Audio settings.
    private AudioSource audioSource;
    private bool isRitualMusicPlaying;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Soundtrack;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayRitualMusic()
    {
        if (!isRitualMusicPlaying)
        {
            audioSource.clip = RitualMusic;
            audioSource.loop = true;
            audioSource.Play();
            isRitualMusicPlaying = true;
        }
    }

    public void PlaySoundtrackMusic()
    {
        if (isRitualMusicPlaying)
        {
            audioSource.clip = Soundtrack;
            audioSource.loop = true;
            audioSource.Play();
            isRitualMusicPlaying = false;
        }
    }

}
