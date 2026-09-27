using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShelfSpaceController : MonoBehaviour
{
    public StockInformation stockInformation;
    public List<StockObject> objectOnShelf;
    public List<Transform> bigDrinkBottlePlacementTransforms;
    public List<Transform> cerealPlacementTransforms;
    public List<Transform> chipsPlacementTransforms;
    public List<Transform> vegetablePlacementTransforms;
    public List<Transform> fruitPlacementTransforms;
    public List<Transform> fruitHalfPlacementTransforms;

    public TMP_Text shelfPriceLabel;
        

    public void PlaceItemOnStock(StockObject objectToPlace)
    {
        //Optimized Version
        /*
        if (objectOnShelf.Count > 0)
        {
            if (!string.Equals(stockInformation.name, objectToPlace.stockInformation.name))
            {
                return;
            }
        }

        stockInformation = objectToPlace.stockInformation;
        objectToPlace.transform.SetParent(transform);
        objectToPlace.PlaceItem();
        objectOnShelf.Add(objectToPlace);
        */

        bool canPlace = false;
        Transform tempTransformToPlaceInShelf = null;

        if (objectOnShelf.Count == 0)
        {
            stockInformation = objectToPlace.stockInformation;
            canPlace = true;
        }
        else
        {
            if (string.Equals(stockInformation.name, objectToPlace.stockInformation.name))
            {
                switch (stockInformation.typeOfStock)
                {
                    case StockInformation.StockType.Drink:
                        if (objectOnShelf.Count < bigDrinkBottlePlacementTransforms.Count)
                        {
                            canPlace = true;
                        }

                        break;
                    case StockInformation.StockType.Cereal:
                        if (objectOnShelf.Count < cerealPlacementTransforms.Count)
                        {
                            canPlace = true;
                        }

                        break;
                    case StockInformation.StockType.Chips:
                        if (objectOnShelf.Count < chipsPlacementTransforms.Count)
                        {
                            canPlace = true;
                        }

                        break;
                    case StockInformation.StockType.Vegetable:
                        if (objectOnShelf.Count < vegetablePlacementTransforms.Count)
                        {
                            canPlace = true;
                        }

                        break;
                    case StockInformation.StockType.FruitWhole:
                        if (objectOnShelf.Count < fruitPlacementTransforms.Count)
                        {
                            canPlace = true;
                        }

                        break;
                    case StockInformation.StockType.FruitCut:
                        if (objectOnShelf.Count < fruitHalfPlacementTransforms.Count)
                        {
                            canPlace = true;
                        }
                        break;
                }
            }
        }

        if (canPlace)
        {
            switch (stockInformation.typeOfStock)
            {
                case StockInformation.StockType.Drink:
                    objectToPlace.transform.SetParent(bigDrinkBottlePlacementTransforms[objectOnShelf.Count]);
                    break;
                case StockInformation.StockType.Cereal:
                    objectToPlace.transform.SetParent(cerealPlacementTransforms[objectOnShelf.Count]);
                    break;
                case StockInformation.StockType.Chips:
                    objectToPlace.transform.SetParent(chipsPlacementTransforms[objectOnShelf.Count]);
                    break;
                case StockInformation.StockType.Vegetable:
                    objectToPlace.transform.SetParent(vegetablePlacementTransforms[objectOnShelf.Count]);
                    break;
                case StockInformation.StockType.FruitWhole:
                    objectToPlace.transform.SetParent(fruitPlacementTransforms[objectOnShelf.Count]);
                    break;
                case StockInformation.StockType.FruitCut:
                    objectToPlace.transform.SetParent(fruitHalfPlacementTransforms[objectOnShelf.Count]);
                    break;
            }
            objectToPlace.PlaceItem();
            objectOnShelf.Add(objectToPlace);
            
            shelfPriceLabel.text = "$"+objectOnShelf[0].stockInformation.price;
        }
    }

    public StockObject GetItemFromStock()
    {
        StockObject objectToReturn = null;
        if (objectOnShelf.Count > 0)
        {
            objectToReturn = objectOnShelf[objectOnShelf.Count - 1];
            objectOnShelf.RemoveAt(objectOnShelf.Count - 1);
        }
        if(objectOnShelf.Count == 0)
        {
            shelfPriceLabel.text = "$0";
        }

        return objectToReturn;
    }
}