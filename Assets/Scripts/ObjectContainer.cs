using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ObjectContainer : MonoBehaviour
{

    public List<GameObject> objects = new List<GameObject>();
    public GameObject needObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Verify()
    {
        if (objects.Count > 0)
        {
            if (needObject.GetComponentInChildren<Draggable>().verifCode == objects[0].GetComponent<Draggable>().verifCode) return true;
            else return false;
        }
        else return false;
    }

}
