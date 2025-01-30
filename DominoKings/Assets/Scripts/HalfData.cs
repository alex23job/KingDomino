using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeResurse { none, money, tree, fud, stone, iron};

public class HalfData : MonoBehaviour
{
    public int LandID = 0;
    public int BuildID = 0;
    public TypeResurse typeResourses = TypeResurse.none;

    private GameObject build = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
