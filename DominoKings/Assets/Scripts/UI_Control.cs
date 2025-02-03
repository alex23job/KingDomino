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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ViewHintBuildPanel()
    {
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

    public void ViewResPlayer(ResourseSet set)
    {
        rpcPlayer.ViewAllResorses(set);
    }

    public void ViewResBot(ResourseSet set)
    {
        rpcBot.ViewAllResorses(set);
    }
}
