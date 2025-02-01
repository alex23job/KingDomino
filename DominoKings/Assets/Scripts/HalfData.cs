using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeResurse { none, money, tree, fud, stone, iron};

public class HalfData : MonoBehaviour
{
    public int LandID = 0;
    public int BuildID = 0;
    public TypeResurse typeResourses = TypeResurse.none;
    private int numPlayer = 0;
    public int NumPlayer { get { return numPlayer; } }

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

    public void SetIsPole()
    {
        isPole = true;
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
        print($"HalfTail isPole = {isPole}");
        if (isPole && Input.GetMouseButtonDown(0))
        {
            levelControl.SetSelectHalfTail(LandID);
        }
    }
}
