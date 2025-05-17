using System;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    
    public bool isMuted;
    [SerializeField] 
    private AudioMixer audioMixer;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        isMuted = PlayerPrefs.GetInt("SoundMuted", 0) == 1;
    
    }

    private void Start()
    {
        audioMixer.SetFloat("MasterVolume", isMuted ? -80f : 0f);
    }

    public void ToggleSound()
    {
        isMuted = !isMuted;
        
        float volume = isMuted ? -80f : 0f;

        audioMixer.SetFloat("MasterVolume", volume);
        
        //Used player prefs that is other way to save small things 
        PlayerPrefs.SetInt("SoundMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void PlaySound(string path){
        PoolManager.Instance.GetObject(Env.AUDIO_SOURCE).GetComponent<PlaySound>().PlayAudio(path, 1f);
    }
    
}