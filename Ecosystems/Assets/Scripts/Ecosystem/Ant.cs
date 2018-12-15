using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : AntsColonie {
    float life;
    int orientation;
    float new_angle, new_speed;
    float minSpeed, maxSpeed;
    float baseCraziness;
    Vector2 goal;
    Vector3 goalVel;
    Vector2 vel;
    public int mode;
    public float stopDur, stopTimer, stopFreq;

    void Start () {
        DNA = transform.parent.parent.GetComponent<AntsColonie>().current_dna;
        Decode(DNA);
        MakeUnikDNA("ant");
        //
        life = 0;
        transform.GetComponent<SpriteRenderer>().color = myColor;
        transform.localScale = new Vector3(size*0.2f,size*0.1f,1);
        transform.GetComponent<TrailRenderer>().widthMultiplier = size * 0.01f;
        //
        mode = 0;
        //
        craziness = craziness * 5;
        baseCraziness = craziness;
        speed = speed * 5;
        //
        /* political */ orientation = Random.Range(0, 1) * 2 - 1; // -1 or 1 --> where am I going ? (turn to left or right)
        StandardMove();
        //
        stopFreq = 30 * (1-temerity); // clock -> sec   temerity -> range 0-1
        stopDur = 0;
    }

    // basic move function
    private void StandardMove()
    {
        new_angle = Random.Range(0, 180);
        new_speed = speed * craziness * Random.Range(0.8f, 1.2f);
        vel = new Vector2(new_speed * Mathf.Cos(new_angle), new_speed * Mathf.Sin(new_angle));
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        Vector3 lastPos = transform.position;
        life += Time.deltaTime; // life duration in secondes

        switch (mode)
        {
            case 0:
                isSearching();
                break;
            case 1:
                detectedFood();
                break;
            case 2:
                returnWithFood();
                break;
            case 3:
                returnWithoutFood();
                break;
            case 4:
                avoidObstacle();
                break;
            case 5:
                stopped();
                break;
            case 6:
                scared();
                break;
        }


        // then update pos rot 
        normaliseSpeed();

        goalVel = new Vector2(vel.x, vel.y);
        Vector3 nextDir = transform.position + goalVel;
        transform.position = nextDir;

        if (transform.position != lastPos)
        {
            float angle = Mathf.Atan2(goalVel.y, goalVel.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }


    }



    // modal comportment
    void isSearching() // MODE 0
    {

        stopTimer += Time.deltaTime;
        // ant is searching food around the world
        float pert_angle = Random.Range(0,360);
        float pert_speed = craziness * vel.magnitude * Random.Range(.05f, .1f);
        Vector2 pert = new Vector2(pert_speed * Mathf.Cos(pert_angle), pert_speed * Mathf.Sin(pert_angle));
        //
        vel = vel+pert;

        // stop sometimes to re-orient randomly
        if (stopTimer > stopFreq)
        {
            vel = vel * 0; // we stp
            stopDur = clock * Random.Range(0.2f,0.5f);
            stopTimer = 0;
            mode = 5;
        }

    }

    void detectedFood () // MODE 1
    {

    }

    void returnWithFood()  // MODE 2
    {

    }

    void returnWithoutFood()  // MODE 3
    {

    }

    void avoidObstacle()  // MODE 4
    {

    }

    void stopped()  // MODE 5
    {
        if (stopDur > 0) stopDur -= Time.deltaTime;
        else
        {
            StandardMove();
            mode = 0;
        }
    }

    void scared()  // MODE 6
    {

    }

    //
    void normaliseSpeed()
    {
        minSpeed = speed * (craziness / 10) * Random.Range(0.5f, 0.8f);
        maxSpeed = speed * (craziness / 10) * Random.Range(1.2f, 1.5f);
        Vector2 nVel = vel.normalized;

        if (mode != 2 && mode != 5)
        {
            if (mode != 1)
            { 
                if (vel.magnitude > maxSpeed) vel = nVel * maxSpeed;
                if (vel.magnitude < minSpeed) vel = nVel * minSpeed;
            }
            else
            {
                if (vel.magnitude > 1.5 * maxSpeed) vel = nVel * (1.5f * maxSpeed);
                if (vel.magnitude < 1.5 * minSpeed) vel = nVel * (1.5f * minSpeed);
            }
        }

        vel = vel * 0.001f; // normalized for Unity scale

    }
}
