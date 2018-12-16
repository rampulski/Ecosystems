using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDetection : MonoBehaviour {

    private Ant parentScript;

    void Start ()
    {
        parentScript = transform.parent.GetComponent<Ant>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Plant") && (parentScript.mode == 0 || parentScript.mode == 4))
        {
            // ant is entering in the PlantTrigger object of a plant while searching for food           
            parentScript.lastDetectedPlant = collision;
            parentScript.mode = 1;
            //Debug.Log("FoodDetected");

        }
        else if (collision.CompareTag("Obstacle"))
        {
            // get close to obstacle, need to re-orient
            parentScript.lastDetectedObstacle = collision;
            parentScript.mode = 7;
        }
    }

}
