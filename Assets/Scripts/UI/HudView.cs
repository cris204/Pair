using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudView : MonoBehaviour
{
    [SerializeField] 
    private Button restartButton;
    
    [SerializeField] 
    private Button toggleSoundButton;
    
    [SerializeField] 
    private TextMeshProUGUI toggleSoundButtonText;
    private void Start()
    {
        toggleSoundButtonText.text = SoundManager.Instance.isMuted ? "Unmute" : "Mute";
        
        restartButton.onClick.AddListener(() =>
        {
            GameManager.Instance.NewGame();
        });
        
        toggleSoundButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ToggleSound();
            
            toggleSoundButtonText.text = SoundManager.Instance.isMuted ? "Unmute" : "Mute";
        });
    }
}
