using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public AudioSource bgmSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        bgmSource.Play();
        DontDestroyOnLoad(gameObject);
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void StartBGM()
    {
        bgmSource.Play();
    }
}
