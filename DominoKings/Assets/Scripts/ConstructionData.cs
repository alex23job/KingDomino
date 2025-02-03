using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionData : MonoBehaviour
{
    public static List<ResourseSet> BuildResourses = new List<ResourseSet>()
    {
        new ResourseSet(-1, 1, 1, 0, 0, 0), //  мельница
        new ResourseSet(-1, 1, 1, 0, 0, 0), //  ферма
        new ResourseSet(-1, 1, 0, 1, 0, 0)  //  лесопилка
    };

    //  массив ID зданий, которые можно построить на видах местности (LandID - 1)
    //  1 поле - мельница(1), огород(8); 2 луг - ферма(2), сад(9); 3 лес - лесопилка(3), монастырь(10); 4 река - домик рыбака(4), гончарня(11)
    //  5 вода - сети(5), порт(12); 6 холмы - шахта(6), кузница(13); 7 - каменоломня(7), монастырь(10)
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
}
