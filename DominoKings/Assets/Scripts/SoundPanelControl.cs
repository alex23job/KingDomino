using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundPanelControl : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Toggle toggle;
    [SerializeField] private Image redCross;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnToggleChanged()
    {
        if (toggle.isOn)
        {
            redCross.gameObject.SetActive(false);
        }
        else
        {
            redCross.gameObject.SetActive(true);
        }
    }

    public void OnSliderChanged()
    {
        if (slider.value > 0)
        {
            toggle.isOn = true;
            redCross.gameObject.SetActive(false);
        }
    }
}
