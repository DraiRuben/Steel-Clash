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

    public void PlaySoundEffect(SoundEffectEnum soundType)
    {
        switch (soundType)
        {
            case SoundEffectEnum.hit:
                CreatePrefabAndPlaySound(_hit);
                break;
            
            case SoundEffectEnum.counterHit:
                CreatePrefabAndPlaySound(_counterHit);
                break;

            case SoundEffectEnum.death:
                CreatePrefabAndPlaySound(_death);
                break;
        }
    }

    void CreatePrefabAndPlaySound(AudioClip audioClip)
    {
        Instantiate(_audioSourcePrefab, gameObject.transform).GetComponent<AudioSource>().clip = audioClip;
    }
    #endregion
}