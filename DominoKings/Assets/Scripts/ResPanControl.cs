using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResPanControl : MonoBehaviour
{
    [SerializeField] private Text txtName;
    [SerializeField] private Text txt1Coin;
    [SerializeField] private Text txt2Food;
    [SerializeField] private Text txt3Tree;
    [SerializeField] private Text txt4Iron;
    [SerializeField] private Text txt5Stone;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ViewName(string nm)
    {
        txtName.text = nm;
    }

    public void ViewCoin(int zn)
    {
        txt1Coin.text = zn.ToString();
    }

    public void ViewFood(int zn)
    {
        txt2Food.text = zn.ToString();
    }

    public void ViewTree(int zn)
    {
        txt3Tree.text = zn.ToString();
    }

    public void ViewIron(int zn)
    {
        txt4Iron.text = zn.ToString();
    }

    public void ViewStone(int zn)
    {
        txt5Stone.text = zn.ToString();
    }

    public void ViewAllResorses(ResourseSet set)
    {
        txt1Coin.text = set.CountMoney.ToString();
        txt2Food.text = set.CountFood.ToString();
        txt3Tree.text = set.CountTree.ToString();
        txt4Iron.text = set.CountIron.ToString();
        txt5Stone.text = set.CountStone.ToString();
    }
}
