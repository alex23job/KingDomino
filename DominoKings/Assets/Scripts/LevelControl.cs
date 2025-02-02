using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    [SerializeField] private UI_Control ui_Control;
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private GameObject[] halfTails;
    [SerializeField] private GameObject[] builds;

    [SerializeField] private GameObject chipRed, chipBlue;

    private List<GameObject> newTails;
    private List<int> numTail;
    private GameObject[] poleTails;
    private int[] pole;

    private int numStep = 0;

    private GameObject selectCard = null;

    private ResourseSet playerRes, botRes;

    private List<Loskut> arLos = new List<Loskut>();

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
        for (int i = 0; i < 8; i++)
        {
            GenerateTail();
        }
        //GameObject chBlue = Instantiate(chipBlue), chRed = Instantiate(chipRed);
        newTails[0].GetComponent<TailControl>().SetNumPlayer(1, chipBlue);
        newTails[1].GetComponent<TailControl>().SetNumPlayer(2, chipRed);
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
            if (newTails.Count == 0) num = Random.Range(0, 14);
            if (newTails.Count == 1) num = Random.Range(13, 25);
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

    public GameObject SelectCard()
    {
        if (numStep == 1)
        {
            numStep++;
            return chipBlue;
        }
        else return null;
    }

    public void EndPlayerGame()
    {
        numStep++;
    }

    public void SetSelectHalfTail(int landID)
    {
        //print($"SetSelectHalfTail land={landID}");
        ui_Control.ViewHintBuildPanel();
    }

    public void SetSelectCard(GameObject go)
    {
        if (numStep == 0)
        {
            selectCard = go;
        }
    }

    public bool UpSelectCard()
    {
        if (selectCard == null) return false;
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
        {   //  €чейки под карточкой пустые -> проверить соседние
            if (TestNeighboringCells(si1, si2, land1, land2))
            {   //  хот€ бы в одной из €чеек р€дом есть одинаковый ландшафт
                //  и можно установить карточку как предлагает игрок
                Vector3 pos = new Vector3((si1 % 10) - 5.5f, 0, 4.5f - si1 / 10);
                h1.transform.parent = null;
                poleTails[si1] = h1;
                h1.transform.position = pos;
                h1.GetComponent<HalfData>().SetIsPole(si1);
                h1.AddComponent(typeof(BoxCollider));
                h1.GetComponent<BoxCollider>().size = new Vector3(1f, 0.2f, 1f);

                pos = new Vector3((si2 % 10) - 5.5f, 0, 4.5f - si2 / 10);
                h2.transform.parent = null;
                poleTails[si2] = h2;
                h2.transform.position = pos;
                h2.GetComponent<HalfData>().SetIsPole(si2);
                h2.AddComponent(typeof(BoxCollider));
                h2.GetComponent<BoxCollider>().size = new Vector3(1f, 0.2f, 1f);

                AddToLoskutAr(h1.GetComponent<HalfData>(), h2.GetComponent<HalfData>());
                pole[si1] = land1;
                pole[si2] = land2;

                int indNew = Mathf.RoundToInt((selectCard.GetComponent<TailControl>().BeginPos.x + 5.375f) / 1.25f);
                GenerateTail(indNew);
                numStep++;
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
        //  горизонтально карточка
        if ((x1 > 0) && (x2 > x1) && (zn1 == pole[i1 - 1])) return true;    //  слева такой же ландшафт
        if ((x2 > 0) && (x2 < x1) && (zn2 == pole[i2 - 1])) return true;    //  слева такой же ландшафт
        if ((y1 > 0) && (y1 == y2) && ((zn1 == pole[i1 - 10]) || (zn2 == pole[i2 - 10]))) return true;   //  сверху или снизу такой же ландшафт
        if ((y1 < 9) && (y1 == y2) && ((zn1 == pole[i1 + 10]) || (zn2 == pole[i2 + 10]))) return true;  //  сверху или снизу такой же ландшафт
        if ((x2 < 9) && (x2 > x1) && (zn2 == pole[i2 + 1])) return true;    //  справа такой же ландшафт
        if ((x1 < 9) && (x2 < x1) && (zn1 == pole[i1 + 1])) return true;    //  справа такой же ландшафт
        //  вертикальна€ карточка
        if ((y1 > 0) && (y2 > y1) && (zn1 == pole[i1 - 10])) return true;   //  сверху такой же ландшафт
        if ((y2 > 0) && (y2 < y1) && (zn2 == pole[i2 - 10])) return true;   //  сверху такой же ландшафт
        if ((x1 > 0) && (x1 == x2) && ((zn1 == pole[i1 - 1]) || (zn2 == pole[i2 - 1]))) return true;   //  слева или справа такой же ландшафт
        if ((x1 < 9) && (x1 == x2) && ((zn1 == pole[i1 + 1]) || (zn2 == pole[i2 + 1]))) return true;  //  слева или справа такой же ландшафт
        if ((y1 < 9) && (y2 < y1) && (zn1 == pole[i1 + 10])) return true;   //  снизу такой же ландшафт
        if ((y2 < 9) && (y2 > y1) && (zn2 == pole[i2 + 10])) return true;   //  снизу такой же ландшафт
        return false;
    }

    private void AddToLoskutAr(HalfData hd1, HalfData hd2)
    {
        List<Loskut> tmpLoskuts = new List<Loskut>();

        //  массив окружающих €чеек, которые надо проверить на принадлежность к лоскутам
        List<int> posTails = new List<int>();
        int i, x = hd1.NumPos % 10, y = hd1.NumPos / 10;
        if (x > 0 && (hd1.NumPos - 1 != hd2.NumPos)) posTails.Add(hd1.NumPos - 1);
        if (x < 9 && (hd1.NumPos + 1 != hd2.NumPos)) posTails.Add(hd1.NumPos + 1);
        if (y > 0 && (hd1.NumPos - 10 != hd2.NumPos)) posTails.Add(hd1.NumPos - 10);
        if (y < 9 && (hd1.NumPos + 10 != hd2.NumPos)) posTails.Add(hd1.NumPos + 10);
        foreach (Loskut los in arLos)
        {
            if (los.LandID == hd1.LandID)
            {
                foreach (int n in posTails)
                {
                    if (los.IsContains(n))
                    {
                        if (tmpLoskuts.Contains(los) == false) { tmpLoskuts.Add(los); break; }
                    }
                }
            }
        }
        if (tmpLoskuts.Count == 0)
        {
            arLos.Add(new Loskut(hd1));
        }
        else if (tmpLoskuts.Count == 1) tmpLoskuts[0].AddHalf(hd1);
        else if (tmpLoskuts.Count > 1)
        {
            for (i = 1; i < tmpLoskuts.Count; i++)
            {
                tmpLoskuts[0].AddLoskut(tmpLoskuts[i]);
                arLos.Remove(tmpLoskuts[i]);
            }
            tmpLoskuts[0].AddHalf(hd1);
        }

        x = hd2.NumPos % 10; y = hd2.NumPos / 10;
        posTails.Clear();tmpLoskuts.Clear();
        //if (x > 0 && (hd2.NumPos - 1 != hd1.NumPos)) posTails.Add(hd2.NumPos - 1);
        //if (x < 9 && (hd2.NumPos + 1 != hd1.NumPos)) posTails.Add(hd2.NumPos + 1);
        //if (y > 0 && (hd2.NumPos - 10 != hd1.NumPos)) posTails.Add(hd2.NumPos - 10);
        //if (y < 9 && (hd2.NumPos + 10 != hd1.NumPos)) posTails.Add(hd2.NumPos + 10);
        if (x > 0) posTails.Add(hd2.NumPos - 1);
        if (x < 9) posTails.Add(hd2.NumPos + 1);
        if (y > 0) posTails.Add(hd2.NumPos - 10);
        if (y < 9) posTails.Add(hd2.NumPos + 10);
        foreach (Loskut los in arLos)
        {
            if (los.LandID == hd2.LandID)
            {
                foreach (int n in posTails)
                {
                    if (los.IsContains(n))
                    {
                        if (tmpLoskuts.Contains(los) == false) { tmpLoskuts.Add(los); break; }
                    }
                }
            }
        }
        if (tmpLoskuts.Count == 0)
        {
            arLos.Add(new Loskut(hd2));
        }
        else if (tmpLoskuts.Count == 1) tmpLoskuts[0].AddHalf(hd2);
        else if (tmpLoskuts.Count > 1)
        {
            for (i = 1; i < tmpLoskuts.Count; i++)
            {
                tmpLoskuts[0].AddLoskut(tmpLoskuts[i]);
                arLos.Remove(tmpLoskuts[i]);
            }
            tmpLoskuts[0].AddHalf(hd2);
        }
        StringBuilder sb = new StringBuilder($"arLos.Count={arLos.Count} => ");
        for (i = 0; i < arLos.Count; i++) sb.Append($"<< i={i} {arLos[i]} >> ");
        print(sb.ToString());
    }
}
