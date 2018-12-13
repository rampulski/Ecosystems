using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntsColonie : Species {

    public GameObject Ant;
    private Transform Ants;
    private Vector2 nestPos;
    private int generation;
    private float reproductionFactor;
    public float maxPopulation;
    private float babiesCount;

	// Use this for initialization
	void Start () {
        Ants = transform.Find("Ants");
        //
        DNA = "ATCTAGAGTGCTGGGGAAAAAAAAGGGGATGGAGACAGAGATCTATCTTGGG";
        Decode(DNA);
        //
        nestPos = transform.position;
        generation = 0;
        maxPopulation = 300.0f;
        //
        InvokeRepeating("Reproduce", 0, clock);

        tmp_reproRate = reproductionRate;

    }
	
	// Update is called once per frame
	void Update () {
        //Decode(species_dna);

        if (Mathf.Abs(reproductionRate - tmp_reproRate) > 0.01f)
        {
            current_dna = Encode();
            tmp_reproRate = reproductionRate;
        }
    }

    void Reproduce () {
        // queen is enfanting new ants
        // calculate num of childrens from reproductionFactor
        reproductionFactor = Mathf.Clamp(((maxPopulation - Ants.childCount) / maxPopulation), 0.00f, 1.00f);
        babiesCount = 100 * reproductionFactor;
        //
        MakeUnikDNA("queen");
        //
        StartCoroutine("GiveBirth");

    }

    IEnumerator GiveBirth()
    {
        for (int i = 0; i < babiesCount; i++)
        {
            Instantiate(Ant, Random.insideUnitCircle*3, Quaternion.identity, Ants);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    protected void MakeUnikDNA(string type){
        // create variations in DNA
        if (type == "queen") {
            // size
            size = Mathf.Clamp(size + Random.Range(-0.1f, 0.1f), 1.0f, 10.0f);
            // color
            float H, S, V;
            Color.RGBToHSV(myColor, out H, out S, out V);
            H = Mathf.Clamp(H + Random.Range(-0.0400f, 0.0400f), 0.0f, 1.0f);
            myColor = Color.HSVToRGB(H,S,V);
            current_dna = Encode();
        }
        else {
            // size
            size = Mathf.Clamp(size + Random.Range(-0.1f, 0.1f), 1.0f, 10.0f);
            // color
            float H, S, V;
            Color.RGBToHSV(myColor, out H, out S, out V);
            H = Mathf.Clamp(H + Random.Range(-0.0400f, 0.0400f), 0.0f, 1.0f);
            myColor = Color.HSVToRGB(H, S, V);
            current_dna = Encode();
        }

    }
}
