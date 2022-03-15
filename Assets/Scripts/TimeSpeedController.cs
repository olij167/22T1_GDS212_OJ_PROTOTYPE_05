using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeSpeedController : MonoBehaviour
{
    public StockMarketPlace stockMarket;

    public Button x5Button, x10Button, x20Button, autoStartNextDayButton;

    TextMeshProUGUI timeSpeedValueText;

    public bool autoStartNextDay;

    private void Start()
    {
        timeSpeedValueText = gameObject.GetComponent<TextMeshProUGUI>();

        timeSpeedValueText.text = "Time Speed: x" + stockMarket.timeSpeed;

        x5Button.onClick.AddListener(delegate { SetTimeSpeed(5); });
        x10Button.onClick.AddListener(delegate { SetTimeSpeed(10); });
        x20Button.onClick.AddListener(delegate { SetTimeSpeed(20); });
    }

    private void Update()
    {
        if (autoStartNextDay)
        {
            AutoStartNextDay();
        }
    }

    void SetTimeSpeed(float timeSpeed)
    {
        stockMarket.timeSpeed = timeSpeed;

        timeSpeedValueText.text = "Time Speed: x" + timeSpeed;
    }

    public void EnableAutoStartDay()
    {
        if (!autoStartNextDay)
        {
            autoStartNextDayButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "True";
            autoStartNextDay = true;
        }
        else
        {
            autoStartNextDayButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "False";
            autoStartNextDay = false;
        }
    }

    void AutoStartNextDay()
    {
        if (stockMarket.marketTimeHours >= 16f && stockMarket.marketTimeMinutes >= 10f)
        {
            stockMarket.NextDay();
            stockMarket.StartDay();
        }
    }
    
}
