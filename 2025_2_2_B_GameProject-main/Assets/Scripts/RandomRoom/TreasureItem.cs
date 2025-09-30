using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureItem : MonoBehaviour
{
    public int goldAmount = 100;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log($"²¥¾Ó º¸¹° È¹µæ! °ñµå ¾ò¾úÀ½!! {goldAmount}");
            Destroy(gameObject);
        }
    }
}
