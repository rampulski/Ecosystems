using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBehavior : MonoBehaviour {

    public int foodQuantity;

    private Vector3 maxSize;

    public float burstFreq;
    public int particlesCount;

    public float timeToBurst;
    public float particleSpeedTimer;

    public Color color;
    public float radiusSize;

    public Transform plantRenderer;
    public Transform particlesTransform;

    public ParticleSystem particlesSystem;
    private bool updateParticleSpeed;


    private bool canUnHide;
    private bool isHidden;

    public GameObject plantPrefab;

    [Range(0,100)]
    public int reproducingChancePercentage;

    void Start () {

        plantRenderer = transform.Find("PlantRenderer");
        particlesSystem = transform.Find("Particles").GetComponent<ParticleSystem>();
        particlesSystem.emission.SetBurst(0, new ParticleSystem.Burst(0f, particlesCount));
        particlesTransform = transform.Find("Particles");
        particlesSystem.Stop();

        Initialyze();		
	}
	
	void Update () {


        //Burst Freq
        timeToBurst += Time.deltaTime;

        if (timeToBurst >= burstFreq)
        {
            Burst();
        }

        //Hide from player
        if (Vector3.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) <= 20f)
        {
            Hide();
            canUnHide = true;
        }
        else
        {            
            if (canUnHide)
            {
                StartCoroutine(UnHide());
                canUnHide = false;
            }
        }
    }

    private void Initialyze()
    {
        timeToBurst = 0f;
        canUnHide = true;
        maxSize = plantRenderer.localScale;
    }

    private void Hide()
    {
        plantRenderer.localScale = Vector3.zero;
        isHidden = true;
    }

    private IEnumerator UnHide()
    {
        float elapsedTime = 0f;
        isHidden = false;
        yield return new WaitForSeconds(1f);

        while (elapsedTime <= 3f)
        {
            plantRenderer.localScale = Vector3.Lerp(plantRenderer.localScale, maxSize, (elapsedTime / 3f));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    public void Burst()
    {
        if (!isHidden)
        {
            var sh = particlesSystem.shape;
            sh.rotation = new Vector3(UnityEngine.Random.Range(0, 360), 0, 0);
            particlesSystem.Play();
            timeToBurst = 0f;

            if (UnityEngine.Random.Range(0,100) <= reproducingChancePercentage)
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
        Debug.Log("Reproducing");
        yield return new WaitForSeconds(3f);

        var particles = new ParticleSystem.Particle[particlesCount];
        particlesSystem.GetParticles(particles);

        for (int i = 0; i < particles.Length; i++)
        {
            if (Vector3.Distance(transform.position, particles[i].position) >= 15)
            {
                Vector3 pos = particles[i].position;

                Instantiate(plantPrefab, pos, Quaternion.identity,GameObject.Find("Plants").transform);
                Debug.Log("Reproduced");
                yield break;
            }
        }

        yield return null;
    }
}
