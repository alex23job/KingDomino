using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundPanelControl : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Toggle toggle;
    [SerializeField] private Image redCross;

    [SerializeField] private AudioSource fone;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnToggleChanged()
    {
        if (toggle.isOn)
        {
            redCross.gameObject.SetActive(false);
            fone.Play();
        }
        else
        {
            redCross.gameObject.SetActive(true);
            fone.Pause();
        }
    }

    public void OnSliderChanged()
    {
        if (slider.value > 0)
        {
            toggle.isOn = true;
            redCross.gameObject.SetActive(false);
        }
        fone.volume = slider.value;
    }

    public void UpdateSound()
    {
        toggle.isOn = GameManager.Instance.currentPlayer.isSoundFone;
        slider.value = (float)GameManager.Instance.currentPlayer.volumeFone / 100.0f;
    }

    public void OnClickExit()
    {
        GameManager.Instance.currentPlayer.isSoundFone = toggle.isOn;
        GameManager.Instance.currentPlayer.volumeFone = (int)(slider.value * 100);
        GameManager.Instance.SaveGame();
    }
}
