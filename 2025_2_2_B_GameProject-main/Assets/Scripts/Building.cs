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

    // Start is called before the first frame update
    void Start()
    {
        SetupBuilding();
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
                    buildingName = "������";
                    break;

                case BuildingType.Customer:
                    mat.color = Color.green;
                    buildingName = "�� ��";
                    break;

                case BuildingType.ChargingStation:
                    mat.color = Color.yellow;
                    buildingName = "������";
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

    void HandleDriverService(DeliveryDriver driver)
    {
        switch (BuildingType)
        {
            case BuildingType.Restaurant:
                Debug.Log($"{buildingName} ���� ������ �Ⱦ� �߽��ϴ�.");
                break;

            case BuildingType.Customer:
                Debug.Log($"{buildingName} ���� ��� �Ϸ�");
                driver.CompleteDelivery();
                break;

            case BuildingType.ChargingStation:
                Debug.Log($"{buildingName} ���� ���͸��� ���� �߽��ϴ�. ");
                driver.ChargeBattery();
                break;
        }
    }
}
