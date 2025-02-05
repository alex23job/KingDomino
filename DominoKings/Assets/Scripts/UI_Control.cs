using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts;

public class UI_Control : MonoBehaviour
{
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private GameObject hintBuildPanel;

    [SerializeField] private ResPanControl rpcPlayer;
    [SerializeField] private ResPanControl rpcBot;

    [SerializeField] private BuildPanelControl[] arPanels;

    [SerializeField] private GameObject hintPanel;
    [SerializeField] private Text txtHint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ViewHintBuildPanel(int landID, ResourseSet set, string nm1, string nm2)
    {
        arPanels[0].SetParams(1, landID, set, nm1);
        arPanels[1].SetParams(2, landID, set, nm2);
        hintBuildPanel.SetActive(true);
    }

    public void ViewEndGamePanel(int playerScore, int botScore)
    {
        endGamePanel.SetActive(true);
    }

    public void Restart()
    {
        endGamePanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ViewNames(int sc1, int sc2)
    {
        rpcPlayer.ViewName($"{(Language.Instance.CurrentLanguage == "ru" ? "Âû" : "You")} : {sc1}");
        rpcBot.ViewName($"{(Language.Instance.CurrentLanguage == "ru" ? "ÀÈ" : "AI")} : {sc2}");
    }

    public void ViewResPlayer(ResourseSet set)
    {
        rpcPlayer.ViewAllResorses(set);
    }

    public void ViewResBot(ResourseSet set)
    {
        rpcBot.ViewAllResorses(set);
    }

    public void ViewMsgHint(string msg)
    {
        txtHint.text = msg;
        hintPanel.SetActive(true);
    }
}
