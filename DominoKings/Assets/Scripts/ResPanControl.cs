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

}
