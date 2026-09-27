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
}
