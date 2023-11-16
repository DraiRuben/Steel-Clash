using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundEffectHandler : MonoBehaviour
{
    #region Variables
    static public SoundEffectHandler Instance { get; private set; }

    [SerializeField] private GameObject m_audioSourcePrefab;

    [Header("List of Audio Sources :")]
    [SerializeField] private List<AudioClip> m_hit;
    [SerializeField] private List<AudioClip> m_counterHit;
    [SerializeField] private List<AudioClip> m_death;

    public enum SoundEffectEnum
    {
        hit,
        counterHit,
        death,
    }
    #endregion

    #region Methods
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    /// <summary> Play a sound effect </summary>
    public void PlaySoundEffect(SoundEffectEnum soundType)
    {
        switch (soundType)
        {
            case SoundEffectEnum.hit:
                StartCoroutine(CreatePrefabPlaySoundAndDestroy(m_hit[Random.Range(0, m_hit.Count)]));
                break;
            
            case SoundEffectEnum.counterHit:
                StartCoroutine(CreatePrefabPlaySoundAndDestroy(m_counterHit[Random.Range(0,m_counterHit.Count)]));
                break;

            case SoundEffectEnum.death:
                StartCoroutine(CreatePrefabPlaySoundAndDestroy(m_death[Random.Range(0, m_death.Count)]));
                break;
        }
    }

    // To optimise this code we can use the Polling method 

    /// <summary> Create a new GameObject, assigns a audio clip to it and play it, after that the GameObject is Destroy</summary>
    IEnumerator CreatePrefabPlaySoundAndDestroy(AudioClip audioClip)
    {
        AudioSource audioSource = Instantiate(m_audioSourcePrefab, gameObject.transform).GetComponent<AudioSource>();

        audioSource.clip = audioClip;
        audioSource.Play();

        yield return new WaitForSecondsRealtime(audioClip.length);

        Destroy(audioSource.GameObject());
    }
    #endregion
}