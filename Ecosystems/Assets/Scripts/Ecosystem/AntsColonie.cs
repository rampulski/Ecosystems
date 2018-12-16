using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntsColonie : Species {

    public GameObject Ant;
    private Transform Ants;
    public Vector2 nestPos;
    private int generation;
    private float babiesCount;
    private float reproductionFactor;
    [SerializeField]
    private float maxPopulation, nestSize;

    // Use this for initialization
    void Start () {
        Ants = transform.Find("Ants");
        //
        DNA = "ATCCAGAGTGCCGGGGAAAAAAAAGGGGAGAGAAATAGAGCAAACAAAGGGG";
        current_dna = DNA;
        Decode(DNA);
        //
        nestPos = transform.position;
        nestSize = 1;
        transform.Find("Nest").transform.localScale = new Vector2(nestSize, nestSize);
        //
        generation = 0;
        maxPopulation = 300.0f;
        //
        InvokeRepeating("Reproduce", 0, clock);
        //
        tmp_lifeTime = lifeTime;
        tmp_reproRate = reproductionRate;
        tmp_clock = clock;
        tmp_myColor = myColor;
        tmp_speed = speed;
        tmp_complexity = complexity;
        tmp_size = size;
        tmp_temerity = temerity;
        tmp_craziness = craziness;
        tmp_sociability = sociability;
    }
	
	// Update is called once per frame
	void Update () {
        //Decode(species_dna);

        if (Mathf.Abs(lifeTime - tmp_lifeTime) > 0.01f)
        {
            current_dna = Encode();
            tmp_lifeTime = lifeTime;
        }
        if (Mathf.Abs(reproductionRate - tmp_reproRate) > 0.01f)
        {
            current_dna = Encode();
            tmp_reproRate = reproductionRate;
        }
        if (Mathf.Abs(clock - tmp_clock) > 0.01f)
        {
            current_dna = Encode();
            tmp_clock = clock;
        }
        if (myColor != tmp_myColor)
        {
            current_dna = Encode();
            tmp_myColor = myColor;
        }
        if (Mathf.Abs(speed - tmp_speed) > 0.01f)
        {
            current_dna = Encode();
            tmp_speed = speed;
        }
        if (Mathf.Abs(complexity - tmp_complexity) > 0.01f)
        {
            current_dna = Encode();
            tmp_complexity = complexity;
        }
        if (Mathf.Abs(size - tmp_size) > 0.01f)
        {
            current_dna = Encode();
            tmp_size = size;
        }
        if (Mathf.Abs(temerity - tmp_temerity) > 0.01f)
        {
            current_dna = Encode();
            tmp_temerity = temerity;
        }
        if (Mathf.Abs(craziness - tmp_craziness) > 0.01f)
        {
            current_dna = Encode();
            tmp_craziness = craziness;
        }
        if (Mathf.Abs(sociability - tmp_sociability) > 0.01f)
        {
            current_dna = Encode();
            tmp_sociability = sociability;
        }
    }

    void Reproduce () {
        // queen is enfanting new ants
        // calculate num of childrens from reproductionFactor
        reproductionFactor = Mathf.Clamp(((maxPopulation - Ants.childCount) / maxPopulation), 0.00f, 1.00f);
        babiesCount = 100 * reproductionFactor;
        //
        //MakeUnikDNA("queen");
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
        }
        else {
            // size
            size = Mathf.Clamp(size + Random.Range(-0.1f, 0.1f), 1.0f, 10.0f);
            // color
            float H, S, V;
            Color.RGBToHSV(myColor, out H, out S, out V);
            H = Mathf.Clamp(H + Random.Range(-0.0400f, 0.0400f), 0.0f, 1.0f);
            myColor = Color.HSVToRGB(H, S, V);
            // crazyness
            craziness = Random.Range(0.75f,1.25f) * craziness;
            temerity = Random.Range(0.75f, 1.25f) * temerity;
            clock = Mathf.RoundToInt(Random.Range(0.75f, 1.25f) * clock); // it will be "seconds" values

            // we encode unik dna
        }
        current_dna = Encode();

    }
}
