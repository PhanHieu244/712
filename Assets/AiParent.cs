using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
public class AiParent : MonoBehaviour
{
    public int totalAIs;
    private void Awake()
    {
        totalAIs = transform.childCount;
        int num = Random.Range(0, totalAIs - 1);
        transform.GetChild(num).gameObject.SetActive(true);
        Debug.Log(transform.GetChild(num).gameObject.name + num);
        transform.GetChild(num).gameObject.GetComponent<SplineFollower>().computer = GameObject.Find("PlayerPath").GetComponent<SplineComputer>();
    }
    void Start()
    {

    }

}
