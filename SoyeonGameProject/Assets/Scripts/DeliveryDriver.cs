using UnityEngine.Events;
using UnityEngine;
using UnityEditor.Rendering;

public class DeliveryDriver : MonoBehaviour
{
    [Header("��٤����ͤ� �����ä� ")]
    public float moveSpeed = 8f;
    public float rotationSpeed = 10.0f;

    [Header("�󤼤�")]
    public float currentMoney = 0f;
    public float batteryLevel = 100f;
    public int deliveryCount = 0;

    [System.Serializable]
    public class DriverEvents                               //�پߤ������� �̺������� ������ Ŭ�������� �����ä� 
    {
        [Header("�̵��� Event")]
        public UnityEvent OnMoveStarted;
        public UnityEvent OnMoveStoped;

        [Header("�󤼤����Ť�ȭ  Event")]
        public UnityEvent<float> OnMoneyChanged;
        public UnityEvent<float> OnBatteryChanged;
        public UnityEvent<int> OnDeliveryCountChanged;

        [Header("�椡�� Event")]
        public UnityEvent OnLowBattery;
        public UnityEvent OnLowBatteryEmpty;
        public UnityEvent OnDeliveryCompleted;

    }

    public DriverEvents driverEvents;

    public bool isMoving = false;


    // Start is called before the first frame update
    void Start()
    {
        //�ʱ� �󤼤� Event �߻���
        driverEvents.OnMoneyChanged?.Invoke(currentMoney);
        driverEvents.OnBatteryChanged?.Invoke(batteryLevel);
        driverEvents.OnDeliveryCountChanged?.Invoke(deliveryCount);
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        UpdateBattery();
    }

    void HandleMovement()
    {
        if(batteryLevel <= 0)
        {
            if(isMoving)
            {

            }
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);
        
        if(moveDirection.magnitude > 0.1f)
        {
            if(!isMoving)
            {
               StartMoving();

            }

            //�̵� ó�� 
            moveDirection = moveDirection.normalized;
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime , Space.World);

            //ȸ��ó��
            if(moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            ChangeBattery(-Time.deltaTime * 3.0f);  //�̵� �� �� ���� ���͸� �Ҹ� 
        }
        else
        {
            if(isMoving)
            {
                StopMoving();
            }
        }
    }

    void UpdateBattery()
    {
        if (batteryLevel > 0)
        {
            ChangeBattery(-Time.deltaTime * 0.5f);
        }
    }

    public void AddMoney(float amount)
    {
        currentMoney += amount;
        driverEvents.OnMoneyChanged?.Invoke(currentMoney);
    }

    public void CompleteDelivery()
    {
        deliveryCount++;
        float reward = Random.Range(3000, 8000);

        AddMoney(reward);
        driverEvents.OnDeliveryCountChanged?.Invoke(deliveryCount);
        driverEvents.OnDeliveryCompleted?.Invoke();
    }

    public void ChargeBattery()
    {
        ChangeBattery(100f - batteryLevel);
    }

    public string GetStautusText()
    {
        return $"�� : {currentMoney:F0} �� | ���͸� : {batteryLevel:F1}% | ��� : {deliveryCount} ��";
    }

    public bool CanMove()
    {
        return batteryLevel > 0;
    }

    void ChangeBattery(float amount)
    {
        float oldBattery = batteryLevel;
        batteryLevel += amount;
        batteryLevel = Mathf.Clamp(batteryLevel, 0, 100);

        //���͸� ��ȭ Event �߻�
        driverEvents.OnBatteryChanged.Invoke(batteryLevel);

        //���͸� ���¿� ���� ���
        if(oldBattery > 20f && batteryLevel <= 20f)
        {
            driverEvents.OnLowBattery?.Invoke();        //���͸� ���� ����
        }

        if (oldBattery > 0f && batteryLevel <= 0f)
        {
            driverEvents.OnLowBatteryEmpty?.Invoke();        //���͸� ����
        }
    }

    void StartMoving()
    {
        isMoving = true;
        driverEvents.OnMoveStarted?.Invoke();
    }

    void StopMoving()
    {
        isMoving = false;
        driverEvents.OnMoveStoped?.Invoke();
    }
}
