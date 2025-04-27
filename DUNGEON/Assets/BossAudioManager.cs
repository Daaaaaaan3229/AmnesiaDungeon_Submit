using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAudioManager : MonoBehaviour
{
    [SerializeField]private List<AudioClip> audioClips = new List<AudioClip>();
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    public void PlayAudio(int audioIndex)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(audioClips[audioIndex]);
    }
}
