using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Type;

public class WorkRequest
{
    public ProductType productType;
    public int quantity;
    public int reward;

    public WorkRequest(ProductType productType, int quantity, int reward)
    {
        this.productType = productType;
        this.quantity = quantity;
        this.reward = reward;
    }
}
