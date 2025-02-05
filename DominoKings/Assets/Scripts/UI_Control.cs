using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts;

public class UI_Control : MonoBehaviour
{
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private Text txtEndTitle;
    [SerializeField] private Text txtEndDescr;
    [SerializeField] private Text txtEndScorePlayer;
    [SerializeField] private Text txtEndScoreBot;

    [SerializeField] private GameObject hintBuildPanel;

    [SerializeField] private ResPanControl rpcPlayer;
    [SerializeField] private ResPanControl rpcBot;

    [SerializeField] private BuildPanelControl[] arPanels;

    [SerializeField] private GameObject hintPanel;
    [SerializeField] private Text txtHint;

    private Color colWin = new Color(0, 0.6f, 0.2f), colLoss = new Color(0.7f, 0.1f, 0), colDraw = new Color(0.4f, 0.1f, 0.7f);

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
        if (Language.Instance.CurrentLanguage == "ru")
        {
            txtEndScorePlayer.text = $"Вы : {playerScore}";
            txtEndScoreBot.text = $"ИИ : {botScore}";
        }
        else
        {
            txtEndScorePlayer.text = $"You : {playerScore}";
            txtEndScoreBot.text = $"AI : {botScore}";
        }
        txtEndDescr.color = colDraw;

        if (playerScore < botScore)
        {
            txtEndScorePlayer.color = colLoss;
            txtEndScoreBot.color = colWin;
            txtEndTitle.color = colLoss;
            if (Language.Instance.CurrentLanguage == "ru")
            {
                txtEndTitle.text = "Поражение ...";
                txtEndDescr.text = "Соперник оказался сильнее! Но можно сыграть ещё раз!";
            }
            else
            {
                txtEndTitle.text = "Defeat ...";
                txtEndDescr.text = "The opponent was stronger! But you can play it again!";
            }
        }
        else if (playerScore > botScore)
        {
            txtEndScorePlayer.color = colWin;
            txtEndScoreBot.color = colLoss;
            txtEndTitle.color = colWin;
            if (Language.Instance.CurrentLanguage == "ru")
            {
                txtEndTitle.text = "Победа !!!";
                txtEndDescr.text = "ИИ Вас поздравляет, но в другой раз результат будет иной ...";
            }
            else
            {
                txtEndTitle.text = "Victory !!!";
                txtEndDescr.text = "The AI congratulates you, but the result will be different next time...";
            }
        }
        else
        {
            txtEndScorePlayer.color = colDraw;
            txtEndScoreBot.color = colDraw;
            txtEndTitle.color = colDraw;
            if (Language.Instance.CurrentLanguage == "ru")
            {
                txtEndTitle.text = "Ничья !!!";
                txtEndDescr.text = "Странно, но в этот раз ничья! ИИ предлагает сыграть ещё раз, чтобы определить кто сильнее ...";
            }
            else
            {
                txtEndTitle.text = "It's a draw !!!";
                txtEndDescr.text = "Strangely, it's a draw this time! The AI suggests playing again to determine who is stronger...";
            }
        }
        endGamePanel.SetActive(true);
    }

    public void Restart()
    {
        endGamePanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ViewNames(int sc1, int sc2)
    {
        rpcPlayer.ViewName($"{(Language.Instance.CurrentLanguage == "ru" ? "Вы" : "You")} : {sc1}");
        rpcBot.ViewName($"{(Language.Instance.CurrentLanguage == "ru" ? "ИИ" : "AI")} : {sc2}");
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
