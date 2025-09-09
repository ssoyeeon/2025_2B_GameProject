using UnityEngine.Events;
using UnityEngine;
using UnityEditor.Rendering;

public class DeliveryDriver : MonoBehaviour
{
    [Header("배다ㄹㅇㅝㄴ 설ㅈㅓㅇ ")]
    public float moveSpeed = 8f;
    public float rotationSpeed = 10.0f;

    [Header("상ㅌㅐ")]
    public float currentMoney = 0f;
    public float batteryLevel = 100f;
    public int deliveryCount = 0;

    [System.Serializable]
    public class DriverEvents                               //다야ㅇㅎㅏㄴ 이베ㄴㅌㅡ 정ㅇㅢ 클ㄹㅐㅅㅡ 선ㅇㅓㄴ 
    {
        [Header("이도ㅇ Event")]
        public UnityEvent OnMoveStarted;
        public UnityEvent OnMoveStoped;

        [Header("상ㅌㅐㅂㅕㄴ화  Event")]
        public UnityEvent<float> OnMoneyChanged;
        public UnityEvent<float> OnBatteryChanged;
        public UnityEvent<int> OnDeliveryCountChanged;

        [Header("경ㄱㅗ Event")]
        public UnityEvent OnLowBattery;
        public UnityEvent OnLowBatteryEmpty;
        public UnityEvent OnDeliveryCompleted;

    }

    public DriverEvents driverEvents;

    public bool isMoving = false;


    // Start is called before the first frame update
    void Start()
    {
        //초기 상ㅌㅐ Event 발새ㅇ
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

            //이동 처리 
            moveDirection = moveDirection.normalized;
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime , Space.World);

            //회전처리
            if(moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            ChangeBattery(-Time.deltaTime * 3.0f);  //이동 할 때 마다 배터리 소모 
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
        return $"돈 : {currentMoney:F0} 원 | 배터리 : {batteryLevel:F1}% | 배달 : {deliveryCount} 건";
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

        //배터리 변화 Event 발생
        driverEvents.OnBatteryChanged.Invoke(batteryLevel);

        //배터리 상태에 따른 경고
        if(oldBattery > 20f && batteryLevel <= 20f)
        {
            driverEvents.OnLowBattery?.Invoke();        //배터리 부족 상태
        }

        if (oldBattery > 0f && batteryLevel <= 0f)
        {
            driverEvents.OnLowBatteryEmpty?.Invoke();        //배터리 방전
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
