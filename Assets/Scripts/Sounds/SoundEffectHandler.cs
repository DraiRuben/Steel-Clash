using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SoundEffectHandler : MonoBehaviour
{
    #region Variables
    static public SoundEffectHandler Instance { get; private set; }

    [SerializeField] GameObject _audioSourcePrefab;

    [Header("List of Audio Sources :")]
    [SerializeField] AudioClip _hit;
    [SerializeField] AudioClip _counterHit;
    [SerializeField] AudioClip _death;

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
                StartCoroutine(CreatePrefabPlaySoundAndDestroy(_hit));
                break;
            
            case SoundEffectEnum.counterHit:
                StartCoroutine(CreatePrefabPlaySoundAndDestroy(_counterHit));
                break;

            case SoundEffectEnum.death:
                StartCoroutine(CreatePrefabPlaySoundAndDestroy(_death));
                break;
        }
    }

    // To optimise this code we can use the Polling method 

    /// <summary> Create a new GameObject, assigns a audio clip to it and play it, after that the GameObject is Destroy</summary>
    IEnumerator CreatePrefabPlaySoundAndDestroy(AudioClip audioClip)
    {
        AudioSource audioSource = Instantiate(_audioSourcePrefab, gameObject.transform).GetComponent<AudioSource>();

        audioSource.clip = audioClip;
        audioSource.Play();

        yield return new WaitForSecondsRealtime(audioClip.length);

        Destroy(audioSource.GameObject());
    }
    #endregion
}