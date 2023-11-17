using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectedButtonArrow : MonoBehaviour
{
    public static SelectedButtonArrow Instance;
    private RectTransform m_transform;
    private EventSystem m_eventSystem;
    private GameObject m_oldSelected;
    private Image m_image;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        m_image = GetComponent<Image>();
        m_transform = GetComponent<RectTransform>();
        m_eventSystem = EventSystem.current;
    }
    private void Update()
    {
        GameObject newSeleted = m_eventSystem.currentSelectedGameObject;
        if (newSeleted == null)
        {
            m_image.enabled = false;
            m_oldSelected = null;
        }
        else if(newSeleted != null)
        {
            if(m_oldSelected != newSeleted)
            {
                m_image.enabled = true;
                m_oldSelected = newSeleted;
                
            }
        }
        if(m_oldSelected != null)
        {
            UpdateArrowPos();
        }
    }
    public void UpdateArrowPos()
    {
        RectTransform _selectedRectTransform = m_oldSelected.GetComponent<RectTransform>();
        m_transform.position = _selectedRectTransform.position;
        m_transform.anchoredPosition3D -= new Vector3(m_transform.sizeDelta.x/1.5f,m_transform.sizeDelta.y/2);
    }
}
