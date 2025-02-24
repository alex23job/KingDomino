using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImgBlinkControl : MonoBehaviour
{
    private Image imgFone;
    private float timer = 0.1f;
    private float deltaFiolet = 0;
    private bool isDeltaUp = true;

    private void Awake()
    {
        imgFone = gameObject.GetComponent<Image>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;
        else
        {
            timer = 0.05f;
            if (isDeltaUp)
            {
                //deltaFiolet += Time.deltaTime;
                deltaFiolet += 0.05f;
                if (deltaFiolet > 0.65f)
                {
                    deltaFiolet = 0.65f;
                    isDeltaUp = false;
                }
            }
            else
            {
                //deltaFiolet -= Time.deltaTime;
                deltaFiolet -= 0.05f;
                if (deltaFiolet < 0)
                {
                    deltaFiolet = 0;
                    isDeltaUp = true;
                }
            }
            Color col = new Color(0.35f + deltaFiolet, 0.001f + deltaFiolet, 0.2f);
            imgFone.color = col;
        }
    }
}
