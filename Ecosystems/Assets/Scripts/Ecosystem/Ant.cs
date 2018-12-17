using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : AntsColonie {
    float life;
    int orientation;
    float new_angle, new_speed;
    float minSpeed, maxSpeed;
    float baseCraziness;
    Vector3 goalVel, nest;
    Vector2 vel;
    public int mode, prevMode;
    public float stopDur, stopTimer, stopFreq, eatDur, meetingTime; // different time variables
    public Collider2D lastDetectedObstacle;
    private PlantBehavior plantToEat;
    public Vector2 lastDetectedPlant;
    private SpriteRenderer foodBit;
    public Ant lastFriend;
    private Ant friend;

    void Start () {
        DNA = transform.parent.parent.GetComponent<AntsColonie>().current_dna;
        Decode(DNA);
        MakeUnikDNA("ant");
        //
        nest = transform.parent.parent.GetComponent<AntsColonie>().nestPos;
        //
        life = 0;
        transform.GetComponent<SpriteRenderer>().color = myColor;
        transform.localScale *= size * 0.2f;
        transform.GetComponent<TrailRenderer>().widthMultiplier = size * 0.01f;
        //
        mode = 0;
        //
        craziness = craziness * 5;
        baseCraziness = craziness;
        speed = speed * 5;
        //
        orientation = Random.Range(0, 1) * 2 - 1; // -1 or 1 --> where am I going ? (turn to left or right --> quite political)
        StandardMove();
        //
        stopFreq = 30 * (1-temerity); // clock -> sec   temerity -> range 0-1
        eatDur = clock * Random.Range(0.75f, 1.25f); // clock -> sec   temerity -> range 0-1
        stopDur = 0;
        meetingTime = 0;
        //
        foodBit = transform.Find("foodBit").gameObject.GetComponent<SpriteRenderer>();
        foodBit.transform.localScale = new Vector2(0, 0);
    }

    // basic move functions
    private void StandardMove()
    {
        new_angle = Random.Range(0, 180);
        new_speed = speed * craziness * Random.Range(0.8f, 1.2f);
        vel = new Vector2(new_speed * Mathf.Cos(new_angle), new_speed * Mathf.Sin(new_angle));
    }

    private void MoveToTarget(Vector3 targetPos)
    {
        Vector3 dir = targetPos - transform.position;
        vel = Vector3.MoveTowards(transform.position, dir, 1);
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        Vector3 lastPos = transform.position;
        life += Time.deltaTime; // life duration in secondes

        switch (mode)
        {
            case 0:
                IsSearching();
                break;
            case 1:
                DetectedFood();
                foodBit.transform.localScale = new Vector2(0,0);
                break;
            case 2:
                EatFood();
                break;
            case 3:
                ReturnWithFood();
                foodBit.transform.localScale = new Vector2(0.9f, 0.4f);
                break;
            case 4:
                ReturnWithoutFood();
                break;
            case 5:
                Stopped();
                break;
            case 6:
                HelpFriend();
                break;
            case 61:
                FriendHasFood();
                break;
            case 7:
                AvoidObstacle();
                break;
        }


        // then update pos rot 
        NormaliseSpeed();

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
    void IsSearching() // MODE 0
    {
        stopTimer += Time.deltaTime;

        // ant is searching food around the world
        float pert_angle = Random.Range(0, 360);
        float pert_speed = craziness * vel.magnitude * Random.Range(.05f, .1f);
        Vector2 pert = new Vector2(pert_speed * Mathf.Cos(pert_angle), pert_speed * Mathf.Sin(pert_angle));
        //
        vel = vel + pert;

        // stop sometimes to re-orient randomly
        if (stopTimer > stopFreq)
        {
            vel = vel * 0; // we stp
            stopDur = clock * Random.Range(0.2f,0.5f);
            stopTimer = 0;
            mode = 5;
            prevMode = 0;
        }

    }

    void DetectedFood () // MODE 1
    {
        //Debug.Log(collidedObject.name);
        MoveToTarget(lastDetectedPlant);

        // pertubate the move a little
        float pert_angle = Random.Range(-45, 45);
        float pert_speed = craziness * vel.magnitude * Random.Range(.05f, .1f);
        Vector2 pert = new Vector2(pert_speed * Mathf.Cos(pert_angle), pert_speed * Mathf.Sin(pert_angle));

        // update vel
        vel = vel + pert;

    }

    void EatFood() // MODE 2
    {
        stopTimer += Time.deltaTime;
        // stop sometimes to re-orient randomly
        if (stopTimer > eatDur)
        {
            vel = vel * 0; // we stp
            stopDur = clock * Random.Range(0.2f, 0.5f);
            if (plantToEat != null) plantToEat.Eat(1);
            // go to "ReturnWithFood" mode avec collecting food !
            prevMode = 3;
            mode = 5;
            // reset timer
            stopTimer = 0;
        }
    }

    void ReturnWithFood()  // MODE 3
    {
        float nestDist = Vector3.Distance(nest,transform.position);
        //
        if (nestDist < 1)
        {
            mode = 1;
        }
        else
        {
            MoveToTarget(nest);
            // pertubate the move a little
            float pert_angle = Random.Range(-45,45);
            float pert_speed = craziness * vel.magnitude * Random.Range(.05f, .1f);
            Vector2 pert = new Vector2(pert_speed * Mathf.Cos(pert_angle), pert_speed * Mathf.Sin(pert_angle));
            vel = vel + pert;
        }
    }

    void ReturnWithoutFood()  // MODE 4
    {

    }

    void Stopped()  // MODE 5
    {
        if (stopDur > 0) stopDur -= Time.deltaTime;
        else
        {
            StandardMove();         
            mode = prevMode;
            stopTimer = 0;
            stopDur = 0;
        }

    }

    void HelpFriend()  // MODE 6
    {
        meetingTime += Time.deltaTime;
        // we stop a moment to share info
        vel = vel * 0;
        //
        if (meetingTime > 5.0f)
        {
            StandardMove();
            meetingTime = 0;
            mode = prevMode;
        }
    }

    void FriendHasFood() // MODE 61
    {
        // function called when ant met antoher ant with food
        meetingTime += Time.deltaTime;
        // we stop a moment to share info
        vel = vel * 0;
        //
        if (meetingTime > 5.0f)
        {
            StandardMove();
            meetingTime = 0;
            mode = 1;
        }
    }

    void AvoidObstacle() // MODE 7
    {
        Vector2 obsPos = lastDetectedObstacle.transform.position;
        float obsDist = Vector2.Distance(obsPos,transform.position);

    }

    //
    void NormaliseSpeed()
    {
        minSpeed = speed * (craziness / 10) * Random.Range(0.5f, 0.8f);
        maxSpeed = speed * (craziness / 10) * Random.Range(1.2f, 1.5f);
        Vector2 nVel = vel.normalized;

        switch (mode)
        {
            case 0:
                if (vel.magnitude > maxSpeed) vel = nVel * maxSpeed;
                if (vel.magnitude < minSpeed) vel = nVel * minSpeed;
                break;
            case 1:
                if (vel.magnitude > 1.5 * maxSpeed) vel = nVel * (1.5f * maxSpeed);
                if (vel.magnitude < 1.5 * minSpeed) vel = nVel * (1.5f * minSpeed);
                break;
        }


        vel = vel * 0.006f; // normalized for Unity scale

    }

    //
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.collider.transform.gameObject.name);
        if (collision.collider.CompareTag("Plant"))
        {
            // touch a plant --> eat food mode
            plantToEat = collision.collider.transform.gameObject.GetComponent<PlantBehavior>();
            mode = 2;
        }
        else if (collision.collider.CompareTag("Ant"))
        {
            // ant has food
            friend = collision.collider.transform.gameObject.GetComponent<Ant>();
            if (mode == 3 && friend != lastFriend && (friend.mode == 0 || friend.mode == 4))
            {
                // friend do not has food, friend is searching for food, friend is not last friend met
                // we pass ONCE to friend our last visited plant's position
                friend.lastDetectedPlant = lastDetectedPlant; 
                friend.mode = 61;
                prevMode = mode;
                mode = 6;
            }
        }
    }
}
