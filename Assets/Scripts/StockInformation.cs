using UnityEngine;

[System.Serializable]
public class StockInformation
{
   public string name;
   public enum StockType
   {
      Cereal,
      Drink,
      Chips,
      FruitWhole,
      FruitCut,
      Vegetable
   }
   public StockType typeOfStock;
   
   public float price;
   public float currentPrice;
   public StockObject theStockObject;

}
