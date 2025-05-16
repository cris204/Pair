﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public GameObject GetGameObject(string path)
    {
        return Resources.Load<GameObject>(path);
    }
    public AudioClip GetAudio(string path)
    {
        return Resources.Load<AudioClip>(path);
    }
}