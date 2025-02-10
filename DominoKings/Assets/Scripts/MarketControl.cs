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

    [SerializeField] private ResPanControl playerResPanControl;

    private ResourseSet marketSet = null, playerSet = null;
    private int count = 0;
    private int currentIndTgMrk = -1, currentIndTgPl = -1;
    private int mode = -1;
    // Start is called before the first frame update
    void Start()
    {
        ButtonExchangeStop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ViewResourse(ResourseSet mrk, ResourseSet pl)
    {
        marketSet = mrk;
        playerSet = pl;
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
        btnInc.interactable = false;
        txtCount.text = count.ToString();
        foreach (Toggle tg in toggles) tg.isOn = false;
    }

    public void OnClickInc()
    {
        int countMrk = GetCount(currentIndTgMrk);
        int countPl = GetCount(currentIndTgPl);

        if (mode == 1)  // купить
        {
            if (countMrk > count && count < countPl)
            {
                count++;
                txtCount.text = $"+{count}";
            }
            else
            {   //  что не так -> будем информировать об этом?

            }
        }
        if (mode == 2)  // Продать
        {
            if (countMrk > count && count < countPl)
            {
                count++;
                txtCount.text = $"-{count}";
            }
            else
            {   //  что не так -> будем информировать об этом?

            }
        }
        if (mode == 3)  // менять
        {
            if (countMrk > count && count < countPl)
            {
                count++;
                txtCount.text = $"{count}";
            }
            else
            {   //  что не так -> будем информировать об этом?

            }
        }
        if (count > 0) btnDec.interactable = true;
    }

    public void OnClickDec()
    {
        if (count > 0)
        {
            count--;
            switch(mode)
            {
                case 1: txtCount.text = $"+{count}"; break;
                case 2: txtCount.text = $"-{count}"; break;
                case 3: txtCount.text = $"{count}"; break;
            }
        }
        if (count <= 0) btnDec.interactable = false;
    }

    public void OnClickExchange()
    {        
        int[] mrkAr = new int[5], plAr = new int[5];
        int[] mrkTmp = marketSet.GetArResourses(), plTmp = playerSet.GetArResourses();
        for(int i = 0; i < 5; i++)
        {
            mrkAr[i] = mrkTmp[i];
            plAr[i] = plTmp[i];
        }
        if ((currentIndTgMrk >= 0 && currentIndTgMrk < 5) && (currentIndTgPl >= 5 && currentIndTgPl < 10))
        {
            print($"OnClickExchange  mode={mode}  count={count}  curIndTgMrk={currentIndTgMrk}  curIndTgPl={currentIndTgPl}");
            if (mode == 1)
            {
                mrkAr[0] += count;
                mrkAr[currentIndTgMrk % 5] -= count;
                plAr[0] -= count;
                plAr[currentIndTgMrk % 5] += count;
                arTxtRes[0].text = mrkAr[0].ToString();
                arTxtRes[currentIndTgMrk % 5].text = mrkAr[currentIndTgMrk % 5].ToString();
                arTxtRes[5].text = plAr[0].ToString();
                arTxtRes[currentIndTgMrk + 5].text = plAr[currentIndTgMrk % 5].ToString();
                print($"OnClickExchange  купить  mkrAr<<{mrkAr[0]} {mrkAr[1]} {mrkAr[2]} {mrkAr[3]} {mrkAr[4]}>>  plAr<<{plAr[0]} {plAr[1]} {plAr[2]} {plAr[3]} {plAr[4]}>>");
            }
            if (mode == 2)
            {
                mrkAr[0] -= count;
                mrkAr[currentIndTgPl % 5] += count;
                plAr[0] += count;
                plAr[currentIndTgPl % 5] -= count;
                arTxtRes[0].text = mrkAr[0].ToString();
                arTxtRes[currentIndTgPl % 5].text = mrkAr[currentIndTgPl % 5].ToString();
                arTxtRes[5].text = plAr[0].ToString();
                arTxtRes[currentIndTgPl].text = plAr[currentIndTgPl % 5].ToString();
                print($"OnClickExchange  Продать  mkrAr<<{mrkAr[0]} {mrkAr[1]} {mrkAr[2]} {mrkAr[3]} {mrkAr[4]}>>  plAr<<{plAr[0]} {plAr[1]} {plAr[2]} {plAr[3]} {plAr[4]}>>");
            }
            if (mode == 3)
            {
                mrkAr[currentIndTgMrk] -= count;
                mrkAr[currentIndTgPl % 5] += count;
                plAr[currentIndTgMrk] += count;
                plAr[currentIndTgPl % 5] -= count;
                arTxtRes[currentIndTgMrk].text = mrkAr[currentIndTgMrk].ToString();
                arTxtRes[currentIndTgPl % 5].text = mrkAr[currentIndTgPl % 5].ToString();
                arTxtRes[currentIndTgMrk + 5].text = plAr[currentIndTgMrk].ToString();
                arTxtRes[currentIndTgPl].text = plAr[currentIndTgPl % 5].ToString();
                print($"OnClickExchange  менять  mkrAr<<{mrkAr[0]} {mrkAr[1]} {mrkAr[2]} {mrkAr[3]} {mrkAr[4]}>>  plAr<<{plAr[0]} {plAr[1]} {plAr[2]} {plAr[3]} {plAr[4]}>>");
            }
            marketSet.InitArResourse(mrkAr);
            playerSet.InitArResourse(plAr);
            print($"OnClickExchange  mrkRes={marketSet}  plRes={playerSet}  mkrAr<<{mrkAr[0]} {mrkAr[1]} {mrkAr[2]} {mrkAr[3]} {mrkAr[4]}>>  plAr<<{plAr[0]} {plAr[1]} {plAr[2]} {plAr[3]} {plAr[4]}>>");
            playerResPanControl.ViewAllResorses(playerSet);
        }
    }

    public void OnToggleChange(int n)
    {
        print($"n={n}  curIndTgMrk={currentIndTgMrk}  curIndTgPl={currentIndTgPl}");
        count = 0;
        if (n >= 0 && n < 5)
        {
            currentIndTgMrk = n;
        }
        if (n >= 5 && n < 10)
        {
            currentIndTgPl = n;
        }
        if (currentIndTgMrk == 0 && currentIndTgPl > 5)
        {
            btnExchange.transform.GetChild(0).GetComponent<Text>().text = (Language.Instance.CurrentLanguage == "ru") ? "Продать" : "Sell";
            txtCount.text = $"-{count}";
            txtCount.color = Color.red;
            btnExchange.interactable = true;
            mode = 2; //    Продать
        }
        else if ((currentIndTgMrk > 0 && currentIndTgMrk < 5) && currentIndTgPl == 5)
        {
            btnExchange.transform.GetChild(0).GetComponent<Text>().text = (Language.Instance.CurrentLanguage == "ru") ? "Купить" : "Buy";
            txtCount.text = $"+{count}";
            txtCount.color = Color.green;
            btnExchange.interactable = true;
            mode = 1; //    купить
        }
        else if ((currentIndTgMrk > 0 && currentIndTgMrk < 5) && currentIndTgPl > 5)
        {
            btnExchange.transform.GetChild(0).GetComponent<Text>().text = (Language.Instance.CurrentLanguage == "ru") ? "Обменять" : "Exchange";
            txtCount.text = $"{count}";
            txtCount.color = Color.yellow;
            btnExchange.interactable = true;
            mode = 3; //  менять  
        }
        else
        {
            ButtonExchangeStop();
            mode = -1; //   none
        }
        if (mode == -1)
        {
            btnInc.interactable = false;
            btnDec.interactable = false;
        }
        else
        {
            if (count > 0) btnDec.interactable = true;
            btnInc.interactable = true;
        }
    }

    private void ButtonExchangeStop()
    {
        btnExchange.transform.GetChild(0).GetComponent<Text>().text = "???????";
        txtCount.text = $"{count}";
        txtCount.color = Color.black;
        btnExchange.interactable = false;
    }

    private int GetCount(int num)
    {
        switch(num)
        {
            case 0: return marketSet.CountMoney;
            case 1: return marketSet.CountFood;
            case 2: return marketSet.CountTree;
            case 3: return marketSet.CountIron;
            case 4: return marketSet.CountStone;
            case 5: return playerSet.CountMoney;
            case 6: return playerSet.CountFood;
            case 7: return playerSet.CountTree;
            case 8: return playerSet.CountIron;
            case 9: return playerSet.CountStone;
        }
        return -1;
    }
}
