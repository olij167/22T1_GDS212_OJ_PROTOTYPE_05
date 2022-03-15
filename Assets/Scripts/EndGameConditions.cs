using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGameConditions : MonoBehaviour
{
    public PlayerMoney player;

    public StockMarketPlace stockMarket;

    public GameObject goalReachedPanel, bankruptPanel, wantToQuitPanel;

    public TextMeshProUGUI winDaysTakenText, loseDaysTakenText;
    void Start()
    {
        goalReachedPanel.SetActive(false);
        bankruptPanel.SetActive(false);
    }

    void Update()
    {
        if (player.netWorth >= player.goalValue)
        {
            GoalReached();
        }

        if (player.netWorth <= 0f && stockMarket.dayCount >= 1 && stockMarket.marketIsOpen)
        {
            Bankrupt();
        }
    }

    public void GoalReached()
    {
        Time.timeScale = 0f;
        //stockMarket.marketIsOpen = false;
        goalReachedPanel.SetActive(true);

        if (stockMarket.dayCount > 1)
        {
            winDaysTakenText.text = "and" + "\n" + "It only took you " + stockMarket.dayCount.ToString() + " days";
        }
        else winDaysTakenText.text = "and" + "\n" + "It only took you " + stockMarket.dayCount.ToString() + " day";

    }

    public void Bankrupt()
    {
        Time.timeScale = 0f;
        //stockMarket.marketIsOpen = false;
        bankruptPanel.SetActive(true);

        if (stockMarket.dayCount > 1)
        {
            loseDaysTakenText.text = "and" + "\n" + "It only took you " + stockMarket.dayCount.ToString() + " days";
        }
        else loseDaysTakenText.text = "and" + "\n" + "It only took you " + stockMarket.dayCount.ToString() + " day";
    }

    public void Continue()
    {
        goalReachedPanel.SetActive(false);
        player.goalValue *= 2;

        //stockMarket.marketIsOpen = true;
        Time.timeScale = 1f;
    }

    public void Restart(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void CheckBeforeQuitting()
    {
        Time.timeScale = 0f;

        wantToQuitPanel.SetActive(true);
    }

    public void CancelQuit()
    {
        Time.timeScale = 1f;

        wantToQuitPanel.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
