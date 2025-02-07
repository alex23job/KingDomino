using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailControl : MonoBehaviour
{
    [SerializeField] private LevelControl lc;

    private bool isOver = false;
    private bool isMove = false;
    private Vector3 delta = Vector3.zero;
    private Vector3 beginPos = Vector3.zero;
    private int numPlayer = 0;
    private int landID_1 = 0, landID_2 = 0;

    private Material mater = null;

    public int NumPlayer { get { return numPlayer; } }

    public Vector3 BeginPos { get { return beginPos; } }

    public int LandID_1 { get { return landID_1; } }
    public int LandID_2 { get { return landID_2; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isOver && (numPlayer > 0) && Input.GetMouseButtonDown(1))
        {
            //print("mouse down R (1)");
            Rotate();
        }
        if (isMove)
        {
            Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 figPos = transform.position;
            figPos.x = mp.x + delta.x; figPos.z = mp.z + delta.z;
            transform.position = figPos;
        }
    }

    public static GameObject GenerateTail(GameObject prHt1, GameObject prHt2, Vector3 pos, LevelControl lc)
    {
        GameObject go = new GameObject();
        go.transform.position = pos;
        go.AddComponent(typeof(TailControl));
        TailControl tailControl = go.GetComponent<TailControl>();
        tailControl.SetParam(lc);
        go.AddComponent(typeof(BoxCollider));
        go.GetComponent<BoxCollider>().size = new Vector3(1f, 0.2f, 2f);
        Vector3 pos1 = pos, pos2 = pos;
        pos1.z = 0.5f; pos2.z = -0.5f;
        GameObject half1 = Instantiate(prHt1, pos1, Quaternion.identity);
        GameObject half2 = Instantiate(prHt2, pos2, Quaternion.identity);
        half1.transform.parent = go.transform;
        half2.transform.parent = go.transform;
        tailControl.SetHalfLands(prHt1.GetComponent<HalfData>().LandID, prHt2.GetComponent<HalfData>().LandID);
        return go;
    }

    public static GameObject GenerateAdsTail(GameObject prHt1, GameObject prHt2, Vector3 pos, LevelControl lc)
    {
        GameObject go = new GameObject();
        go.transform.position = pos;
        go.AddComponent(typeof(TailControl));
        TailControl tailControl = go.GetComponent<TailControl>();
        tailControl.SetParam(lc);
        go.AddComponent(typeof(BoxCollider));
        go.GetComponent<BoxCollider>().size = new Vector3(2f, 0.2f, 1f);
        Vector3 pos1 = pos, pos2 = pos;
        pos1.x += 0.5f; pos2.x += -0.5f;
        GameObject half1 = Instantiate(prHt1, pos1, Quaternion.identity);
        GameObject half2 = Instantiate(prHt2, pos2, Quaternion.identity);
        half1.transform.parent = go.transform;
        half2.transform.parent = go.transform;
        tailControl.SetHalfLands(prHt1.GetComponent<HalfData>().LandID, prHt2.GetComponent<HalfData>().LandID);
        return go;
    }

    private void SetHalfUnSelect()
    {
        Transform ch1 = transform.GetChild(0);
        Transform ch2 = transform.GetChild(1);
        ch1.gameObject.GetComponent<HalfData>().SetSelect(false, mater);
        ch2.gameObject.GetComponent<HalfData>().SetSelect(false, mater);
    }

    public void SetHalfSelect(bool zn, Material mat)
    {
        mater = mat;
        Transform ch1 = transform.GetChild(0);
        Transform ch2 = transform.GetChild(1);
        ch1.gameObject.GetComponent<HalfData>().SetSelect(zn, mat);
        ch2.gameObject.GetComponent<HalfData>().SetSelect(zn, mat);
    }

    public void SetHalfLands(int land1, int land2)
    {
        landID_1 = land1;
        landID_2 = land2;
    }

    public void Rotate()
    {
        if (NumPlayer == 1)
        {
            Transform h1 = transform.GetChild(0), h2 = transform.GetChild(1);
            Vector3 pos1, pos2;
            if (h1.localPosition.x == 0)
            {
                if (h1.localPosition.z == 0.5f)
                {
                    pos1 = h1.localPosition;
                    pos1.x = 0.5f; pos1.z = 0;
                    pos2 = h2.localPosition;
                    pos2.x = -0.5f; pos2.z = 0;
                    h1.localPosition = pos1;
                    h2.localPosition = pos2;
                }
                else if (h1.localPosition.z == -0.5f)
                {
                    pos1 = h1.localPosition;
                    pos1.x = -0.5f; pos1.z = 0;
                    pos2 = h2.localPosition;
                    pos2.x = 0.5f; pos2.z = 0;
                    h1.localPosition = pos1;
                    h2.localPosition = pos2;
                }
                return;
            }
            if (h1.localPosition.z == 0)
            {
                if (h1.localPosition.x == 0.5f)
                {
                    pos1 = h1.localPosition;
                    pos1.z = -0.5f; pos1.x = 0;
                    pos2 = h2.localPosition;
                    pos2.z = 0.5f; pos2.x = 0;
                    h1.localPosition = pos1;
                    h2.localPosition = pos2;
                }
                else if (h1.localPosition.x == -0.5f)
                {
                    pos1 = h1.localPosition;
                    pos1.z = 0.5f; pos1.x = 0;
                    pos2 = h2.localPosition;
                    pos2.z = -0.5f; pos2.x = 0;
                    h1.localPosition = pos1;
                    h2.localPosition = pos2;
                }
                //return;
            }
        }
    }

    private void OnMouseEnter()
    {
        isOver = true;
    }

    private void OnMouseExit()
    {
        isOver = false;
    }

    private void OnMouseDown()
    {
        if (numPlayer == 0)
        {
            GameObject prefab = lc.SelectCard();
            if (prefab != null) SetNumPlayer(1, prefab);
            return;
        }
        //print($"pos = {transform.position}    down");
        if (NumPlayer == 2) return;
        lc.SetSelectCard(transform.gameObject);
        if (Input.GetMouseButtonDown(0))
        {
            isMove = true;
            beginPos = transform.position;
            Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            delta.x = transform.position.x - mp.x;
            delta.z = transform.position.z - mp.z;
        }
    }

    private void OnMouseUp()
    {
        if (numPlayer == 0) return;
        //print($"pos = {transform.position}    up");
        if (lc.UpSelectCard())
        {
            //SetHalfUnSelect();
            Destroy(transform.gameObject);
        }
        else
        {
            if (beginPos != Vector3.zero) transform.position = beginPos;
        }
        if (!Input.GetMouseButtonDown(0))
        {
            isMove = false;
            delta = Vector3.zero;
        }
    }

    public void SetParam(LevelControl levCntr)
    {
        lc = levCntr;
    }

    public void SetNumPlayer(int num, GameObject prefab) 
    {
        numPlayer = num;
        beginPos = transform.position;
        Transform ch1 = transform.GetChild(0);
        Transform ch2 = transform.GetChild(1);
        if (ch1 != null)
        {
            GameObject chip = Instantiate(prefab);
            ch1.GetComponent<HalfData>().SetNumPlayer(num, chip.transform, lc);
        }
        if (ch2 != null)
        {
            GameObject chip = Instantiate(prefab);
            ch2.GetComponent<HalfData>().SetNumPlayer(num, chip.transform, lc);
        }
    }

    public bool TestLand(int numLand)
    {
        if (transform.GetChild(0).gameObject.GetComponent<HalfData>().LandID == numLand) return true;
        if (transform.GetChild(1).gameObject.GetComponent<HalfData>().LandID == numLand) return true;
        return false;
    }

    public int GetLandID1() => transform.GetChild(0).gameObject.GetComponent<HalfData>().LandID;
    public int GetLandID2() => transform.GetChild(1).gameObject.GetComponent<HalfData>().LandID;
}
