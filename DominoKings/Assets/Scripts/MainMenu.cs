using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void GetLeaderboardEntries();

    [SerializeField] private SoundPanelControl spControl;
    [SerializeField] private Text[] arTxtRecItems;

    [SerializeField] private RawImage riAvatar;
    [SerializeField] private Text txtName;
    [SerializeField] private Text txtScore;

    [SerializeField] private AudioSource sourceFone;

    // Start is called before the first frame update
    void Start()
    {
        ViewLeaderboard("");
        Invoke("GetLeaderboard", 0.02f);
        if (GameManager.Instance.currentPlayer.isLoaded) UpdateSound();
        if (GameManager.Instance.currentPlayer.totalScore > 0) ViewScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ViewScore()
    {
        txtScore.text = $"{GameManager.Instance.currentPlayer.totalScore}     {GameManager.Instance.currentPlayer.countWin}({GameManager.Instance.currentPlayer.countBattle})";
    }

    public void UpdateSound()
    {
        if (GameManager.Instance.currentPlayer.isSoundFone)
        {
            sourceFone.volume = (float)GameManager.Instance.currentPlayer.volumeFone / 100.0f;
            sourceFone.Play();
        }
        else
        {
            sourceFone.Stop();
        }
        spControl.UpdateSound();
    }


    public void GetLeaderboard()
    {
#if UNITY_WEBGL
        GetLeaderboardEntries();
#endif
        /*MyArrRecords test = new MyArrRecords();
        test.records = new PersonRecord[2];
        PersonRecord r1 = new PersonRecord(1, 200, "Alex");
        PersonRecord r2 = new PersonRecord(2, 150, "Lena");
        test.records[0] = r1;
        test.records[1] = r2;
        Debug.Log($"test=<< {test} >>");
        string jsonStr1 = JsonUtility.ToJson(r1);
        Debug.Log($"jsonStr=<< {jsonStr1} >>   rec0={test.records[0]}");
        string jsonStr2 = JsonUtility.ToJson(r2);
        Debug.Log($"jsonStr=<< {jsonStr2} >>   rec1={test.records[1]}");
        string jsonStr = JsonUtility.ToJson(test);
        Debug.Log($"jsonStr=<< {jsonStr} >>");

        string myStr = "[{\"Rank\":1,\"Score\":100,\"Name\":\"Александр Ткаченко\"},{\"Rank\":2,\"Score\":40,\"Name\":\"Мария Ткаченко\"}]";
        PersonRecord[] data = JsonConvert.DeserializeObject<PersonRecord[]>(myStr);
        Debug.Log(data);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
        {
            Debug.Log(data[i]);
            sb.Append($"{data[i]}\n");
        }
        txtDescrLeader.text = sb.ToString();
        panelLiders.SetActive(true);*/
    }

    public void ViewAvatar()
    {
        txtName.text = GameManager.Instance.currentPlayer.playerName;
        riAvatar.texture = GameManager.Instance.currentPlayer.photo;
        Debug.Log($"ViewAvatar => name={GameManager.Instance.currentPlayer.playerName}");
    }


    public void ViewLeaderboard(string strJson)
    {
        if (strJson == "")
        {
            Debug.Log("ViewLeaderboard strJson= <" + strJson + ">");
            for (int i = 0; i < arTxtRecItems.Length; i++) arTxtRecItems[i].text = "";
            return;
        }
        try
        {
            //Debug.Log("ViewLeaderboard => " + strJson);
            //PersonRecord[] data = JsonConvert.DeserializeObject<PersonRecord[]>(strJson);
            //PersonRecord[] data = JsonUtility.FromJson<PersonRecord[]>(strJson);
            PersonRecord[] data = GetDataFromJson(strJson);
            //Debug.Log("data=>" + data);
            //StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length && i < arTxtRecItems.Length; i++)
            {
                arTxtRecItems[i].text = $"{data[i]}";
                //Debug.Log("VL => " + data[i].ToString());
                //sb.Append($"{data[i]}\n");
            }
            //txtDescrLeader.text = sb.ToString();
            //Debug.Log("VL sb=" + sb.ToString());
        }
        catch
        {
            arTxtRecItems[0].text = Language.Instance.CurrentLanguage == "ru" ? "Ошибка" : "Error";
        }
        //panelLiders.SetActive(true);
    }

    private PersonRecord[] GetDataFromJson(string s)
    {
        List<PersonRecord> arr = new List<PersonRecord>();
        string[] ss = s.Split("{");
        for (int i = 1; i < ss.Length; i++)
        {
            int end = ss[i].LastIndexOf('}');
            //Debug.Log($"ss[i]={ss[i]} end={end}");
            string strJson = $"{ss[i].Substring(0, end)}";
            strJson = "{" + strJson + "}";
            //Debug.Log($"strJson={strJson}");
            PersonRecord pr = JsonUtility.FromJson<PersonRecord>(strJson);
            //Debug.Log($"pr={pr}");
            arr.Add(pr);
        }

        return arr.ToArray();
    }
}

[Serializable]
public class MyArrRecords
{
    public PersonRecord[] records { get; set; }
    public MyArrRecords() { }
    public override string ToString()
    {
        return $"Counts={records.Length}";
    }
}

[Serializable]
public class PersonRecord
{
    //public int Rank { get; set; }
    public int Rank;
    //public int Score { get; set; }
    public int Score;
    //public string Name { get; set; }
    public string Name;

    public PersonRecord() { }
    public PersonRecord(int r, int sc, string nm)
    {
        Rank = r;
        Score = sc;
        Name = nm;
    }
    public override string ToString()
    {
        //string nm = String.Format("{0,-25}", Name);
        //return $"{Rank:00} {nm} {Score}";
        return $"{Rank:00} {Name} {Score}";
    }
}

