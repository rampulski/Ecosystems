using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBehavior : Species {


    public GameManager gameManager;
    public int foodQuantity;

    private Vector3 maxSize;
    private Vector3 minSize;

    public float timeToBurst;
    public float particleSpeedTimer;

    public Color color;
    public float radiusSize;

    public Transform plantRenderer;
    public Transform particlesTransform;

    public ParticleSystem particlesSystem;
    private bool updateParticleSpeed;

    private bool canUnHide;
    private float timeToUnHide;
    private bool isHidden;

    public GameObject plantPrefab;

    private float timeToHide;

    void Start () {

        gameManager = GameManager.instance;

        plantRenderer = transform.Find("PlantRenderer");
        particlesSystem = transform.Find("Particles").GetComponent<ParticleSystem>();
        particlesSystem.emission.SetBurst(0, new ParticleSystem.Burst(0f, gameManager.plant_seedsPerBurstCount));
        particlesTransform = transform.Find("Particles");
        particlesSystem.Stop();

        Initialyze();		
	}
	
	void Update () {


        //Burst Freq
        timeToBurst += Time.deltaTime;

        if (timeToBurst >= gameManager.plant_reproductionRate)
        {
            Burst();
        }

        //Hide from player
        Hide();
    }

    private void Initialyze()
    {
        
        timeToBurst = 0f;
        isHidden = true;
        canUnHide = true;
        //set sizes
        maxSize = plantRenderer.localScale;
        minSize = Vector3.zero;
        plantRenderer.localScale = minSize;
    }

    private void Hide()
    {
        if (Vector3.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) <= 10f)
        {
            isHidden = true;
            timeToHide -= Time.deltaTime * 4f;
        }
        else
        {
            timeToHide += Time.deltaTime;
        }
        if (timeToHide >= 6f)
        {
            isHidden = false;
        }
        timeToHide = Mathf.Clamp(timeToHide, 0, 6f);
        plantRenderer.localScale = Vector3.Slerp(minSize, maxSize, (timeToHide / 6f));

    }
    public void Burst()
    {
        if (!isHidden)
        {
           //var sh = particlesSystem.shape;
           //sh.rotation = new Vector3(UnityEngine.Random.Range(0, 360), 0, 0);
            particlesSystem.Play();
            timeToBurst = 0f;
            
            if (UnityEngine.Random.Range(0,100) <= gameManager.plant_reproductionChance)
            {
                StartCoroutine(Reproduce());

            }
        }
    }

    public void Eat(int quantity)
    {
        if (foodQuantity + quantity >= 0)
        {
            foodQuantity -= quantity;
        }
    }

    public IEnumerator Reproduce()
    {
        yield return new WaitForSeconds(5f);

        var particles = new ParticleSystem.Particle[gameManager.plant_seedsPerBurstCount];
        particlesSystem.GetParticles(particles);

        for (int i = 0; i < particles.Length; i++)
        {
            if (Vector3.Distance(transform.position, particles[i].position) >= 5f)
            {
                Vector3 pos = particles[i].position;

                Instantiate(plantPrefab, pos, Quaternion.identity,GameObject.Find("Plants").transform);
                yield break;
            }
        }

        yield return null;
    }
}
