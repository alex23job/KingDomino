using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    [SerializeField] private Yandex myYandex;

    [SerializeField] private UI_Control ui_Control;
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private GameObject[] halfTails;
    [SerializeField] private GameObject[] builds;

    [SerializeField] private GameObject chipRed, chipBlue;
    [SerializeField] private GameObject adsRect;
    [SerializeField] private GameObject cube;
    [SerializeField] private Material selectHalfMat;

    private List<GameObject> adsTails;
    private List<GameObject> newTails;
    private List<int> numTail;
    private GameObject[] poleTails;
    private int[] pole;
    private int numTailForBuild = -1;

    private int numStep = 0;
    private bool isNoBuild = true;
    private bool isCardPut = false;

    private bool isFocus = true;
    private float timer = 0.1f;

    private GameObject selectCard = null;

    private ResourseSet playerRes, botRes;
    private ResourseSet marketRes;

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
        HalfData hd1 = poleTails[44].GetComponent<HalfData>();
        HalfData hd2 = poleTails[45].GetComponent<HalfData>();

        //  не забыть про построить на половинках замок и рынок
        hd1.SetLevelControl(GetComponent<LevelControl>());
        hd1.BuildComplete(builds[14]);
        hd2.SetLevelControl(GetComponent<LevelControl>());
        hd2.BuildComplete(builds[13]);
        hd1.SetIsPole(44);hd2.SetIsPole(45);
        arLos.Add(new Loskut(hd1));
        arLos.Add(new Loskut(hd2));
        for (int i = 0; i < 8; i++)
        {
            GenerateTail();
        }
        //GameObject chBlue = Instantiate(chipBlue), chRed = Instantiate(chipRed);
        newTails[0].GetComponent<TailControl>().SetNumPlayer(1, chipBlue);
        newTails[0].GetComponent<TailControl>().SetHalfSelect(true, selectHalfMat);
        newTails[1].GetComponent<TailControl>().SetNumPlayer(2, chipRed);
        playerRes = new ResourseSet(1, 10, 4, 2, 0, 0);
        botRes = new ResourseSet(1, 10, 4, 2, 0, 0);
        marketRes = new ResourseSet(1, 100, 10, 10, 10, 10);
        ui_Control.ViewResPlayer(playerRes);
        ui_Control.ViewResBot(botRes);
        ui_Control.ViewNames(0, 0);
        //ui_Control.ViewEndGamePanel(200, 154);
        EndPlayerStep();
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
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = 0.25f;

#if UNITY_WEBGL            
            bool curFocus = myYandex.IsFocus();
            if (curFocus != isFocus)
            {
                isFocus = curFocus;
                if (isFocus) myYandex.GameStart();
                else myYandex.GameStop();
                //print($"isFocus => {isFocus}");
            }
#endif
        }
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
            Invoke("CalcResult", 1f);
        }
    }

    public void Generate3AdsTail(int zn = 0)
    {
        adsRect.SetActive(true);
        adsTails = new List<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            Vector3 pos = new Vector3(-4f + 3f * i, 2.1f, 0);
            int num = Random.Range(0, numTail.Count);
            int ht1 = numTail[num] / 10;
            int ht2 = numTail[num] % 10;
            //numTail.RemoveAt(num);
            GameObject go = TailControl.GenerateAdsTail(halfTails[ht1], halfTails[ht2], pos, GetComponent<LevelControl>());
            pos.z = 5f;
            go.transform.position = pos;
            adsTails.Add(go);
        }
    }

    public void OnAdsClick()
    {
        //Generate3AdsTail();
        myYandex.ClickRewardButton();
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

    private void CalcScore()
    {
        int sc1 = 0, sc2 = 0, count;
        foreach(Loskut los in arLos)
        {
            count = los.CountBotHalf + los.CountPlayerHalf;
            sc1 += count * (1 + los.GetStars(1));
            sc2 += count * (1 + los.GetStars(2));
        }
        ui_Control.ViewNames(sc1, sc2);
    }

    private void CalcResult()
    {
        int sc1 = 0, sc2 = 0, count;
        foreach (Loskut los in arLos)
        {
            count = los.CountBotHalf + los.CountPlayerHalf;
            sc1 += count * (1 + los.GetStars(1));
            sc2 += count * (1 + los.GetStars(2));
        }
        ui_Control.ViewEndGamePanel(sc1, sc2);
    }

    public GameObject SelectCard()
    {
        if (numStep == 0)
        {   //  ранее выбранную карточку некуда поставить и есть карточки за рекламу
            //  из которых выбрали карточку для установки -> ею надо заменить неудачную, а лишние удалить
            if (adsRect.activeInHierarchy) return chipBlue;
        }
        if (numStep == 1)
        {
            numStep++;
            ui_Control.ViewHintBtnFone(true);
            return chipBlue;
        }
        else return null;
    }

    public Vector3 MoveAdsSelectCard(GameObject card)
    {
        int i, indexPlayerNewTail = -1;
        adsRect.SetActive(false);
        TailControl tc = null;
        for (i = 0; i < newTails.Count; i++)
        {
            tc = newTails[i].GetComponent<TailControl>();
            if (tc.NumPlayer == 1)
            {
                indexPlayerNewTail = i;
                break;
            }
        }
        //print($"MoveAdsSelectCard indexPlayerNewTail = {indexPlayerNewTail}");
        if (indexPlayerNewTail != -1)
        {
            card.transform.position = newTails[indexPlayerNewTail].transform.position;
            //print($"MoveAdsSelectCard 1  card.pos={card.transform.position}");
            if (tc != null) Destroy(newTails[indexPlayerNewTail].gameObject);
            //print($"MoveAdsSelectCard 2  card.pos={card.transform.position}  c0.pos={adsTails[0].transform.position}  c1.pos={adsTails[1].transform.position}  c2.pos={adsTails[2].transform.position}");
            newTails[indexPlayerNewTail] = card;
            for(i = 0; i < 3; i++)
            {
                if (adsTails[i].transform.position != card.transform.position) Destroy(adsTails[i]);
            }
            //print($"MoveAdsSelectCard 3  card.pos={card.transform.position}   card.beginPos = {card.GetComponent<TailControl>().BeginPos}");
        }
        adsTails = null;
        card.GetComponent<TailControl>().SetHalfSelect(true, selectHalfMat);
        return card.transform.position;
    }

    public bool CheckSelectNewTail(int num)
    {
        foreach(GameObject go in newTails)
        {
            TailControl tc = go.GetComponent<TailControl>();
            if (tc != null && tc.NumPlayer == num) return true;
        }
        return false;
    }

    public void EndPlayerStep()
    {
        if (CheckSelectNewTail(1) == false)
        {
            string msg = (Language.Instance.CurrentLanguage == "ru") ? "Карточка для следующего хода не выбрана ! Кликните по одной из свободных карточек ..." : "The card for the next move is not selected! Click on one of the free cards ...";
            ui_Control.ViewMsgHint(msg);
            return;
        }
        if (isCardPut == false)
        {
            string msg = (Language.Instance.CurrentLanguage == "ru") ? "Выставьте карточку на поле ! Далее кликните по своему участку местности чтобы построить здание ..." : "Put the card on the field! Next, click on your area to build a building ...";
            ui_Control.ViewMsgHint(msg);
            return;
        }
        numStep++;
        ui_Control.ViewHintBtnFone(false);
        CollectResoure(playerRes, 1);
        int i, ln1 = 0, ln2 = 0;
        TailControl tc = null;
        for (i = 0; i < 8; i++)
        {
            tc = newTails[i].GetComponent<TailControl>();
            if (tc.NumPlayer == 2)
            {
                ln1 = tc.LandID_1; ln2 = tc.LandID_2;
                break;
            }
        }
        //print($"EndPlayerStep =>  ln1 = {ln1} ln2 = {ln2}");
        TailPos2 tp2 = enemyAI.SelectCardPos(ln1, ln2, pole, arLos);
        if (tp2.hf1 != -1 && tp2.hf2 != -1)
        {   //  установить карточку на выбранную позицию
            print($"EndPlayerStep =>  res=<< {tp2} >>");
            if (tc != null)
            {
                //  можно установить карточку как предлагает Бот
                GameObject h1 = tc.transform.GetChild(0).gameObject;
                GameObject h2 = tc.transform.GetChild(1).gameObject;
                if (ln1 != tp2.land)
                {
                    h1 = tc.transform.GetChild(1).gameObject;
                    h2 = tc.transform.GetChild(0).gameObject;
                }
                Vector3 pos = new Vector3((tp2.hf1 % 10) - 5.5f, 0, 4.5f - tp2.hf1 / 10);
                h1.transform.parent = null;
                poleTails[tp2.hf1] = h1;
                h1.transform.position = pos;
                h1.GetComponent<HalfData>().SetIsPole(tp2.hf1);
                h1.AddComponent(typeof(BoxCollider));
                h1.GetComponent<BoxCollider>().size = new Vector3(1f, 0.2f, 1f);

                pos = new Vector3((tp2.hf2 % 10) - 5.5f, 0, 4.5f - tp2.hf2 / 10);
                h2.transform.parent = null;
                poleTails[tp2.hf2] = h2;
                h2.transform.position = pos;
                h2.GetComponent<HalfData>().SetIsPole(tp2.hf2);
                h2.AddComponent(typeof(BoxCollider));
                h2.GetComponent<BoxCollider>().size = new Vector3(1f, 0.2f, 1f);

                AddToLoskutAr(h1.GetComponent<HalfData>(), h2.GetComponent<HalfData>());
                pole[tp2.hf1] = ln1;
                pole[tp2.hf2] = ln2;
                if (ln1 != tp2.land)
                {
                    pole[tp2.hf1] = ln2;
                    pole[tp2.hf2] = ln1;
                }

                int indNew = Mathf.RoundToInt((tc.GetComponent<TailControl>().BeginPos.x + 5.375f) / 1.25f);
                GenerateTail(indNew);
                Destroy(tc.gameObject);
            }
        }
        else
        {   //  Бот не может поставить свою карточку => отправим её назад в "колоду"
            int indNew = Mathf.RoundToInt((tc.GetComponent<TailControl>().BeginPos.x + 5.375f) / 1.25f);
            numTail.Add(10 * ln1 + ln2);    //  вернули карточку в массив выбора
            Destroy(tc.gameObject);         //  удалили "плохую" карточку
            GenerateTail(indNew);           //  сгенерили новую карточку
        }
        int numCard = enemyAI.GetNextTail(pole, newTails);
        if (numCard != -1)
        {
            newTails[numCard].GetComponent<TailControl>().SetNumPlayer(2, chipRed);
        }
        TailPos2 tpBuild = enemyAI.SelectBuildPos(botRes, arLos, poleTails);
        print($"EndPlayerStep =>   tpBuild = << {tpBuild} >>");
        if (tpBuild.hf1 != -1 && tpBuild.hf2 != -1)
        {
            EnemyBuilding(tpBuild.hf1, tpBuild.hf2);
        }
        CollectResoure(botRes, 2);
        if (enemyAI.SellResourse(marketRes, botRes)) ui_Control.ViewResBot(botRes);
        numStep = 0;isNoBuild = true;   //  ход Бота и весь ход завершён
        CalcScore();

        foreach(GameObject card in newTails)
        {
            tc = card.GetComponent<TailControl>();
            if (tc.NumPlayer == 1) tc.SetHalfSelect(true, selectHalfMat);
        }
        isCardPut = false;
        ui_Control.UpdateBtnRewAds(true);
    }

    public void SetSelectHalfTail(int landID, int buildID, int numHalfTail)
    {   //  не забыть про замок и рынок
        //print($"SetSelectHalfTail land={landID}");
        if (buildID != 0)
        {   //  здание уже построено - может вывести инфу о нём?
            if (buildID < 14)
            {
                string nmBuild = poleTails[numHalfTail].GetComponent<HalfData>().NameConstruction;
                ui_Control.ViewMsgHint(BuildDescr(buildID, nmBuild));
            }
            if (buildID == 14)
            {   //  чтобы оправдать рынок нужно сделать купи-продай ресурсы
                print("А это рынок ?");
                cube.SetActive(true);
                ui_Control.ViewMarket(marketRes, playerRes);
            }
            if (buildID ==15)
            {
                string strCastle = Language.Instance.CurrentLanguage == "ru" ? "Это замок Короля !" : "This is the King's castle!";
                ui_Control.ViewMsgHint(strCastle);
            }
        }
        else if (landID > 0 && landID < 8 && isNoBuild)
        {
            cube.SetActive(true);
            numTailForBuild = numHalfTail;
            string nm1 = builds[ConstructionData.LandsBuilds[landID - 1][0] - 1].GetComponent<ConstructionData>().NameConstruction;
            string nm2 = builds[ConstructionData.LandsBuilds[landID - 1][1] - 1].GetComponent<ConstructionData>().NameConstruction;
            //print($"SetSelectHalfTail land={landID} {ConstructionData.LandsBuilds[landID - 1]} nm1={nm1} nm2={nm2}");
            ui_Control.ViewHintBuildPanel(landID, playerRes, nm1, nm2);
        }
    }

    private string BuildDescr(int buildID, string nm)
    {
        ResourseSet buildRes = ConstructionData.BuildResourses[buildID - 1];
        StringBuilder sb = new StringBuilder();
        if (Language.Instance.CurrentLanguage == "ru")
        {
            sb.Append($"Построенное здание {nm} производит: деньги +{buildRes.CountMoney}");
            if (buildRes.CountFood > 0) sb.Append($", еда +{buildRes.CountFood}");
            if (buildRes.CountTree > 0) sb.Append($", дерево +{buildRes.CountTree}");
            if (buildRes.CountIron > 0) sb.Append($", железо +{buildRes.CountIron}");
            if (buildRes.CountStone > 0) sb.Append($", камень +{buildRes.CountStone}");
        }
        else
        {
            sb.Append($"The constructed building {nm} produces: money +{buildRes.CountMoney}");
            if (buildRes.CountFood > 0) sb.Append($", food +{buildRes.CountFood}");
            if (buildRes.CountTree > 0) sb.Append($", tree +{buildRes.CountTree}");
            if (buildRes.CountIron > 0) sb.Append($", iron +{buildRes.CountIron}");
            if (buildRes.CountStone > 0) sb.Append($", stone +{buildRes.CountStone}");
        }
        return sb.ToString();
    }

    public void OnClickExitHBP()
    {
        numTailForBuild = -1;
        cube.SetActive(false);
    }

    /// <summary>
    /// Постройка здания по выбору игрока
    /// </summary>
    /// <param name="zn">0 - левое, 1 - правое из двух для вида местности</param>
    public void OnClickBuildHBP(int zn)
    {
        if (isNoBuild)
        {
            HalfData hd = poleTails[numTailForBuild].GetComponent<HalfData>();
            int index = ConstructionData.LandsBuilds[hd.LandID - 1][zn] - 1;
            hd.BuildComplete(builds[index]);
            playerRes.DecrResourse(ConstructionData.BuildPrice[index]);
            ui_Control.ViewResPlayer(playerRes);
        }
        isNoBuild = false;  //  здание на этом ходу построено
        cube.SetActive(false);
        EndPlayerStep();
    }

    /// <summary>
    /// Постройка здания Ботом
    /// </summary>
    /// <param name="numHalf">номер клетки</param>
    /// <param name="zn">0 или 1 - какое из 2 возможных зданий строим</param>
    public void EnemyBuilding(int numHalf, int zn)
    {
        HalfData hd = poleTails[numHalf].GetComponent<HalfData>();
        int index = ConstructionData.LandsBuilds[hd.LandID - 1][zn] - 1;
        hd.BuildComplete(builds[index]);
        botRes.DecrResourse(ConstructionData.BuildPrice[index]);
        ui_Control.ViewResPlayer(botRes);
        //cube.SetActive(false);
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
        {   //  ячейки под карточкой пустые -> проверить соседние
            if (TestNeighboringCells(si1, si2, land1, land2))
            {   //  хотя бы в одной из ячеек рядом есть одинаковый ландшафт
                //  и можно установить карточку как предлагает игрок
                selectCard.gameObject.GetComponent<TailControl>().SetHalfSelect(false, selectHalfMat);
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
                Destroy(selectCard);
                numStep++;
                isCardPut = true;
                ui_Control.UpdateBtnRewAds(false);
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
        //  вертикальная карточка
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

        //  массив окружающих ячеек, которые надо проверить на принадлежность к лоскутам
        List<int> posTails = new List<int>();
        int i, x = hd1.NumPos % 10, y = hd1.NumPos / 10;
        //print($"AddToLoskutAr =>  x={x} y={y}");
        if (x > 0 && (hd1.NumPos - 1 != hd2.NumPos)) posTails.Add(hd1.NumPos - 1);
        if (x < 9 && (hd1.NumPos + 1 != hd2.NumPos)) posTails.Add(hd1.NumPos + 1);
        if (y > 0 && (hd1.NumPos - 10 != hd2.NumPos)) posTails.Add(hd1.NumPos - 10);
        if (y < 9 && (hd1.NumPos + 10 != hd2.NumPos)) posTails.Add(hd1.NumPos + 10);
        print($"AddToLoskutAr =>  posTails.Count={posTails.Count} pT[0]={(posTails.Count > 0 ? posTails[0] : -1)} pT[1]={(posTails.Count > 1 ? posTails[1] : -1)} pT[2]={(posTails.Count > 2 ? posTails[2] : -1)}");
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
        print($"AddToLoskutAr tmpLoskuts.Count = {tmpLoskuts.Count}");
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

    private void CollectResoure(ResourseSet rs, int mode)
    {
        foreach(Loskut los in arLos)
        {
            rs.AddResourse(los.GetResourses(mode));
        }
        if (mode == 1)
        {   //  собираем ресурсы для игрока
            ui_Control.ViewResPlayer(rs);
        }
        if (mode == 2)
        {   //  собираем ресурсы для бота
            ui_Control.ViewResBot(rs);
        }
    }
}
