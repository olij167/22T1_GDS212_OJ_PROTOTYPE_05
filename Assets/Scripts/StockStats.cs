using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "StockOption")]
public class StockStats : ScriptableObject
{
    public string stockName;

    public float stockPrice, minPriceChangeTimer, maxPriceChangeTimer, stocksOwned, originalMinPriceChangePerSecond, originalMaxPriceChangePerSecond;
    [HideInInspector] public float priceChangeTimer, priceChangePerSecond, minPriceChangePerSecond, maxPriceChangePerSecond;

    public Color stockColor;

    //public UILineRenderer line;

    //public List<int> stockPriceEveryHour = new List<int>();

    public void RandomiseStockTimer()
    {
        priceChangeTimer = Random.Range(minPriceChangeTimer, maxPriceChangeTimer);
    }

    public void RandomiseStockValueChangePerSecond()
    {
        priceChangePerSecond = Random.Range(minPriceChangePerSecond, maxPriceChangePerSecond);
    }

    public float GetOwnedStockValue()
    {
        return stocksOwned * stockPrice;
    }
}
