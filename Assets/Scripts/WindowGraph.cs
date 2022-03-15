using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    public StockMarketPlace stockMarket;
    private RectTransform graphContainer;

    [SerializeField] private Sprite circleSprite;

    private void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
    }

    //private void Update()
    //{
    //    if (stockMarket.marketIsOpen)
    //    {
            
    //        foreach (GameObject stockOption in stockMarket.ownedStockUIList)
    //        {
    //            StockStats stockOptionStats = stockOption.GetComponent<StockStats>();
    //            List<int> valueList = new List<int>();

    //            for (int i = 0; i < stockOptionStats.stockPriceEveryHour.Count; i++)
    //            {
                    

    //                valueList.Add((int)stockMarket.marketTimeHours);
    //                valueList.Add((int)stockOptionStats.stockPriceEveryHour[i]);

    //                ShowGraph(valueList);
    //            }
    //        }
    //    }
    //}

    public GameObject CreateCircle(Vector2 anchorchedPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchorchedPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    public void ShowGraph(List<int> valueList)
    {
        float xSize = 50f;
        float yMax = 1000f;
        float graphHeight = graphContainer.sizeDelta.y;

        GameObject lastCircleGameObject = null;

        for (int i = 0; i < valueList.Count; i++)
        {
            float xPos = i * xSize;
            float yPos = (valueList[i] / yMax) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPos, yPos));

            if (lastCircleGameObject != null)
            {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }

            lastCircleGameObject = circleGameObject;
        }
    }

    private void CreateDotConnection(Vector2 dotPosA, Vector2 dotPosB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPosB - dotPosA).normalized;

        float distance = Vector2.Distance(dotPosA, dotPosB); 

        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(100f, 3f);
        rectTransform.anchoredPosition = dotPosA + dir * distance * .5f;

        rectTransform.localEulerAngles = new Vector3(0, 0, (Mathf.Atan2(dir.y, dir.x) * 180 / Mathf.PI)); // from tutorial comment section
    }
}
