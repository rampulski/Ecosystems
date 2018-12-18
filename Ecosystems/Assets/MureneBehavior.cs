using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MureneBehavior : MonoBehaviour {

    private bool canEat;
    private float timer;
    private Vector3 startPos;

	void Start () {

        canEat = true;
        startPos = transform.position;
    }
	
	void Update () {

        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            canEat = true;
        }      
    }


    IEnumerator Eat(Vector3 pos)
    {
        canEat = false;

        transform.position = pos;

        yield return new WaitForSeconds(0.3f);

        transform.position = startPos;

        yield return null;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canEat && collision.transform.tag == "Butterfly")
        {
            StartCoroutine(Eat(collision.transform.position));
            Debug.Log("StartCoroutine");
            collision.transform.GetComponent<ButterflyBehavior>().Death();
            timer = 0f;
        }

    }

}
