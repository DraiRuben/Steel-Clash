using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsInterfaceHandler : MonoBehaviour
{
    #region Variables
    [SerializeField] Image _playerCircleImage;

    [Header("Pourcentage part :")]
    [SerializeField] TextMeshProUGUI _playerNumberTextMeshPro;
    [SerializeField] TextMeshProUGUI _pourcentageTextMeshPro;
    [SerializeField] TextMeshProUGUI _pourcentageOutlineTextMeshPro;

    [Header("Life part :")]
    [SerializeField] List<Image> _fullLifeImageArray;
    #endregion

    #region Methods
    private void Awake()
    {
        SetCurrentPourcentageToZero();
        SetFullLife();
    }
    
    public void SetIDTo(int id, List<Color> colorList)
    {
        _playerCircleImage.color = colorList[id - 1];
        _playerNumberTextMeshPro.text = "P" + id;
    }

    #region Pourcentage
    public void SetCurrentPourcentageToZero()
    {
        _pourcentageTextMeshPro.text = "0 %";
        _pourcentageOutlineTextMeshPro.text = "0 %";
        _pourcentageTextMeshPro.color = Color.white;
    }

    public void SetCurrentPourcentageTo(int pourcentage)
    {
        // Showing the pourcentage
        _pourcentageTextMeshPro.text = pourcentage + " %";
        _pourcentageOutlineTextMeshPro.text = pourcentage + " %";

        // Adding color to the pourcentage
        if (pourcentage >= 300)
        {
            _pourcentageTextMeshPro.color = new Color(100f / 255f, 0, 0); // Really Dark red
        }
        else if (pourcentage >= 200)
        {
            _pourcentageTextMeshPro.color = Color.Lerp(Color.red, new Color(50f / 255f, 0, 0), (float)pourcentage / 100); // Red -> Dark Red
        }
        else if (pourcentage >= 50)
        {
            _pourcentageTextMeshPro.color = Color.Lerp(Color.yellow, Color.red, (float)pourcentage / 150); // Yellow -> Red
        }
        else if (pourcentage >= 0)
        {
            _pourcentageTextMeshPro.color = Color.Lerp(Color.white, Color.yellow, (float)pourcentage / 50); // White -> Yellow
        }
        else
        {
            _pourcentageTextMeshPro.color = new Color(0, 150f / 255f, 0, 1);
            _pourcentageTextMeshPro.text = "Cheater";
            _pourcentageOutlineTextMeshPro.text = "Cheater";
        }
    }
    #endregion

    #region Life
    public void SetFullLife()
    {
        for (int i = 0; i < _fullLifeImageArray.Count; i++)
        {
            _fullLifeImageArray[i].enabled = true;
        }
    }
    public void SetLifeTo(int number)
    {
        Debug.Assert(!(number > _fullLifeImageArray.Count), "You can't set the life value superior to the max number of life");
        Debug.Assert(!(number < 0), "You can't set the life value under 0");

        if (number == 0)
        {
            _pourcentageTextMeshPro.enabled = false;
            _pourcentageOutlineTextMeshPro.enabled = false;
        }
        for (int i = 0; i < _fullLifeImageArray.Count; i++)
        {
            if (i <= number - 1)
            {
                _fullLifeImageArray[i].enabled = true;
            }
            else
            {
                _fullLifeImageArray[i].enabled = false;
            }
        }
    }
    #endregion
    #endregion
}