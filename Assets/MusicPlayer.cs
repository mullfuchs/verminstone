using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] Songs;
    int currentSongIndex;
    AudioSource currentSong;

    // Start is called before the first frame update
    void Start()
    {
        currentSong = GetComponent<AudioSource>();
        if (Songs[0] != null)
        {
            currentSong.clip = Songs[0];
            currentSong.Play();
        }
    }

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!currentSong.isPlaying)
        {
            if(currentSongIndex + 1 >= Songs.Length)
            {
                currentSongIndex = 0;
            }
            else
            {
                currentSongIndex += 1;
            }
            currentSong.clip = Songs[currentSongIndex];
            currentSong.Play();
        }
    }
}
