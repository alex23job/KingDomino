using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpPanelControl : MonoBehaviour
{
    [SerializeField] private GameObject[] panelsInfo;
    [SerializeField] private Text txtNumPage;
    [SerializeField] private Text[] txtDescrs;

    private int numPage = 0;

    private string[] descr = { "�� ������� �� �������������� ����� ������ ���������� � ���� ������ ��� ��������. ������ �������� ���� ����� 8 ��������, ������� �� ��������� � ��������� ����������� �� ������� ����. ������ �������� ������� �� ���� �������� ��������� (��� � ������). �������� ����� �������, ������ �� ��� ������ ������� ����, � ����������, ����� �� �� ����� ������� ����. �������� ����� ���������� � ��� ��������� �� ������� ���� ���, ����� ��� ��������� �� ��������� �������� �������� � ����� �� ���������� �� ����� �� ������ ����. ����� ����� ��� ����� ������� �������� ��� ���������� ����, ������� �� ��� ����� ������� ����. ����� �� ������ �� ������������� ��� ������� ��������� ������������ ��������. �������� �� ����� �� ���������� ����� ����� �������� ������ � ���������� ��� ����� ��������� �� ����� ������� ���������. ���� ��������� � ��� �������� ����������, �� ������������ ����� ���� ��������� ������ �� ������ <<���������>> ��� ���. �� ���� ��� ��������� ��� �������� � ��������� ��������� ��� �� ��� ��������. ����� ��� �������������� ������������� ������� � ������������ �� ������� ������. ����� ����� �� ������ ���������� ��������� ����� ��������. >>>",
    "���� �������� ��������� ����������, �� ����� ������� � ��������� ������������ ��� ����� ������ ������ <<��������� ���>> ������ ������. ����� ����� �� �������� ������� �������� ��������� �������� �� ���� �� ���� ������ �������� ��������� � ���������� ������ ���������. ���� �������������, ���� �� ������� ���� ������ ���������� ���������� ��������. ��������������� ��������� ������ � ���������� ����� ��������� �������� ������� (�������, ��������), �� ������� ���������� ������� ���������� �����. ����� ������� ������� � ������� ������ ������ (��������), ������� ����� ������������ ��� ��� ��� � ����������. ����������� ������ �� ������ �������� ������������ �������� �� ��������, �� � �������� ������ ������� ������ (1 ��� 2). ����� ����������� ������ ������� �������� ����������� ��� ��� � ����������. ��������� ���� ��� ������ ������� �������������� ��� ������������ ����� ���� ������ �� ����� ���� ��� ������� ������. ����� ����� ����� ����������� ��� ����� �����, ��������� �� ���� ��������. ���������� ���������� ������������ �������� �� �������� ���������, � ����� �������, ������������ �������� � ������ ��� �� ���������, ����� � �������� �� ��������� ���������.",
    "You play as a medieval lord against the computer and your task is to defeat him. There are 8 cards on top of the playing field, which you select and place on the playing field in turn. Each card consists of two sections of terrain (as in domino). The card can be rotated by right-clicking on it and moved by left-clicking on it. The card must be attached to those already on the playing field so that the type of terrain on the half of the card coincides with the same terrain on one of the squares of the field. After that, you need to select a card for the next move by clicking on it with the left mouse button. Next, you can build resource production on the cells you own. Click on any of the cells marked with your blue marker and see what can be built on such a piece of terrain. If your available resources are sufficient, then production can be built by clicking on a button <<Build>> under it. This completes your next move and the opponent performs the same three actions. After that, the resources produced are calculated and displayed in the panels on the right. Next, you must display the previously selected card again. >>>",
    " If the card cannot be displayed, then you can proceed to the construction of the production or immediately click the <<Finish move>> button on the top right. You can also change the selected card to one of the other three randomly selected and displayed on top of the existing ones. The game ends if it is no longer possible to place cards on the playing field. The cells touching the sides with the same type of terrain form areas (patches, patches), according to which the points scored are calculated. It is important to build areas with a large number of cells (areas) that can belong to both you and the enemy. Constructed buildings not only increase the production of resources on the sites, but also bring their owner stars (1 or 2). The stars in each area are added separately for you and the opponent. The points scored for each area are calculated as the product of the number of all cells and the number of stars for each player. The total number of points is calculated as the sum of the points scored in all areas. You can view the amount of resources produced in the areas, as well as the resources produced by buildings and needed for their construction, in the tables on the following pages."}; 
    // Start is called before the first frame update
    void Start()
    {
        if (numPage < 2) ViewDescr();
    }

    public void OnClickLeft()
    {
        panelsInfo[numPage].SetActive(false);
        numPage += panelsInfo.Length - 1;
        numPage %= panelsInfo.Length;
        txtNumPage.text = $"{(numPage + 1):00}";
        if (numPage < 2)
        {
            ViewDescr();
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
            ViewDescr();
        }
        panelsInfo[numPage].SetActive(true);
    }

    private void ViewDescr()
    {
        if (Language.Instance.CurrentLanguage == "ru")
        {
            txtDescrs[numPage].text = descr[numPage];
        }
        if (Language.Instance.CurrentLanguage == "en")
        {
            txtDescrs[numPage].text = descr[numPage + 2];
        }
    }
}
