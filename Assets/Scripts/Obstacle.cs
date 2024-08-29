using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class Obstacle : MonoBehaviour
{
    public GameObject collectedbrick;
    public int numberofcollectedbricks=0;
    private void OnTriggerEnter(Collider other)
    {
       
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "collectedbricks")
        {
            //numberofcollectedbricks++;
            //GameObject go = Instantiate(collectedbrick, other.gameObject.transform.position, other.gameObject.transform.rotation);
            //go.GetComponent<Rigidbody>().isKinematic = false;
            //go.GetComponent<Collider>().isTrigger = false;
            //PlayerScript.instance.brickParent.transform.GetChild(PlayerScript.instance.bricksCount).gameObject.SetActive(false);
        }
    }
    public void ins()
    {

    }
}
