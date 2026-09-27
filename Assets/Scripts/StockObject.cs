using System;
using UnityEngine;

public class StockObject : MonoBehaviour
{
    public StockInformation stockInformation;
    public float moveSpeed;

    [SerializeField]
    public bool isPlaced;
    [SerializeField]
    public Rigidbody rigidbody;
    [SerializeField]
    public Collider collider;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>(); 
        collider = GetComponent<Collider>();
        stockInformation = StockInformationController.Instance.GetStockInformation(stockInformation.name);
    }

    private void Update()
    {
        if (isPlaced)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, moveSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, moveSpeed * Time.deltaTime);   
        }
    }

    public void PickUp()
    {
        transform.localPosition = new Vector3(0.2f,0f,0f);
        transform.localRotation = Quaternion.identity;
        rigidbody.isKinematic = true;
        isPlaced = false;
        collider.enabled = false;
    }

    public void PlaceItem()
    {
        rigidbody.isKinematic = true;
        isPlaced = true;
        collider.enabled = false;
    }
    
    public void ReleaseItem()
    {
        rigidbody.isKinematic = false;
        collider.enabled = true;
    }
}
