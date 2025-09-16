using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Building : MonoBehaviour
{
    [Header("�ǹ� ����")]
    public BuildingType BuildingType;
    public string buildingName = "�ǹ�";

    [System.Serializable]
    public class BuildingEvents
    {
        public UnityEvent<string> OnDriverEntered;
        public UnityEvent<string> OnDriverExited;
        public UnityEvent<BuildingType> OnServiceUsed;
    }

    public BuildingEvents buildingEvents;

    private DeliveryOrderSystem orderSystem;

    // Start is called before the first frame update
    void Start()
    {
        SetupBuilding();
        orderSystem = FindObjectOfType<DeliveryOrderSystem>();
        CreateNameTag();
    }
    void SetupBuilding()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Material mat = renderer.material;
            switch (BuildingType)
            {
                case BuildingType.Restaurant:
                    mat.color = Color.red;
                    break;

                case BuildingType.Customer:
                    mat.color = Color.green;
                    break;

                case BuildingType.ChargingStation:
                    mat.color = Color.yellow;
                    break;
            }
        }
        Collider col = GetComponent<Collider>();
        if (col != null) { col.isTrigger = true; }        
    }

    void OnTriggerEnter(Collider other)
    {
        DeliveryDriver driver = other.GetComponent<DeliveryDriver>();   
        if( driver != null)
        {
            buildingEvents.OnDriverEntered?.Invoke(buildingName);
            HandleDriverService(driver);
        }
    }

    void OnTriggerExit(Collider other)
    {
        DeliveryDriver driver = other.GetComponent<DeliveryDriver>();
        if (driver != null)
        {
            buildingEvents.OnDriverExited?.Invoke(buildingName);
            Debug.Log($"{buildingName} �� �������ϴ�.");
        }
    }

    void CreateNameTag()
    {
        //�ǹ� ���� �̸�ǥ ����
        GameObject nameTag = new GameObject("NameTag");
        nameTag.transform.SetParent(transform);
        nameTag.transform.localPosition = Vector3.up * 1.5f;
        
        TextMesh textMesh = nameTag.AddComponent<TextMesh>();
        textMesh.text = buildingName;
        textMesh.characterSize = 0.2f;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.color = Color.white;
        textMesh.fontSize = 20;

        nameTag.AddComponent<Bildboard>();
    }

    void HandleDriverService(DeliveryDriver driver)
    {
        switch (BuildingType)
        {
            case BuildingType.Restaurant:
                if (orderSystem != null)
                    orderSystem.OnDriverEnteredRestaurant(this);
                break;

            case BuildingType.Customer:
                if (orderSystem != null) orderSystem.OnDriverEnteredCustom(this);
                driver.CompleteDelivery();
                break;

            case BuildingType.ChargingStation:
                driver.ChargeBattery();
                break;
        }
        buildingEvents.OnServiceUsed?.Invoke(BuildingType);
    }
}
