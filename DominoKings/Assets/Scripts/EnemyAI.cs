using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public TailPos2 SelectCardPos(int land1, int land2, int[] pole, List<Loskut> arLos)
    {
        print($"EnemyAI SelectCardPos l1={land1} l2={land2} cntLos={arLos.Count}");
        List<TailPos2> ar = new List<TailPos2>();
        List<Loskut> tmpLos = new List<Loskut>();

        foreach(Loskut los in arLos)
        {
            if (land1 == los.LandID)
            {
                tmpLos.Add(los);
            }
            if (land2 == los.LandID)
            {
                tmpLos.Add(los);
            }
        }
        if (tmpLos.Count > 0) print($"tmpLos.Count={tmpLos.Count}    tmpLos[0]=<< {tmpLos[0]} >>");
        List<HalfData> aH;
        int x, y;
        foreach(Loskut los in tmpLos)
        {   //  собираем в массив возможные варианты установки карты рядом с занятой клеткой такой же местности
            aH = los.GetAr();
            foreach (HalfData hd in aH)
            {
                x = hd.NumPos % 10; y = hd.NumPos / 10;
                if ((x > 0) && (pole[hd.NumPos - 1] == 0))
                {   //  клетка слева свободна - а дальше в 3 стороны?
                    if ((x > 1) && (pole[hd.NumPos - 2] == 0)) ar.Add(new TailPos2(hd.NumPos - 1, hd.NumPos - 2, los.CountBotHalf, los.LandID));
                    if ((y > 0) && (pole[hd.NumPos - 11] == 0)) ar.Add(new TailPos2(hd.NumPos - 1, hd.NumPos - 11, los.CountBotHalf, los.LandID));
                    if ((y < 9) && (pole[hd.NumPos + 9] == 0)) ar.Add(new TailPos2(hd.NumPos - 1, hd.NumPos + 9, los.CountBotHalf, los.LandID));
                }
                if ((x < 9) && (pole[hd.NumPos + 1] == 0))
                {   //  клетка справа свободна - а дальше в 3 стороны?
                    if ((x < 8) && (pole[hd.NumPos + 2] == 0)) ar.Add(new TailPos2(hd.NumPos + 1, hd.NumPos + 2, los.CountBotHalf, los.LandID));
                    if ((y > 0) && (pole[hd.NumPos - 9] == 0)) ar.Add(new TailPos2(hd.NumPos + 1, hd.NumPos - 9, los.CountBotHalf, los.LandID));
                    if ((y < 9) && (pole[hd.NumPos + 11] == 0)) ar.Add(new TailPos2(hd.NumPos + 1, hd.NumPos + 11, los.CountBotHalf, los.LandID));
                }
                if ((y > 0) && (pole[hd.NumPos - 10] == 0))
                {   //  клетка сверху свободна - а дальше в 3 стороны?
                    if ((y > 1) && (pole[hd.NumPos - 20] == 0)) ar.Add(new TailPos2(hd.NumPos - 10, hd.NumPos - 20, los.CountBotHalf, los.LandID));
                    if ((x > 0) && (pole[hd.NumPos - 11] == 0)) ar.Add(new TailPos2(hd.NumPos - 10, hd.NumPos - 11, los.CountBotHalf, los.LandID));
                    if ((x < 9) && (pole[hd.NumPos - 9] == 0)) ar.Add(new TailPos2(hd.NumPos - 10, hd.NumPos - 9, los.CountBotHalf, los.LandID));
                }
                if ((y < 9) && (pole[hd.NumPos + 10] == 0))
                {   //  клетка снизу свободна - а дальше в 3 стороны?
                    if ((y < 8) && (pole[hd.NumPos + 20] == 0)) ar.Add(new TailPos2(hd.NumPos + 10, hd.NumPos + 20, los.CountBotHalf, los.LandID));
                    if ((x > 0) && (pole[hd.NumPos + 9] == 0)) ar.Add(new TailPos2(hd.NumPos + 10, hd.NumPos + 9, los.CountBotHalf, los.LandID));
                    if ((x < 9) && (pole[hd.NumPos + 11] == 0)) ar.Add(new TailPos2(hd.NumPos + 10, hd.NumPos + 11, los.CountBotHalf, los.LandID));
                }
            }
        }
        print($"SelectCardPos => tp2.count={ar.Count} tp2[0]=<< {(ar.Count > 0 ? ar[0] : "none ?")} >>  tp2[1]=<< {(ar.Count > 1 ? ar[1] : "none ?")} >>");
        if (land1 != land2)
        {   //  половинки карточки разные - добавляем значимость возможной пристыковки 2 половинки
            foreach(TailPos2 tp in ar)
            {
                bool isAdding;
                foreach(Loskut los in tmpLos)
                {
                    isAdding = false;
                    if (los.LandID != tp.land)
                    {
                        x = tp.hf2 % 10; y = tp.hf2 / 10;
                        aH = los.GetAr();
                        foreach (HalfData hd in aH)
                        {
                            if ((x > 0) && (tp.hf2 - 1 == hd.NumPos)) isAdding = true;
                            if ((x < 0) && (tp.hf2 + 1 == hd.NumPos)) isAdding = true;
                            if ((y > 0) && (tp.hf2 - 10 == hd.NumPos)) isAdding = true;
                            if ((y < 0) && (tp.hf2 + 10 == hd.NumPos)) isAdding = true;
                        }
                    }
                    if (isAdding) tp.countHalf += los.CountBotHalf;
                }
            }
        }
        int maxCountBotHalf = -1;
        TailPos2 res = new TailPos2(-1, -1, 0, 0);
        foreach(TailPos2 tp in ar)
        {
            if (maxCountBotHalf < tp.countHalf)
            {
                res = tp;
                maxCountBotHalf = tp.countHalf;
            }
        }
        print($"SelectCardPos res=<< {res} >>");
        if (maxCountBotHalf != -1) return res;
        return new TailPos2(-1, -1);
    }

    /// <summary>
    /// Enemy выбирает карточку для следующего хода
    /// </summary>
    /// <param name="pole">массив клеток поля 10х10=100</param>
    /// <param name="cards">массив новых карточек 8 штук</param>
    /// <returns>номер карточки</returns>
    public int GetNextTail(int[] pole, List<GameObject> cards)
    {
        for (int k = 0; k < cards.Count; k++)
        {
            TailControl tc = cards[k].GetComponent<TailControl>();
            for (int i = 0; i < 90; i++)
            {
                if (pole[i] == 0)
                {
                    if (pole[i + 10] == 0)
                    {
                        if (TestNeighboringCells(i, i + 10, tc.GetLandID1(), tc.GetLandID2(), pole)) return k;
                    }
                    if ((i < 9) && (pole[i + 1] == 0))
                    {
                        if (TestNeighboringCells(i, i + 1, tc.GetLandID1(), tc.GetLandID2(), pole)) return k;
                    }
                }
            }
        }
        return -1;
    }

    private bool TestNeighboringCells(int i1, int i2, int zn1, int zn2, int[] pole)
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
}
