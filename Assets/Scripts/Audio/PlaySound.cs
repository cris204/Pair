using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour
{

    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();    
    }

    public void PlayAudio(string path, float volume = 1)
    {
        audioSource.clip = ResourceManager.Instance.GetAudio(path);
        audioSource.volume = volume;
        audioSource.Play();
        StartCoroutine(WaitToReturnToPool());
    }

    private IEnumerator WaitToReturnToPool()
    {
        if (this.audioSource.clip != null) {
            yield return new WaitForSeconds(audioSource.clip.length);
        } else {
            yield return null;
        }
        PoolManager.Instance.ReleaseObject(Env.AUDIO_SOURCE, gameObject);
    }


}