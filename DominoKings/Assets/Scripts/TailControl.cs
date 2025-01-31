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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isOver && Input.GetMouseButtonDown(1))
        {
            //print("mouse down R (1)");
            Rotate();
        }
        if (isMove)
        {
            Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 figPos = transform.position;
            figPos.x = mp.x + delta.x; figPos.z = mp.z + delta.z;
            //txtDebug.text = $"<{(int)figPos.x},{(int)figPos.z}> x={(int)mp.x} y={(int)mp.z}";
            transform.position = figPos;
        }
    }

    public static GameObject GenerateTail(GameObject prHt1, GameObject prHt2, Vector3 pos, LevelControl lc)
    {
        GameObject go = new GameObject();
        go.transform.position = pos;
        go.AddComponent(typeof(TailControl));
        go.GetComponent<TailControl>().SetParam(lc);
        go.AddComponent(typeof(BoxCollider));
        go.GetComponent<BoxCollider>().size = new Vector3(1f, 0.2f, 2f);
        Vector3 pos1 = pos, pos2 = pos;
        pos1.z = 0.5f;pos2.z = -0.5f;
        GameObject half1 = Instantiate(prHt1, pos1, Quaternion.identity);
        GameObject half2 = Instantiate(prHt2, pos2, Quaternion.identity);
        half1.transform.parent = go.transform;
        half2.transform.parent = go.transform;
        return go;
    }

    public void Rotate()
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
        //print($"pos = {transform.position}    down");
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
        //print($"pos = {transform.position}    up");
        if (lc.UpSelectCard())
        {
            Destroy(transform.gameObject);
        }
        else
        {
            transform.position = beginPos;
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
}
