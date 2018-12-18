using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDanger : MonoBehaviour {


    public float dangerosity;
    private float dangerosityMax;


    private Vector3 oldPos;

	void Start () {

        oldPos = transform.position;
        dangerosity = 10;
        dangerosityMax = dangerosity;
    }
	
	void Update () {

        if (Vector3.Distance(oldPos,transform.position) > 0)
        {
            dangerosity = dangerosityMax;
            oldPos = transform.position;
        }

        if (dangerosity <0)
        {
            dangerosity = 0;
        }
        dangerosity -= Time.deltaTime;


    }
}
