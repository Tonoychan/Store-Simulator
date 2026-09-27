using System.Collections.Generic;
using UnityEngine;

public class StockInformationController : MonoBehaviour
{
    public static StockInformationController Instance;
    
    public List<StockInformation> foodInformation, produceInformation;
    
    private List<StockInformation> allStockInformation = new List<StockInformation>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        
        allStockInformation.AddRange(foodInformation);
        allStockInformation.AddRange(produceInformation);
        
        foreach (var info in allStockInformation)
        {
            if (info.currentPrice.Equals(0f))
            {
                info.currentPrice = info.price;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public StockInformation GetStockInformation(string stockName)
    {
        StockInformation informationToReturn = null;
        foreach (var info in allStockInformation)
        {
            if (info.name == stockName)
            {
                informationToReturn = info;
                break;
            }
        }
        return informationToReturn;
    }
    
    public void UpdatePrice(string stockName, float newPrice)
    {
        foreach (var info in allStockInformation)
        {
            if (info.name == stockName)
            {
                info.currentPrice = newPrice;
                break;
            }
        }
        
        List<ShelfSpaceController> shelves = new List<ShelfSpaceController>();
        shelves.AddRange(FindObjectsByType<ShelfSpaceController>());
        foreach (var shelf in shelves)
        {
            if (shelf.stockInformation.name == stockName)
            {
                shelf.UpdateDisplayPrice(newPrice);
            }
        }
    }
}
