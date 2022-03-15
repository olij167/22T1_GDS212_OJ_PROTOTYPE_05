using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StockMarketPlace : MonoBehaviour
{
    public List<StockStats> stockOptionsList;
    public List<GameObject> stockUIList, ownedStockUIList;

    public GameObject stockUIPrefab, linePrefab, grid, startDayButton, nextDayButton;

    public float marketTimeHours, marketTimeMinutes, timeSpeed;

    public int dayCount = 1;

    public TextMeshProUGUI timeOfDayText, marketOpenText, trendText, dayCountText;

    public bool marketIsOpen, onlyShowOwnedStock;

    public GraphAnimator graphAnimator;

    public string amPMString, trendString = "Neutral";

    public Color trendIncreaseColour, trendDecreaseColour;

    //public WindowGraph windowGraph;
    void Start()
    {

        dayCountText.text = "Day No. " + dayCount.ToString();

        graphAnimator.lines = new UILineRenderer[stockOptionsList.Count];

        stockUIList = new List<GameObject>();
        ownedStockUIList = new List<GameObject>();

        foreach (StockStats stockOption in stockOptionsList)
        {
            GameObject newStock = Instantiate(stockUIPrefab, transform);

            newStock.GetComponent<StockUI>().stock = stockOption;

            stockOption.RandomiseStockTimer();
            stockOption.RandomiseStockValueChangePerSecond();

            stockUIList.Add(newStock);

            if (newStock.GetComponent<StockUI>().stock.stocksOwned > 0)
            {
                ownedStockUIList.Add(newStock);
            }

            GameObject newLine = Instantiate(linePrefab, grid.transform);

            newLine.GetComponent<UILineRenderer>().grid = grid.GetComponent<UIGridRenderer>();
            //newLine.GetComponent<UILineRenderer>().thickness = 5f;

            newStock.GetComponent<StockUI>().line = newLine.GetComponent<UILineRenderer>();

            newLine.GetComponent<UILineRenderer>().color = newStock.GetComponent<StockUI>().stock.stockColor;

            for (int i = 0; i < stockUIList.Count; i++)
            {
                graphAnimator.lines[i] = stockUIList[i].GetComponent<StockUI>().line;
            }
        }

        graphAnimator.enabled = true;

        GraphStockPercentages();

        DecideTrend();
        trendText.text = "Trend: " + trendString;

        foreach(StockStats stock in stockOptionsList)
        {
            stock.stocksOwned = 0;
        }
    }

    void Update()
    {
        if (marketIsOpen)
        {
            marketOpenText.text = "Market Open";
            marketOpenText.color = trendIncreaseColour;
            marketTimeMinutes += Time.deltaTime * timeSpeed;

            if (marketTimeMinutes >= 59)
            {

                marketTimeHours += 1;
                marketTimeMinutes = 00;

                foreach (GameObject stock in stockUIList)
                {
                    StockUI stockOptionUI = stock.GetComponent<StockUI>();

                    stockOptionUI.CalculatePoint();

                }
                GraphStockPercentages();
            }

            if (marketTimeHours == 16f && marketTimeMinutes >= 10f)
            {
                nextDayButton.SetActive(true);
                marketIsOpen = false;
            }



            foreach (StockStats stockOption in stockOptionsList)
            {
                stockOption.priceChangeTimer -= Time.deltaTime;

                stockOption.stockPrice = stockOption.stockPrice * (1 + stockOption.priceChangePerSecond / 100 * Time.deltaTime);

                if (stockOption.priceChangeTimer <= 0)
                {
                    stockOption.RandomiseStockValueChangePerSecond();
                    stockOption.RandomiseStockTimer();
                }
            }

            //if (marketTimeMinutes == 15f || marketTimeMinutes == 30f || marketTimeMinutes == 45f)
            //{
            //    foreach (GameObject stock in stockUIList)
            //    {
            //        StockUI stockOptionUI = stock.GetComponent<StockUI>();

            //        stockOptionUI.CalculatePoint();

            //    }
            //    GraphStockPercentages();
            //}


        }
        else
        {
            marketOpenText.text = "Market Closed";
            marketOpenText.color = trendDecreaseColour;

        }


        if (marketTimeHours >= 12)
        {
            amPMString = "pm";
        }
        else amPMString = "am";

        timeOfDayText.text = marketTimeHours.ToString("00") + ":" + marketTimeMinutes.ToString("00") + " " + amPMString;

        AddOwnedStocksToList();

       

    }

    public void StartDay()
    {

        marketIsOpen = true;

        graphAnimator.enabled = true;


        GraphStockPercentages();

        startDayButton.SetActive(false);
    }

    public void NextDay()
    {
        foreach (UILineRenderer line in graphAnimator.lines)
        {
            line.points.Clear();
        }

        foreach (GameObject stockOption in stockUIList)
        {
            StockUI stockOptionUI = stockOption.GetComponent<StockUI>();
            stockOptionUI.startDayPrice = stockOption.GetComponent<StockUI>().stock.stockPrice;
        }

        DecideTrend();

        dayCount++;
        marketTimeHours = 10f;
        marketTimeMinutes = 00f;
        amPMString = "am";

        dayCountText.text = "Day No. " + dayCount.ToString();

        trendText.text = "Trend: " + trendString;

        //startDayButton.SetActive(true);
        nextDayButton.SetActive(false);

        StartDay();
    }

    public void AddOwnedStocksToList()
    {
        foreach (GameObject stockUI in stockUIList)
        {
            if (stockUI.GetComponent<StockUI>().stock.stocksOwned > 0 && !ownedStockUIList.Contains(stockUI))
            {
                ownedStockUIList.Add(stockUI);
            }

            if (ownedStockUIList.Contains(stockUI) && stockUI.GetComponent<StockUI>().stock.stocksOwned <= 0)
            {
                ownedStockUIList.Remove(stockUI);
            }

        }
    }

    public void ToggleShowOwnedStock()
    {
        if (!onlyShowOwnedStock)
        {
            foreach (GameObject stockUI in stockUIList)
            {
                if (!ownedStockUIList.Contains(stockUI))
                {
                    stockUI.SetActive(false);
                }
            }

            onlyShowOwnedStock = true;
        }
        else
        {
            foreach (GameObject stockUI in stockUIList)
            {
                stockUI.SetActive(true);
            }

            onlyShowOwnedStock = false;
        }
    }

    public void GraphStockPercentages()
    {
        foreach (GameObject stockOption in stockUIList)
        {
            StockUI stockOptionUI = stockOption.GetComponent<StockUI>();

           // Vector2 point = new Vector2((marketTimeHours - 9) * grid.GetComponent<UIGridRenderer>().gridSize.x, (stockOptionUI.stockChangePercentage * 100) / grid.GetComponent<RectTransform>().rect.height);//  grid.GetComponent<UIGridRenderer>().gridSize.y));  / grid.GetComponent<RectTransform>().rect.width


            Vector2 point = new Vector2((marketTimeHours - 10) * grid.GetComponent<RectTransform>().rect.width / grid.GetComponent<UIGridRenderer>().gridSize.x, // x axis
                stockOptionUI.stockChangePercentage * grid.GetComponent<RectTransform>().rect.height / grid.GetComponent<UIGridRenderer>().gridSize.y); // y axis

            stockOptionUI.line.points.Add(point);

            graphAnimator.Animate(stockOptionUI.line, stockOptionUI.line.points);
        }
    }

    public void DecideTrend()
    {
        int randTrend = Random.Range(0, 3);

        switch (randTrend)
        {
            case 0:
                {
                    trendString = "Decrease";
                    trendText.color = trendDecreaseColour;
                    break;
                }
            case 1:
                {
                    trendString = "Neutral";
                    trendText.color = Color.white;
                    break;
                }
            case 2:
                {
                    trendString = "Increase";
                    trendText.color = trendIncreaseColour;
                    break;
                }
            case 3:
                {
                    trendText.color = Color.white;
                    return;
                }
        }

        ApplyTrendAffects();
    }

    public void ApplyTrendAffects()
    {
        float originalMinPriceChange = stockUIList[0].GetComponent<StockUI>().stock.minPriceChangePerSecond;
        float originalMaxPriceChange = stockUIList[0].GetComponent<StockUI>().stock.maxPriceChangePerSecond;

        switch (trendString)
        {
            case "Bear":
                {
                    foreach (GameObject stockUI in stockUIList)
                    {
                        stockUI.GetComponent<StockUI>().stock.maxPriceChangePerSecond = stockUI.GetComponent<StockUI>().stock.originalMaxPriceChangePerSecond / 2;
                        stockUI.GetComponent<StockUI>().stock.minPriceChangePerSecond = stockUI.GetComponent<StockUI>().stock.originalMaxPriceChangePerSecond;
                        
                    }
                    break;
                }

            case "Bull":
                {
                    foreach (GameObject stockUI in stockUIList)
                    {
                        stockUI.GetComponent<StockUI>().stock.minPriceChangePerSecond = stockUI.GetComponent<StockUI>().stock.minPriceChangePerSecond / 2;
                        stockUI.GetComponent<StockUI>().stock.maxPriceChangePerSecond = stockUI.GetComponent<StockUI>().stock.originalMaxPriceChangePerSecond;
                        
                    }
                    break;
                }

            case "Neutral":
                {
                    foreach (GameObject stockUI in stockUIList)
                    {
                        stockUI.GetComponent<StockUI>().stock.minPriceChangePerSecond = stockUI.GetComponent<StockUI>().stock.originalMinPriceChangePerSecond;
                        stockUI.GetComponent<StockUI>().stock.maxPriceChangePerSecond = stockUI.GetComponent<StockUI>().stock.originalMaxPriceChangePerSecond;

                    }
                    break;
                }
        }
    }
}
