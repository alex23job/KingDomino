using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    [SerializeField] private GameObject[] halfTails;
    [SerializeField] private GameObject[] builds;

    private List<GameObject> newTails;
    private GameObject[] poleTails;

    private GameObject selectCard = null;

    // Start is called before the first frame update
    void Start()
    {
        newTails = new List<GameObject>();
        for(int i = 0; i < 8; i++) GenerateTail();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateTail(int mode = 0)
    {
        if (mode == 0)
        {
            if (newTails.Count < 8)
            {
                Vector3 pos = new Vector3(-4.375f + 1.25f * newTails.Count, 1f, 0);
                int ht1 = Random.Range(0, halfTails.Length);
                int ht2 = Random.Range(0, halfTails.Length);
                GameObject go = TailControl.GenerateTail(halfTails[ht1], halfTails[ht2], pos, GetComponent<LevelControl>());
                pos.z = 4.8f;
                go.transform.position = pos;
                newTails.Add(go);
            }
        }
    }

    public void SetSelectCard(GameObject go)
    {

    }

    public void UpSelectCard()
    {

    }
}
