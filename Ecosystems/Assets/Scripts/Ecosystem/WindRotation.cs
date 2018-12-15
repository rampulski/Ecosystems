using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindRotation : MonoBehaviour {

    public float rotationAngleMin;
    public float rotationAngleMax;

    private float rotationAngle;

    public float timeToChangeSpeed;
    private float timer;


    void Start () {

        timeToChangeSpeed = 0f;

    }
	
	void Update () {

        timer += Time.deltaTime;

        if (timer >= timeToChangeSpeed)
        {
            rotationAngle = Random.Range(rotationAngleMin, rotationAngleMax);
            timer = 0f;
        }
        transform.Rotate(Vector3.right, rotationAngle);

	}
}
