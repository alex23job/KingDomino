using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private int[] arBuilds;
    // Start is called before the first frame update
    void Start()
    {
        arBuilds = new int[15] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ��� �������� ������ ��� ������������� ������
    /// </summary>
    /// <param name="botRes">��������� ������� ����</param>
    /// <param name="arLos">������ ��������(��������) �� ���������</param>
    /// <param name="poleTails">������ ���� ���������</param>
    /// <returns>������� ��������� ��� ���������: ������ ���������, ����� ���������� ������ 0 ��� 1,
    /// ����� ��� ����������� ����� ������, ��������� ���������</returns>
    public TailPos2 SelectBuildPos(ResourseSet botRes, List<Loskut> arLos, GameObject[] poleTails)
    {
        List<HalfData> ar;
        List<TailPos2> candidat = new List<TailPos2>();
        int b1, b2;
        foreach(Loskut los in arLos)
        {
            if (los.CountBotHalf > 0)
            {
                ar = los.GetAr();
                foreach(HalfData hd in ar)
                {
                    if ((hd.NumPlayer == 2) && (hd.BuildID == 0))
                    {
                        b1 = ConstructionData.LandsBuilds[los.LandID - 1][0];
                        b2 = ConstructionData.LandsBuilds[los.LandID - 1][1];
                        if (arBuilds[b1 - 1] == 0)
                        {
                            if (botRes.CheckResourse(ConstructionData.BuildPrice[b1 - 1]))
                            {   //  �������� ������� -> ������ ���
                                return new TailPos2(hd.NumPos, 0, arBuilds[b1 - 1], los.LandID);
                            }
                        }
                        else
                        {
                            if (arBuilds[b2 - 1] == 0)
                            {
                                if (botRes.CheckResourse(ConstructionData.BuildPrice[b2 - 1]))
                                {   //  �������� ������� -> ������ ���
                                    return new TailPos2(hd.NumPos, 0, arBuilds[b2 - 1], los.LandID);
                                }
                            }
                            //  ���� ����� �� ����, �� ����� ������ ��� ���������. �������� ������� �������� ��� ��������� � ������� �����������.
                            if (botRes.CheckResourse(ConstructionData.BuildPrice[b2 - 1])) candidat.Add(new TailPos2(hd.NumPos, 0, arBuilds[b1 - 1], los.LandID));
                            if (botRes.CheckResourse(ConstructionData.BuildPrice[b1 - 1])) candidat.Add(new TailPos2(hd.NumPos, 1, arBuilds[b2 - 1], los.LandID));
                        }
                    }
                }
            }
        }
        //  �������� �� ������ ���������� ��� ��������� ������� ������ ����� ������� ������ ��� ������
        int minBuild = 100, i, indexMin = 0;
        for(i = 0; i < candidat.Count; i++)
        {
            if (candidat[i].countHalf < minBuild)
            {
                minBuild = candidat[i].countHalf; indexMin = i;
            }
        }
        
        if (minBuild < 100)
        {
            print($"candidat.Count={candidat.Count} candidat[0]={candidat[0]}");
            return candidat[indexMin];
        }
        return new TailPos2(-1, -1);
    }

    /// <summary>
    /// ����� ������� ��� ��������� ��������
    /// </summary>
    /// <param name="land1">��������� 1 ���������</param>
    /// <param name="land2">��������� 2 ���������</param>
    /// <param name="pole">������ ���� ���������</param>
    /// <param name="arLos">������ ���� ��������(��������) �� ���������</param>
    /// <returns>������� ��������: ������ ��� 1 ���������, ������ ��� 2 ���������,
    /// ������������ ����� �������� ���������, ��������� 1 ���������</returns>
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
        {   //  �������� � ������ ��������� �������� ��������� ����� ����� � ������� ������� ����� �� ���������
            aH = los.GetAr();
            foreach (HalfData hd in aH)
            {
                x = hd.NumPos % 10; y = hd.NumPos / 10;
                if ((x > 0) && (pole[hd.NumPos - 1] == 0))
                {   //  ������ ����� �������� - � ������ � 3 �������?
                    if ((x > 1) && (pole[hd.NumPos - 2] == 0)) ar.Add(new TailPos2(hd.NumPos - 1, hd.NumPos - 2, los.CountBotHalf, los.LandID));
                    if ((y > 0) && (pole[hd.NumPos - 11] == 0)) ar.Add(new TailPos2(hd.NumPos - 1, hd.NumPos - 11, los.CountBotHalf, los.LandID));
                    if ((y < 9) && (pole[hd.NumPos + 9] == 0)) ar.Add(new TailPos2(hd.NumPos - 1, hd.NumPos + 9, los.CountBotHalf, los.LandID));
                }
                if ((x < 9) && (pole[hd.NumPos + 1] == 0))
                {   //  ������ ������ �������� - � ������ � 3 �������?
                    if ((x < 8) && (pole[hd.NumPos + 2] == 0)) ar.Add(new TailPos2(hd.NumPos + 1, hd.NumPos + 2, los.CountBotHalf, los.LandID));
                    if ((y > 0) && (pole[hd.NumPos - 9] == 0)) ar.Add(new TailPos2(hd.NumPos + 1, hd.NumPos - 9, los.CountBotHalf, los.LandID));
                    if ((y < 9) && (pole[hd.NumPos + 11] == 0)) ar.Add(new TailPos2(hd.NumPos + 1, hd.NumPos + 11, los.CountBotHalf, los.LandID));
                }
                if ((y > 0) && (pole[hd.NumPos - 10] == 0))
                {   //  ������ ������ �������� - � ������ � 3 �������?
                    if ((y > 1) && (pole[hd.NumPos - 20] == 0)) ar.Add(new TailPos2(hd.NumPos - 10, hd.NumPos - 20, los.CountBotHalf, los.LandID));
                    if ((x > 0) && (pole[hd.NumPos - 11] == 0)) ar.Add(new TailPos2(hd.NumPos - 10, hd.NumPos - 11, los.CountBotHalf, los.LandID));
                    if ((x < 9) && (pole[hd.NumPos - 9] == 0)) ar.Add(new TailPos2(hd.NumPos - 10, hd.NumPos - 9, los.CountBotHalf, los.LandID));
                }
                if ((y < 9) && (pole[hd.NumPos + 10] == 0))
                {   //  ������ ����� �������� - � ������ � 3 �������?
                    if ((y < 8) && (pole[hd.NumPos + 20] == 0)) ar.Add(new TailPos2(hd.NumPos + 10, hd.NumPos + 20, los.CountBotHalf, los.LandID));
                    if ((x > 0) && (pole[hd.NumPos + 9] == 0)) ar.Add(new TailPos2(hd.NumPos + 10, hd.NumPos + 9, los.CountBotHalf, los.LandID));
                    if ((x < 9) && (pole[hd.NumPos + 11] == 0)) ar.Add(new TailPos2(hd.NumPos + 10, hd.NumPos + 11, los.CountBotHalf, los.LandID));
                }
            }
        }
        print($"SelectCardPos => tp2.count={ar.Count} tp2[0]=<< {(ar.Count > 0 ? ar[0] : "none ?")} >>  tp2[1]=<< {(ar.Count > 1 ? ar[1] : "none ?")} >>");
        if (land1 != land2)
        {   //  ��������� �������� ������ - ��������� ���������� ��������� ����������� 2 ���������
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
    /// Enemy �������� �������� ��� ���������� ����
    /// </summary>
    /// <param name="pole">������ ������ ���� 10�10=100</param>
    /// <param name="cards">������ ����� �������� 8 ����</param>
    /// <returns>����� ��������</returns>
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
