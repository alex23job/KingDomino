using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using System.Text;

public class BuildPanelControl : MonoBehaviour
{
    [SerializeField] private Sprite[] spritesBuilds;

    [SerializeField] private Image imgConstr;
    [SerializeField] private Text nameConstr;
    [SerializeField] private Button btnBuild;
    [SerializeField] private Text txtDescr;
    [SerializeField] private Text[] arTxtPriceRes;  //  5 ресурсов: деньги, еда, дерево, железо, камень

    private Color colYes = new Color(0.25f, 0.5f, 0.25f);
    private Color colNo = new Color(1f, 0.58f, 0.58f);
    [SerializeField] private int ID;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetParams(int panelID, int landID, ResourseSet playerRes, string nm)
    {
        if (panelID == 1)
        {
            nameConstr.text = nm;
            ViewBuildResuorse(0, landID, playerRes);
        }
        if (panelID == 2)
        {
            nameConstr.text = nm;
            ViewBuildResuorse(1, landID, playerRes);
        }
    }

    private void ViewBuildResuorse(int num, int landID, ResourseSet playerRes)
    {
        int index = ConstructionData.LandsBuilds[landID - 1][num] - 1;
        print($"index={index} sprBuilds.Count={spritesBuilds.Length}");
        ResourseSet buildRes = ConstructionData.BuildResourses[index];
        ResourseSet priceRes = ConstructionData.BuildPrice[index];
        imgConstr.sprite = spritesBuilds[index];
        StringBuilder sb = new StringBuilder();
        if (Language.Instance.CurrentLanguage == "ru")
        {
            sb.Append($"Производит: деньги +{buildRes.CountMoney}");
            if (buildRes.CountFood > 0) sb.Append($", еда +{buildRes.CountFood}");
            if (buildRes.CountTree > 0) sb.Append($", дерево +{buildRes.CountTree}");
            if (buildRes.CountIron > 0) sb.Append($", железо +{buildRes.CountIron}");
            if (buildRes.CountStone > 0) sb.Append($", камень +{buildRes.CountStone}");

            arTxtPriceRes[0].text = $"Деньги: {priceRes.CountMoney}";
            arTxtPriceRes[0].color = playerRes.CountMoney >= priceRes.CountMoney ? colYes : colNo;
            arTxtPriceRes[1].text = $"Еда : {priceRes.CountFood}";
            arTxtPriceRes[1].color = playerRes.CountFood >= priceRes.CountFood ? colYes : colNo;
            arTxtPriceRes[2].text = $"Дерево: {priceRes.CountTree}";
            arTxtPriceRes[2].color = playerRes.CountTree >= priceRes.CountTree ? colYes : colNo;
            arTxtPriceRes[3].text = $"Железо: {priceRes.CountIron}";
            arTxtPriceRes[3].color = playerRes.CountIron >= priceRes.CountIron ? colYes : colNo;
            arTxtPriceRes[4].text = $"Камень: {priceRes.CountStone}";
            arTxtPriceRes[4].color = playerRes.CountStone >= priceRes.CountStone ? colYes : colNo;

        }
        if (Language.Instance.CurrentLanguage == "en")
        {
            sb.Append($"Produces: money +{buildRes.CountMoney}");
            if (buildRes.CountFood > 0) sb.Append($", food +{buildRes.CountFood}");
            if (buildRes.CountTree > 0) sb.Append($", tree +{buildRes.CountTree}");
            if (buildRes.CountIron > 0) sb.Append($", iron +{buildRes.CountIron}");
            if (buildRes.CountStone > 0) sb.Append($", stone +{buildRes.CountStone}");

            arTxtPriceRes[0].text = $"Coins: {priceRes.CountMoney}";
            arTxtPriceRes[0].color = playerRes.CountMoney >= priceRes.CountMoney ? colYes : colNo;
            arTxtPriceRes[1].text = $"Food : {priceRes.CountFood}";
            arTxtPriceRes[1].color = playerRes.CountFood >= priceRes.CountFood ? colYes : colNo;
            arTxtPriceRes[2].text = $"Tree : {priceRes.CountTree}";
            arTxtPriceRes[2].color = playerRes.CountTree >= priceRes.CountTree ? colYes : colNo;
            arTxtPriceRes[3].text = $"Iron : {priceRes.CountIron}";
            arTxtPriceRes[3].color = playerRes.CountIron >= priceRes.CountIron ? colYes : colNo;
            arTxtPriceRes[4].text = $"Stone: {priceRes.CountStone}";
            arTxtPriceRes[4].color = playerRes.CountStone >= priceRes.CountStone ? colYes : colNo;
        }
        txtDescr.text = sb.ToString();
        bool btnYes = true;
        if (playerRes.CountMoney < priceRes.CountMoney) btnYes = false;
        if (playerRes.CountFood < priceRes.CountFood) btnYes = false;
        if (playerRes.CountTree < priceRes.CountTree) btnYes = false;
        if (playerRes.CountIron < priceRes.CountIron) btnYes = false;
        if (playerRes.CountStone < priceRes.CountStone) btnYes = false;
        btnBuild.interactable = btnYes;
    }
}
