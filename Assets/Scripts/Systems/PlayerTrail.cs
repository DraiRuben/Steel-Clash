using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_afterImage;
    public void StartAfterimageTrail(float _duration ,float _amount)
    {
        StartCoroutine(AfterimageTrail(_duration, _amount));
    }
    private IEnumerator AfterimageTrail(float _duration, float _amount)
    {
        for(int i =0;i< _amount; i++)
        {
            var instantiated = Instantiate(m_afterImage.gameObject, transform.position, m_afterImage.transform.root.rotation);
            instantiated.GetComponent<Animator>().SetBool("Afterimage", true);
            Destroy(instantiated, 3f);
            yield return new WaitForSeconds(_duration/_amount);
        }
    }
}
