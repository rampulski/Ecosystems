using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyBehavior : MonoBehaviour {

    public GameObject manager;
    private SpriteRenderer rndr;

    private Transform player;

    public bool isHidden;
    private bool canMoveManager;

    private void Start()
    {
        rndr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update () {

        if (Vector2.Distance(transform.position, player.position) <= 1)
        {
            Appear();
            canMoveManager = true;
            MoveManager();
        }
        else if(Vector2.Distance(transform.position, manager.transform.position) <= 2)
        {
            Hide();
        }
    }


    void Hide()
    {
        rndr.color = new Color(rndr.color.r, rndr.color.g, rndr.color.b, 0);
        isHidden = true;
    }

    void Appear()
    {
        rndr.color = new Color(rndr.color.r, rndr.color.g, rndr.color.b, 1);
        isHidden = false;
    }

    void MoveManager()
    {
        Vector3 pos = new Vector3(Random.value, Random.value, 0);
        pos = Camera.main.ViewportToWorldPoint(pos);
        manager.transform.position = pos;
        canMoveManager = false;
    }
}
