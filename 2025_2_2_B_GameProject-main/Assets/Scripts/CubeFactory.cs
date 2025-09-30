using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

public class CubeFactory : MonoBehaviour
{
    public GameObject cubePrefab;
    public Transform queuePoint;
    public Transform woodStorage;
    public Transform metalStorage;
    public Transform assemblyArea;

    private Queue<GameObject> materialQueue = new Queue<GameObject>();
    private Stack<GameObject> woodWarehouse = new Stack<GameObject>();
    private Stack<GameObject> metalWarehouse = new Stack<GameObject>();
    private Stack<string> assemblyStack = new Stack<string>();
    private List<WorkRequest> requestsList = new List<WorkRequest>();
    private Dictionary<ProductType, int> products = new Dictionary<ProductType, int>();

    public int money = 500;
    public int score = 0;

    private float lastMaterialTime;
    private float lastOrderTime;

    void AddMaterial()
    {
        ResourceType randomType = (Random.value > 0.5f) ? ResourceType.Wood : ResourceType.Metal;

        GameObject newCube = Instantiate(cubePrefab);
        ResourceCube cubeComponent = newCube.AddComponent<ResourceCube>();
        cubeComponent.Initalize(randomType);

        materialQueue.Enqueue(newCube);
    }

    void ProcessQueue()
    {
        if(materialQueue.Count == 0)
        {
            return;
        }
        GameObject cube = materialQueue.Dequeue();
        ResourceCube resource = cube.GetComponent<ResourceCube>();

        if (resource.type == ResourceType.Wood)
        {
            woodWarehouse.Push(cube);
        }
        else if (resource.type == ResourceType.Metal)
        {
            metalWarehouse.Push(cube);
        }
    }

    void ProcessAssembly()
    {
        if(woodWarehouse.Count == 0 || metalWarehouse.Count == 0)
        {
            return;
        }
        if(assemblyStack.Count == 0)
        {
            return;
        }
        string work = assemblyStack.Pop();

        GameObject wood = woodWarehouse.Pop();
        GameObject metal = metalWarehouse.Pop();    
        Destroy(wood);
        Destroy(metal);

        if(assemblyStack.Count == 0)
        {
            products[ProductType.Chair]++;
            score += 100;

            assemblyStack.Push("포장");
            assemblyStack.Push("조립");
            assemblyStack.Push("준비");
        }
    }

    void AddRequest()
    {
        int quantity = Random.Range(1, 4);
        int reward = quantity * 200;

        WorkRequest newRequest = new WorkRequest(ProductType.Chair, quantity, reward);

        requestsList.Add(newRequest);
    }

    void ProcessRequest()
    {
        if(requestsList.Count == 0)
        {
            return;
        }

        WorkRequest firestRequest = requestsList[0];

        if (products[firestRequest.productType] >= firestRequest.quantity)
        {
            products[firestRequest.productType] -= firestRequest.quantity;
            money += firestRequest.reward;
            score += firestRequest.reward;

            requestsList.RemoveAt(0);
        }
        else
        {
            int available = products[firestRequest.productType];
            int needed = firestRequest.quantity - available;
        }
    }
}
