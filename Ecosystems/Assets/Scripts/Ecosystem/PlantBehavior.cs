using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlantBehavior : MonoBehaviour
{

    public GameManager gameManager;
    public int foodQuantity;
    private Transform player;

    public Vector3 maxSizeTemp;
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

    public Animator anim;
    private float foodQuantitymax;
    private bool isDead;

    void Start () {

        gameManager = GameManager.instance;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        plantRenderer = transform.Find("PlantRenderer");
        particlesSystem = transform.Find("Particles").GetComponent<ParticleSystem>();
        particlesTransform = transform.Find("Particles");
        particlesSystem.Stop();
        anim = plantRenderer.GetComponent<Animator>();
        color = plantRenderer.GetComponent<SpriteRenderer>().color;



        Initialyze();		
	}
	
	void Update () {


        //Burst Freq
        timeToBurst += Time.deltaTime;

        if (!isDead)
        {
            if (timeToBurst >= gameManager.plant_reproductionRate)
            {
            Burst();
            }

        //Hide from player
            Hide();
        }     
    }

    private void Initialyze()
    {
        
        timeToBurst = 0f;
        isHidden = true;
        canUnHide = true;
        //set sizes
        maxSizeTemp = new Vector3 (gameManager.plant_size, gameManager.plant_size, 1);
        maxSize = maxSizeTemp;
        particlesSystem.emission.SetBurst(0, new ParticleSystem.Burst(0f, gameManager.plant_seedsPerBurstCount));
        minSize = Vector3.zero;
        transform.localScale = minSize;
        foodQuantitymax = foodQuantity;
        //color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);

        MakeUnique();
    }

    private void MakeUnique()
    {
        maxSizeTemp = maxSizeTemp * Random.Range(0.8f, 1.2f);
        anim.speed = Random.Range(0.6f, 1.8f);
        //plantRenderer.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
    }

    private void Hide()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= 10f && player.GetComponent<playerDanger>().dangerosity > 0)
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
        transform.localScale = Vector3.Slerp(minSize, maxSizeTemp, (timeToHide / 6f));

    }
    public void Burst()
    {
        if (!isHidden)
        {
           //var sh = particlesSystem.shape;
           //sh.rotation = new Vector3(UnityEngine.Random.Range(0, 360), 0, 0);
            particlesSystem.Play();
            timeToBurst = 0f;
            
            if (Random.Range(0,100) <= gameManager.plant_reproductionChance)
            {
                StartCoroutine(Reproduce());

            }
        }
    }

    public void Eat(int quantity)
    {
        if (foodQuantity - quantity >= 0)
        {
            foodQuantity -= quantity;
            float foodQuantityLerp = foodQuantity;
            maxSizeTemp = Vector3.Lerp(Vector3.one * 0.4f, maxSize, foodQuantityLerp / foodQuantitymax);
        }
        else
        {
            Death();
        }
    }

    public IEnumerator Reproduce()
    {
        yield return new WaitForSeconds(2f);

        var particles = new ParticleSystem.Particle[gameManager.plant_seedsPerBurstCount];
        particlesSystem.GetParticles(particles);

        for (int i = 0; i < particles.Length; i++)
        {
            if (Vector3.Distance(transform.position, particles[i].position) >= 5f)
            {
                Vector3 pos = particles[i].position;

                Instantiate(plantPrefab, pos, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)), GameObject.Find("Plants").transform);
                yield break;
            }
        }

        yield return null;
    }

    private void Death()
    {
        isDead = true;
        GetComponentsInChildren<CircleCollider2D>()[1].enabled = false;
        plantRenderer.GetComponent<SpriteRenderer>().color = Color.grey;
    }
}
