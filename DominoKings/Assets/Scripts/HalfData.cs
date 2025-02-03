using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeResurse { none, money, tree, fud, stone, iron};

public class HalfData : MonoBehaviour
{
    //  массив значений вырабатываемых ресурсов в каждом типе местности (LandID - 1)
    //  1 - поле, 2 - луг, 3 - лес, 4 - река, 5 - море, 6 - холмы, 7 - горы
    public static ResourseSet[] LandResourseSets = new ResourseSet[8] { new ResourseSet(-1, 0, 0, 0, 0, 0),
        new ResourseSet(-1, 0, 1, 0, 0, 0), new ResourseSet(-1, 0, 1, 0, 0, 0), new ResourseSet(-1, 0, 0, 1, 0, 0), new ResourseSet(-1, 1, 0, 0, 0, 0),
        new ResourseSet(-1, 0, 1, 0, 0, 0), new ResourseSet(-1, 0, 0, 0, 1, 0), new ResourseSet(-1, 0, 0, 0, 0, 1), };

    public int LandID = 0;
    public int BuildID = 0;
    public TypeResurse typeResourses = TypeResurse.none;
    private int numPlayer = 0;
    public int NumPlayer { get { return numPlayer; } }
    private int numPos = -1;
    public int NumPos { get { return numPos; } }

    private GameObject build = null;

    private bool isPole = false;
    private LevelControl levelControl = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildComplete(GameObject prefab)
    {
        build = Instantiate(prefab);
        build.transform.parent = transform;
        build.transform.localPosition = Vector3.zero;
        BuildID = build.GetComponent<ConstructionData>().BuildID;
    }

    public ResourseSet GetBuildResourses()
    {
        if (BuildID == 0) return HalfData.LandResourseSets[0];
        return build.GetComponent<ConstructionData>().ResSet;
    }

    public ResourseSet GetLandResourse()
    {
        int index = (LandID > 0 && LandID < 8) ? LandID : 0;
        return HalfData.LandResourseSets[index];
    }


    public void SetIsPole(int pos)
    {
        isPole = true;
        numPos = pos;
    }

    public void SetNumPlayer(int num, Transform chip, LevelControl lc)
    {
        numPlayer = num;
        chip.parent = transform;
        chip.localPosition = new Vector3(-0.3f, 0.1f, -0.3f);        
        levelControl = lc;
    }

    private void OnMouseDown()
    {
        //print($"HalfTail isPole = {isPole}");
        if (isPole && Input.GetMouseButtonDown(0))
        {
            levelControl.SetSelectHalfTail(LandID);
        }
    }
}
