using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsInterfaceHandler : MonoBehaviour
{
    #region Variables
    [SerializeField] private Image m_playerCircleImage;

    [Header("Pourcentage part :")]
    [SerializeField] private TextMeshProUGUI m_playerNumberTextMeshPro;
    [SerializeField] private TextMeshProUGUI m_pourcentageTextMeshPro;
    [SerializeField] private TextMeshProUGUI m_pourcentageOutlineTextMeshPro;

    [Header("Life part :")]
    [SerializeField] private List<Image> m_fullLifeImageArray;
    #endregion

    #region Methods
    private void Awake()
    {
        SetCurrentPourcentageToZero();
        SetFullLife();
    }
    public void SetIDTo(int _id, List<Color> _colorList)
    {
        m_playerCircleImage.color = _colorList[_id - 1];
        m_playerNumberTextMeshPro.text = "P" + _id;
    }

    #region Pourcentage
    public void SetCurrentPourcentageToZero()
    {
        m_pourcentageTextMeshPro.text = "0 %";
        m_pourcentageOutlineTextMeshPro.text = "0 %";
        m_pourcentageTextMeshPro.color = Color.white;
    }

    public void SetCurrentPourcentageTo(int _pourcentage)
    {
        // Showing the pourcentage
        m_pourcentageTextMeshPro.text = _pourcentage + " %";
        m_pourcentageOutlineTextMeshPro.text = _pourcentage + " %";

        // Adding color to the pourcentage
        if (_pourcentage >= 300)
        {
            m_pourcentageTextMeshPro.color = new Color(100f / 255f, 0, 0); // Really Dark red
        }
        else if (_pourcentage >= 200)
        {
            m_pourcentageTextMeshPro.color = Color.Lerp(Color.red, new Color(50f / 255f, 0, 0), (float)_pourcentage / 100); // Red -> Dark Red
        }
        else if (_pourcentage >= 50)
        {
            m_pourcentageTextMeshPro.color = Color.Lerp(Color.yellow, Color.red, (float)_pourcentage / 150); // Yellow -> Red
        }
        else if (_pourcentage >= 0)
        {
            m_pourcentageTextMeshPro.color = Color.Lerp(Color.white, Color.yellow, (float)_pourcentage / 50); // White -> Yellow
        }
        else
        {
            m_pourcentageTextMeshPro.color = new Color(0, 150f / 255f, 0, 1);
            m_pourcentageTextMeshPro.text = "Cheater";
            m_pourcentageOutlineTextMeshPro.text = "Cheater";
        }
    }
    #endregion

    #region Life
    public void SetFullLife()
    {
        for (int i = 0; i < m_fullLifeImageArray.Count; i++)
        {
            m_fullLifeImageArray[i].enabled = true;
        }
    }
    public void SetLifeTo(int _number)
    {
        Debug.Assert(!(_number > m_fullLifeImageArray.Count), "You can't set the life value superior to the max number of life");
        Debug.Assert(!(_number < 0), "You can't set the life value under 0");

        if (_number == 0)
        {
            m_pourcentageTextMeshPro.enabled = false;
            m_pourcentageOutlineTextMeshPro.enabled = false;
        }
        for (int i = 0; i < m_fullLifeImageArray.Count; i++)
        {
            if (m_fullLifeImageArray[i] != null)
            {
                if (i <= _number - 1)
                {
                    m_fullLifeImageArray[i].enabled = true;
                }
                else
                {
                    m_fullLifeImageArray[i].enabled = false;
                }
            }

        }
    }
    #endregion
    #endregion
}