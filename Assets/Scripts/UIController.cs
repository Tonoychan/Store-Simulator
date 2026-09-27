using System;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    
    public GameObject updatePricePanel;
    public TMP_Text basePriceText;
    public TMP_Text currentPriceText;
    public TMP_InputField priceInputField;
    
    private StockInformation activeStockInformation;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void OpenUpdatePricePanel(StockInformation stockInformationToUpdate)
    {
        activeStockInformation = stockInformationToUpdate;
        updatePricePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        basePriceText.text = "$"+stockInformationToUpdate.price.ToString("F2");
        currentPriceText.text = "$"+stockInformationToUpdate.currentPrice.ToString("F2");
        priceInputField.text = stockInformationToUpdate.currentPrice.ToString();
    }
    
    public void CloseUpdatePricePanel()
    {
        updatePricePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ApplyPriceUpdate()
    {
        float.TryParse(priceInputField.text, out var newPrice);
        currentPriceText.text = "$"+activeStockInformation.currentPrice.ToString("F2");
        StockInformationController.Instance.UpdatePrice(activeStockInformation.name, newPrice);
        CloseUpdatePricePanel();
    }
}
