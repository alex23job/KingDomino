using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpPanelControl : MonoBehaviour
{
    [SerializeField] private GameObject[] panelsInfo;
    [SerializeField] private Text txtNumPage;

    private int numPage = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnClickLeft()
    {
        panelsInfo[numPage].SetActive(false);
        numPage += panelsInfo.Length - 1;
        numPage %= panelsInfo.Length;
        txtNumPage.text = $"{(numPage + 1):00}";
        if (numPage < 2)
        {

        }
        panelsInfo[numPage].SetActive(true);
    }

    public void OnClickRight()
    {
        panelsInfo[numPage].SetActive(false);
        numPage++;
        numPage %= panelsInfo.Length;
        txtNumPage.text = $"{(numPage + 1):00}";
        if (numPage < 2)
        {

        }
        panelsInfo[numPage].SetActive(true);
    }
}
