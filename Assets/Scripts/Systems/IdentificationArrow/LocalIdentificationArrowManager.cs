using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalIdentificationArrowManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        GoToTheCharacterPosition(transform.parent.transform);
    }

    void GoToTheCharacterPosition(Transform _characterTransform)
    {
        transform.position = new Vector2(_characterTransform.position.x, _characterTransform.position.y + 0.75f);
    }
}
