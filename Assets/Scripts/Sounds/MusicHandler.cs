using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    #region Variables
    public static MusicHandler Instance;

    [SerializeField] List<AudioClip> m_musicList;
    private bool m_isMusicStopped;

    private AudioSource m_audioSource;
    #endregion

    #region Methods
    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();

        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(PlayRandomizedMusicList());
    }

    /// <summary> Randomize the order of the musics </summary>
    void ShuffleMusicList()
    {
        // Generation of a random number
        System.Random _randomNumber = new();

        // Shuffling of the list
        List<AudioClip> _shuffledMusics = m_musicList.OrderBy(_audioClip => _randomNumber.Next()).ToList();
        m_musicList = _shuffledMusics;
    }

    /// <summary> Play a randomised list of music </summary>
    public IEnumerator PlayRandomizedMusicList()
    {
        while (true) 
        {
            ShuffleMusicList();

            for (int i = 0; i < m_musicList.Count; i++)
            {
                m_audioSource.clip = m_musicList[i];

                m_audioSource.Play();

                yield return new WaitForSecondsRealtime(m_musicList[i].length);
            }
        }
    }

    /// <summary> Stop the music to play, and stop the PlayRandomizedMusicList Coroutine </summary>
    public void StopMusic()
    {
        StopCoroutine(PlayRandomizedMusicList());
        m_audioSource.Stop();
    }
    #endregion
}