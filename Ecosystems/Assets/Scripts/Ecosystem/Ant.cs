using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : AntsColonie {

	// Use this for initialization
	void Start () {
        DNA = transform.parent.parent.GetComponent<AntsColonie>().current_dna;
        Decode(DNA);
        MakeUnikDNA("ant");

        // 
        transform.GetComponent<SpriteRenderer>().color = myColor;
        transform.localScale = new Vector3(size,size,1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

}
