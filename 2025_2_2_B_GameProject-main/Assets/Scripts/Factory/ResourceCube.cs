using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

public class ResourceCube : MonoBehaviour
{
    public ResourceType type;

    public void Initalize(ResourceType resourceType)
    {
        type = resourceType;
        Renderer renderer = GetComponent<Renderer>();

        if (resourceType == ResourceType.Wood) renderer.material.color = new Color(0.6f, 0.3f, 0.1f);
        if (resourceType == ResourceType.Wood) renderer.material.color = Color.gray;
    }
}
