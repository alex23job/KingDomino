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

    public bool SelectCardPos(int land1, int land2, int[] pole, List<Loskut> arLos)
    {
        List<HalfData> ar;
        foreach(Loskut los in arLos)
        {
            if (land1 == los.LandID)
            {
                ar = los.GetAr();
                foreach(HalfData hd in ar)
                {

                }
            }
        }
        return true;
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
