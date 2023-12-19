using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnetFollowCamera : MonoBehaviour
{
   
    void Update()
    {
        transform.position = new Vector3(Camera.main.transform.position.x-1, Camera.main.transform.position.y - 4f, Camera.main.transform.position.z + 1);
    }
}
