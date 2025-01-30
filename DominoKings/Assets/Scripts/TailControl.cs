using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailControl : MonoBehaviour
{    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static GameObject GenerateTail(GameObject prHt1, GameObject prHt2, Vector3 pos)
    {
        GameObject go = new GameObject();
        go.transform.position = pos;
        go.AddComponent(typeof(TailControl));
        Vector3 pos1 = pos, pos2 = pos;
        pos1.z = 0.5f;pos2.z = -0.5f;
        GameObject half1 = Instantiate(prHt1, pos1, Quaternion.identity);
        GameObject half2 = Instantiate(prHt2, pos2, Quaternion.identity);
        half1.transform.parent = go.transform;
        half2.transform.parent = go.transform;
        return go;
    }
}
