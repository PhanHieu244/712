using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    GameObject LadderParent;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag== "ladder")
        {
            Debug.Log("Ladder touch Ground");
            //Destroy(other.gameObject.AddComponent<Rigidbody>(),2f);
        }
    }
}
