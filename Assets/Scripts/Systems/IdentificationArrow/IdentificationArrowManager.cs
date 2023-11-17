using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentificationArrowManager : MonoBehaviour
{
    #region Variables
    [SerializeField] GameObject m_identificationArrow;

    public static IdentificationArrowManager Instance;
    #endregion

    #region Methods

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void InstantiateIdentificationArrow(GameObject _characterGameObject, List<Color> _colorList)
    {
        GameObject _identificationArrow = Instantiate(
            m_identificationArrow,
            new Vector2(_characterGameObject.transform.position.x, _characterGameObject.transform.position.y + 1.5f),
            Quaternion.identity,
            _characterGameObject.transform
        );

        _identificationArrow.GetComponent<SpriteRenderer>().color = _colorList[PlayerManager.Instance.Players.Count - 1];
    }
    #endregion
}