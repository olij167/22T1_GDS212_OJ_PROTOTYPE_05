using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StockUI : MonoBehaviour
{
    public StockStats stock;

    public TextMeshProUGUI stockNameUI, stockPriceUI, stocksOwnedUI, lastPurchasePriceText;

    private PlayerMoney playerMoney;

    //public List<Vector2> stockPriceEveryHour;

    public float stockChangePercentage, startDayPrice, lastPurchasePrice;

    public GameObject buyButton, sellButton;

    public Image priceChangeDisplayArrow;
    public Sprite upArrowSprite, downArrowSprite, dashSprite;

    [HideInInspector] public UILineRenderer line;

    [HideInInspector] public StockMarketPlace stockMarket;

    public AudioClip buySound, sellSound;

    AudioSource audioSource;

    void Start()
    {

        audioSource = GetComponent<AudioSource>();

        stockMarket = GameObject.FindGameObjectWithTag("StockMarket").GetComponent<StockMarketPlace>();

        //stockPriceEveryHour = new List<Vector2>();
        startDayPrice = stock.stockPrice;
        playerMoney = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoney>();
        stockNameUI.text = stock.stockName; // + " Stock"

        stockNameUI.color = stock.stockColor;
        //stockPriceUI.color = stock.stockColor;
        //stocksOwnedUI.color = stock.stockColor;

        //buyButton.transform.GetComponentInChildren<TextMeshProUGUI>().color = stock.stockColor;
        //sellButton.transform.GetComponentInChildren<TextMeshProUGUI>().color = stock.stockColor;
        
        buyButton.GetComponent<Image>().color = stock.stockColor;
        sellButton.GetComponent<Image>().color = stock.stockColor;
    }

    void Update()
    {
        stockPriceUI.text = "Price: $" + stock.stockPrice.ToString(".00");
        stocksOwnedUI.text = "Owned: " + stock.stocksOwned.ToString();

        if (stock.stocksOwned >= 1f)
        {
            
            lastPurchasePriceText.text = "Bought: " + "\n" + "$" + lastPurchasePrice.ToString(".00");
            //priceChangeDisplayArrow.enabled = true;
            
        }
        else
        {
            //priceChangeDisplayArrow.enabled = false;
            //priceChangeDisplayArrow.sprite = dashSprite;
            //priceChangeDisplayArrow.color = Color.white;
            lastPurchasePriceText.text = "Bought:" + "\n" + "N/A";
            
        }

        ChoosePriceChangeArrow();


    }

    //public void SetPointsOnGraph()
    //{
    //    for (int i = 0; i < stockPriceEveryHour.Count; i++)
    //    {
    //        line.points.Add(stockPriceEveryHour[i]);

    //        //if (!line.points.Contains(stockPriceEveryHour[i]))
    //        //{
                
    //        //}
    //    }
    //}


    public void BuyStocks()
    {
        if (playerMoney.money >= stock.stockPrice && stockMarket.marketIsOpen)
        {
            playerMoney.money -= stock.stockPrice;
            lastPurchasePrice = stock.stockPrice;
            //startDayPrice = stock.stockPrice;

            audioSource.PlayOneShot(buySound);

            stock.stocksOwned++;
            
        }
    }

    public void SellStocks()
    {
        if (stock.stocksOwned > 0 && stockMarket.marketIsOpen)
        {
            playerMoney.money += stock.stockPrice;

            audioSource.PlayOneShot(sellSound);

            stock.stocksOwned--;
        }
    }

    public void AddStockToPortfolio()
    {
        if (!playerMoney.stockMarket.stockOptionsList.Contains(stock))
        {
            playerMoney.stockMarket.stockOptionsList.Add(stock);
            playerMoney.stocksOwnedValueList.Add(stock.GetOwnedStockValue());
        }
    }

    public void RemoveStockFromPortfolio()
    {
        if (playerMoney.stocksOwnedValueList.Contains(stock.GetOwnedStockValue()) && stock.GetOwnedStockValue() <= 0)
        {
            playerMoney.stockMarket.stockOptionsList.Remove(stock);
            playerMoney.stocksOwnedValueList.Remove(stock.GetOwnedStockValue());
        }
    }

    public void CalculatePoint()
    {
        if (line.points.Count > 0)
        {

            stockChangePercentage = (stock.stockPrice - startDayPrice) / startDayPrice * 100;
            //line.points[line.points.Count - 1].y / line.points[0].y;

        }
    }

    public void ChoosePriceChangeArrow()
    {
        

        if (Mathf.Round(stock.priceChangePerSecond * 100f) * 0.01f > 0f)
        {
            //priceChangeDisplayArrow.enabled = true;
            priceChangeDisplayArrow.sprite = upArrowSprite;
            priceChangeDisplayArrow.color = stockMarket.trendIncreaseColour;
        }

        if (Mathf.Round(stock.priceChangePerSecond * 100f) * 0.01f == 0f)
        {
            //priceChangeDisplayArrow.enabled = true;
            priceChangeDisplayArrow.sprite = dashSprite;
            priceChangeDisplayArrow.color = Color.white;
        }

        if (Mathf.Round(stock.priceChangePerSecond * 100f) * 0.01f < 0f)
        {
            //priceChangeDisplayArrow.enabled = true;
            priceChangeDisplayArrow.sprite = downArrowSprite;
            priceChangeDisplayArrow.color = stockMarket.trendDecreaseColour;
        } 
        
        //if (Mathf.Round(stock.stockPrice * 100f) * 0.01f > Mathf.Round(lastPurchasePrice * 100f) * 0.01f)
        //{
        //    //priceChangeDisplayArrow.enabled = true;
        //    priceChangeDisplayArrow.sprite = upArrowSprite;
        //    priceChangeDisplayArrow.color = stockMarket.trendIncreaseColour;
        //}

        //if (Mathf.Round(stock.stockPrice * 100f) * 0.01f == Mathf.Round(lastPurchasePrice * 100f) * 0.01f)
        //{
        //    //priceChangeDisplayArrow.enabled = true;
        //    priceChangeDisplayArrow.sprite = dashSprite;
        //    priceChangeDisplayArrow.color = Color.white;
        //}

        //if (Mathf.Round(stock.stockPrice * 100f) * 0.01f < Mathf.Round(lastPurchasePrice * 100f) * 0.01f)
        //{
        //    //priceChangeDisplayArrow.enabled = true;
        //    priceChangeDisplayArrow.sprite = downArrowSprite;
        //    priceChangeDisplayArrow.color = stockMarket.trendDecreaseColour;
        //}
    }
}
