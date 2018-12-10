using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPosition : MonoBehaviour {

	private Vector3 mousePosition;

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;
        transform.position = mousePosition;

	}
}
