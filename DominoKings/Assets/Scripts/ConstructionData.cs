using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionData : MonoBehaviour
{
    public static List<ResourseSet> BuildResourses = new List<ResourseSet>()
    {
        new ResourseSet(-1, 1, 1, 0, 0, 0), //  ��������
        new ResourseSet(-1, 1, 1, 0, 0, 0), //  �����
        new ResourseSet(-1, 1, 0, 2, 0, 0), //  ���������
        new ResourseSet(-1, 1, 1, 0, 0, 0), //  ����� ������
        new ResourseSet(-1, 1, 1, 0, 0, 0), //  ����
        new ResourseSet(-1, 1, 0, 0, 2, 0), //  �����
        new ResourseSet(-1, 1, 0, 0, 0, 2), //  �����������
        new ResourseSet(-1, 1, 2, 0, 0, 0), //  ������
        new ResourseSet(-1, 1, 2, 0, 0, 0), //  ���
        new ResourseSet(-1, 2, 0, 1, 1, 1), //  ���������
        new ResourseSet(-1, 2, 0, 0, 0, 1), //  ��������
        new ResourseSet(-1, 3, 1, 0, 0, 0), //  ����
        new ResourseSet(-1, 2, 0, 0, 1, 0)  //  �������     
    };

    public static List<ResourseSet> BuildPrice = new List<ResourseSet>()
    {
        new ResourseSet(-1, 6, 2, 2, 1, 2), //  ��������
        new ResourseSet(-1, 8, 4, 2, 0, 2), //  �����
        new ResourseSet(-1, 10, 2, 4, 1, 0), //  ���������
        new ResourseSet(-1, 8, 2, 2, 1, 0), //  ����� ������
        new ResourseSet(-1, 4, 2, 0, 0, 0), //  ����
        new ResourseSet(-1, 10, 2, 4, 0, 0), //  �����
        new ResourseSet(-1, 6, 2, 2, 1, 0), //  �����������
        new ResourseSet(-1, 4, 2, 0, 0, 0), //  ������
        new ResourseSet(-1, 4, 2, 2, 0, 0), //  ���
        new ResourseSet(-1, 20, 10, 8, 2, 6), //  ���������
        new ResourseSet(-1, 10, 2, 2, 0, 2), //  ��������
        new ResourseSet(-1, 20, 8, 6, 2, 6), //  ����
        new ResourseSet(-1, 12, 2, 4, 1, 2)  //  �������     
    };

    //  ������ ID ������, ������� ����� ��������� �� ����� ��������� (LandID - 1)
    //  1 ���� - ��������(1), ������(8); 2 ��� - �����(2), ���(9); 3 ��� - ���������(3), ���������(10); 4 ���� - ����� ������(4), ��������(11)
    //  5 ���� - ����(5), ����(12); 6 ����� - �����(6), �������(13); 7 - �����������(7), ���������(10)
    public static int[][] LandsBuilds = new int[][] { new int[] { 1, 8 }, new int[] { 2, 9 }, new int[] { 3, 10 }, new int[] { 4, 11 }, new int[] { 5, 12 }, new int[] { 6, 13 }, new int[] { 7, 10 } };

    [SerializeField] private int buildID;
    [SerializeField] private string nameRu;
    [SerializeField] private string nameEn;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int cntStars = 0;
    [SerializeField] private ResourseSet resSet;

    public ConstructionData()
    {
        if (buildID > 0 && buildID <= ConstructionData.BuildResourses.Count) resSet = BuildResourses[buildID - 1];
    }
    public int BuildID { get { return buildID; } }
    public ResourseSet ResSet { get { return resSet; } }
    public int CountStars { get { return cntStars; } }
    public Sprite ConstSprite { get { return sprite; } }
    public string NameConstruction { get { return (Language.Instance.CurrentLanguage == "ru" ? nameRu : nameEn); } }

    public void InitResourseSet()
    {
        if ((buildID > 0) && (buildID <= ConstructionData.BuildResourses.Count)) resSet = ConstructionData.BuildResourses[buildID - 1];
        print($"buildID={buildID} {resSet}");
    }
}
