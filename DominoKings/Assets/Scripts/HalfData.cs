using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeResurse { none, money, tree, fud, stone, iron};

public class HalfData : MonoBehaviour
{
    //  ������ �������� �������������� �������� � ������ ���� ��������� (LandID - 1)
    //  1 - ����, 2 - ���, 3 - ���, 4 - ����, 5 - ����, 6 - �����, 7 - ����
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

    private GameObject cubeSelect = null;

    private GameObject build = null;
    public string NameConstruction { get { return (build != null) ? build.GetComponent<ConstructionData>().NameConstruction : ""; } }

    public int Stars { get { return ((build != null) ? build.GetComponent<ConstructionData>().CountStars : 0); } }

    private bool isPole = false;
    private LevelControl levelControl = null;
    private float timer = 0.25f;
    private bool isSelect = false;
    private bool isView = false;

    private void Awake()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.parent = transform;
        cube.transform.localScale = new Vector3(1.08f, 0.1f, 1.08f);
        cube.transform.localPosition = new Vector3(0, -0.1f, 0);
        cubeSelect = cube;
        cubeSelect.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelect)
        {
            if (timer > 0) timer -= Time.deltaTime;
            else
            {
                timer = 0.25f;
                isView = !isView;
                cubeSelect.SetActive(isView);
            }
        }
    }

    public void SetSelect(bool zn, Material mat)
    {
        isSelect = zn;
        cubeSelect.GetComponent<MeshRenderer>().materials = new Material[] { mat };
        if (isSelect == false) cubeSelect.SetActive(false);
    }

    public void BuildComplete(GameObject prefab)
    {
        build = Instantiate(prefab);
        build.transform.parent = transform;
        build.transform.localPosition = new Vector3(0, 0.8f, 0);
        ConstructionData construction = build.GetComponent<ConstructionData>();
        BuildID = construction.BuildID;
        construction.InitResourseSet();        
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

    public void SetLevelControl(LevelControl lc)
    {
        levelControl = lc;
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
            if (levelControl != null && numPlayer == 1) levelControl.SetSelectHalfTail(LandID, BuildID, NumPos);
        }
    }
}
