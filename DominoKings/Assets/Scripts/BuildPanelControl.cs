using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class BuildPanelControl : MonoBehaviour
{
    [SerializeField] private Sprite[] spritesBuilds;

    [SerializeField] private Image imgConstr;
    [SerializeField] private Text nameConstr;
    [SerializeField] private Button btnBuild;
    [SerializeField] private Text txtDescr;
    [SerializeField] private Text[] arTxtPriceRes;  //  5 ресурсов: деньги, еда, дерево, железо, камень

    private Color colYes = new Color(0.25f, 1f, 0.25f);
    private Color colNo = new Color(1f, 0.58f, 0.58f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetParams(Vector3 pos, int landID, ResourseSet playerRes)
    {

    }
}
