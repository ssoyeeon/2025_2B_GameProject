using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bildboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180, 0);        //�ؽ�Ʈ ������ ����
        }
    }
}
