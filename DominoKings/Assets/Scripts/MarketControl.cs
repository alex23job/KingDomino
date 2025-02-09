using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class MarketControl : MonoBehaviour
{
    [SerializeField] private Text[] arTxtRes;
    [SerializeField] private Button btnExchange;
    [SerializeField] private Button btnInc, btnDec;
    [SerializeField] private Text txtCount;
    [SerializeField] private Toggle[] toggles;

    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ViewResourse(ResourseSet mrk, ResourseSet pl)
    {
        arTxtRes[0].text = mrk.CountMoney.ToString();
        arTxtRes[1].text = mrk.CountFood.ToString();
        arTxtRes[2].text = mrk.CountTree.ToString();
        arTxtRes[3].text = mrk.CountIron.ToString();
        arTxtRes[4].text = mrk.CountStone.ToString();
        arTxtRes[5].text = pl.CountMoney.ToString();
        arTxtRes[6].text = pl.CountFood.ToString();
        arTxtRes[7].text = pl.CountTree.ToString();
        arTxtRes[8].text = pl.CountIron.ToString();
        arTxtRes[9].text = pl.CountStone.ToString();
        btnDec.interactable = false;
        txtCount.text = count.ToString();
        foreach (Toggle tg in toggles) tg.isOn = false;
    }
}
