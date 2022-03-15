using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class PlayerMoney : MonoBehaviour
{
    public float money, netWorth, totalValueOfStocksOwned, currentNetWorth, goalValue;

    public StockMarketPlace stockMarket;

    public List<float> stocksOwnedValueList;

    public TextMeshProUGUI moneyUI, netWorthUI, goalText;

    public Image netWorthArrow;

    public Sprite upArrowSprite, downArrowSprite;

    private void Start()
    {

        foreach (StockStats stockOption in stockMarket.stockOptionsList)
        {
            stocksOwnedValueList.Add(stockOption.GetOwnedStockValue());
        }
    }

    private void Update()
    {
        currentNetWorth = netWorth;

        for (int i = 0; i < stocksOwnedValueList.Count; i++)
        {
            stocksOwnedValueList[i] = stockMarket.stockOptionsList[i].GetOwnedStockValue(); 
            
            totalValueOfStocksOwned = stocksOwnedValueList.Sum();
        }

        //Debug.Log("total value of stocks = " + totalValueOfStocksOwned);

        netWorth = totalValueOfStocksOwned + money;


        moneyUI.text = "Money: $" + money.ToString(".00");
        
        netWorthUI.text = "Net Worth: $" + netWorth.ToString(".00");

        SetNetWorthColour();

        goalText.text = "Goal: $" + goalValue.ToString() + " Net Worth" + "\n" + "Remaining: $" + (goalValue - netWorth).ToString(".00");

        
    }

    public void SetNetWorthColour()
    {
        if (currentNetWorth < netWorth)
        {
            //netWorthUI.color = stockMarket.trendIncreaseColour;
            netWorthArrow.gameObject.SetActive(true);
            netWorthArrow.sprite = upArrowSprite;
            netWorthArrow.color = stockMarket.trendIncreaseColour;
        }

        if (currentNetWorth == netWorth)
        {
            //netWorthUI.color = Color.white;
            netWorthArrow.gameObject.SetActive(false);
        }

        if (currentNetWorth > netWorth)
        {
            //netWorthUI.color = stockMarket.trendDecreaseColour;
            netWorthArrow.gameObject.SetActive(true);
            netWorthArrow.sprite = downArrowSprite;
            netWorthArrow.color = stockMarket.trendDecreaseColour;
        }
    }


}
