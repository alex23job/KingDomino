using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    [SerializeField] private UI_Control ui_Control;
    [SerializeField] private GameObject[] halfTails;
    [SerializeField] private GameObject[] builds;

    private List<GameObject> newTails;
    private List<int> numTail;
    private GameObject[] poleTails;
    private int[] pole;

    private GameObject selectCard = null;

    // Start is called before the first frame update
    void Start()
    {
        GenerateNumTails();
        newTails = new List<GameObject>();
        poleTails = new GameObject[100];
        pole = new int[100];
        pole[44] = 1;pole[45] = 2;
        poleTails[44] = Instantiate(halfTails[0], new Vector3(-1.5f, 0, 0.5f), Quaternion.identity);
        poleTails[45] = Instantiate(halfTails[1], new Vector3(-0.5f, 0, 0.5f), Quaternion.identity);
        for (int i = 0; i < 8; i++) GenerateTail();
    }

    private void GenerateNumTails()
    {
        numTail = new List<int>();
        int i, j;
        for(i = 0; i < 7; i++)
        {
            for(j = i; j < 7; j++)
            {
                numTail.Add(10 * i + j);
                numTail.Add(10 * i + j);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateTail()
    {
        if (newTails.Count < 8)
        {
            Vector3 pos = new Vector3(-5.375f + 1.25f * newTails.Count, 1f, 0);
            int num = Random.Range(0, numTail.Count);
            int ht1 = numTail[num] / 10;
            int ht2 = numTail[num] % 10;
            numTail.RemoveAt(num);
            GameObject go = TailControl.GenerateTail(halfTails[ht1], halfTails[ht2], pos, GetComponent<LevelControl>());
            pos.z = 5.7f;
            go.transform.position = pos;
            newTails.Add(go);
        }
    }

    private void GenerateTail(int numPos)
    {
        if (numPos >= 0 && numPos < 8)
        {
            Vector3 pos = new Vector3(-5.375f + 1.25f * numPos, 1f, 0);
            int num = Random.Range(0, numTail.Count);
            int ht1 = numTail[num] / 10;
            int ht2 = numTail[num] % 10;
            numTail.RemoveAt(num);
            GameObject go = TailControl.GenerateTail(halfTails[ht1], halfTails[ht2], pos, GetComponent<LevelControl>());
            pos.z = 5.7f;
            go.transform.position = pos;
            newTails[numPos] = go;
        }
        if (TestEndGame())
        {
            CalcResult();
        }
    }

    private bool TestEndGame()
    {
        for(int i = 0; i < 90; i++)
        {
            if (pole[i] == 0)
            {
                if (pole[i + 10] == 0) return false;
                if ((i < 9) && (pole[i + 1] == 0)) return false;
            }
        }
        return true;
    }

    private void CalcResult()
    {
        ui_Control.ViewEndGamePanel(0, 0);
    }

    public void SetSelectCard(GameObject go)
    {
        selectCard = go;
    }

    public bool UpSelectCard()
    {
        bool res = true;
        int si1, si2;
        GameObject h1 = selectCard.transform.GetChild(0).gameObject;
        GameObject h2 = selectCard.transform.GetChild(1).gameObject;
        int land1 = h1.GetComponent<HalfData>().LandID, land2 = h2.GetComponent<HalfData>().LandID;
        int x, y;
        x = Mathf.RoundToInt(h1.transform.position.x + 5.5f); y = Mathf.RoundToInt(3.5f - h1.transform.position.z);
        //print($"h1 =>  x={x}  pos.x={h1.transform.position.x}  y={y}  pos.z={h1.transform.position.z}");
        si1 = ((x >= 0 && x < 10) && (y >= 0 && y < 10)) ? (10 * y + x) : -1;
        x = Mathf.RoundToInt(h2.transform.position.x + 5.5f); y = Mathf.RoundToInt(3.5f - h2.transform.position.z);
        //print($"h2 =>  x={x}  pos.x={h2.transform.position.x}  y={y}  pos.z={h2.transform.position.z}");
        si2 = ((x >= 0 && x < 10) && (y >= 0 && y < 10)) ? (10 * y + x) : -1;
        //print($"UpSelectCard si1={si1}  si2={si2}  ln1={land1}  ln2={land2}");
        if ((si1 != -1 && pole[si1] == 0) && (si2 != -1 && pole[si2] == 0))
        {   //  ������ ��� ��������� ������ -> ��������� ��������
            if (TestNeighboringCells(si1, si2, land1, land2))
            {   //  ���� �� � ����� �� ����� ����� ���� ���������� ��������
                //  � ����� ���������� �������� ��� ���������� �����
                Vector3 pos = new Vector3((si1 % 10) - 5.5f, 0, 4.5f - si1 / 10);
                h1.transform.parent = null;
                poleTails[si1] = h1;
                h1.transform.position = pos;
                pos = new Vector3((si2 % 10) - 5.5f, 0, 4.5f - si2 / 10);
                h2.transform.parent = null;
                poleTails[si2] = h2;
                h2.transform.position = pos;
                pole[si1] = land1;
                pole[si2] = land2;
                int indNew = Mathf.RoundToInt((selectCard.GetComponent<TailControl>().BeginPos.x + 5.375f) / 1.25f);
                //print($"indNew = {indNew}");
                GenerateTail(indNew);
            }
            else res = false;
        }
        else res = false;
        selectCard = null;
        return res;
    }

    private bool TestNeighboringCells(int i1, int i2, int zn1, int zn2)
    {
        //print($"TestCells i1={i1}  i2={i2}  zn1={zn1}  zn2={zn2}");
        int x1 = i1 % 10, x2 = i2 % 10, y1 = i1 / 10, y2 = i2 / 10;
        //  ������������� ��������
        if ((x1 > 0) && (x2 > x1) && (zn1 == pole[i1 - 1])) return true;    //  ����� ����� �� ��������
        if ((x2 > 0) && (x2 < x1) && (zn2 == pole[i2 - 1])) return true;    //  ����� ����� �� ��������
        if ((y1 > 0) && (y1 == y2) && ((zn1 == pole[i1 - 10]) || (zn2 == pole[i2 - 10]))) return true;   //  ������ ��� ����� ����� �� ��������
        if ((y1 < 9) && (y1 == y2) && ((zn1 == pole[i1 + 10]) || (zn2 == pole[i2 + 10]))) return true;  //  ������ ��� ����� ����� �� ��������
        if ((x2 < 9) && (x2 > x1) && (zn2 == pole[i2 + 1])) return true;    //  ������ ����� �� ��������
        if ((x1 < 9) && (x2 < x1) && (zn1 == pole[i1 + 1])) return true;    //  ������ ����� �� ��������
        //  ������������ ��������
        if ((y1 > 0) && (y2 > y1) && (zn1 == pole[i1 - 10])) return true;   //  ������ ����� �� ��������
        if ((y2 > 0) && (y2 < y1) && (zn2 == pole[i2 - 10])) return true;   //  ������ ����� �� ��������
        if ((x1 > 0) && (x1 == x2) && ((zn1 == pole[i1 - 1]) || (zn2 == pole[i2 - 1]))) return true;   //  ����� ��� ������ ����� �� ��������
        if ((x1 < 9) && (x1 == x2) && ((zn1 == pole[i1 + 1]) || (zn2 == pole[i2 + 1]))) return true;  //  ����� ��� ������ ����� �� ��������
        if ((y1 < 9) && (y2 < y1) && (zn1 == pole[i1 + 10])) return true;   //  ����� ����� �� ��������
        if ((y2 < 9) && (y2 > y1) && (zn2 == pole[i2 + 10])) return true;   //  ����� ����� �� ��������
        return false;
    }
}
