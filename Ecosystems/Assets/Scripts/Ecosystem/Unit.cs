using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {


    public GameObject manager;

    public Vector2 location;
    public Vector2 velocity;
    Vector2 goalPos;
    Vector2 currentForce;


    void Start () {

        location = new Vector2(Random.Range(0.01f,0.1f), Random.Range(0.01f, 0.1f));
        velocity = new Vector2(transform.position.x, transform.position.y);

    }

    Vector2 Seek(Vector2 target)
    {

        return (target - location);
    }

    void ApplyForce(Vector2 f)
    {
        Vector3 force = new Vector3(f.x, f.y, 0);

        if (force.magnitude > manager.GetComponent<AllUnits>().maxForce)
        {
            force = force.normalized;
            force *= manager.GetComponent<AllUnits>().maxForce;
        }

        transform.GetComponent<Rigidbody2D>().AddForce(force);

        if ( GetComponent<Rigidbody2D>().velocity.magnitude > manager.GetComponent<AllUnits>().maxVelocity)
        {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized;
            GetComponent<Rigidbody2D>().velocity *= manager.GetComponent<AllUnits>().maxVelocity ;
        }

        Debug.DrawRay(transform.position, force, Color.yellow);

    }

    Vector2 Align()
    {
        float neighboorDist = manager.GetComponent<AllUnits>().neighboorDistance;
        Vector2 sum = Vector2.zero;
        int count = 0;

        foreach (GameObject other in manager.GetComponent<AllUnits>().units)
        {
            if (other == this.gameObject) continue;

            float d = Vector2.Distance(location, other.GetComponent<Unit>().location);
            if (d < neighboorDist)
            {
                sum += other.gameObject.GetComponent<Unit>().velocity;
                count++;
            }
        }
            
            if (count > 0)
            {
                sum /= count;
                Vector2 steer = sum - velocity;
                return steer;
            }
            return Vector2.zero;
        }

    Vector2 Cohesion()
    {
        float neighboorDist = manager.GetComponent<AllUnits>().neighboorDistance;
        Vector2 sum = Vector2.zero;
        int count = 0;

        foreach (GameObject other in manager.GetComponent<AllUnits>().units)
        {
            if (other == this.gameObject) continue;
            
                float d = Vector2.Distance(location, other.GetComponent<Unit>().location);
                if (d < neighboorDist)
                {
                    sum += other.gameObject.GetComponent<Unit>().location;
                    count++;
                }
            }
            if (count > 0)
            {
                sum /= count;
                Vector2 steer = sum - velocity;
                return Seek(sum);
            }
            return Vector2.zero;
        }

    void Flock()
    {

        location = transform.position;
        velocity = GetComponent<Rigidbody2D>().velocity;

        if ( manager.GetComponent<AllUnits>().obedient && Random.Range(0,50) <= 1)
        {
            Vector2 align = Align();
            Vector2 coh = Cohesion();
            Vector2 gl;
            if (manager.GetComponent<AllUnits>().seekgoal)
            {
                gl = Seek(goalPos);
                currentForce = gl + align + coh;
            }
            else
            {
                currentForce = align + coh;
            }

            currentForce = currentForce.normalized;
        }

        if (manager.GetComponent<AllUnits>().willfull && Random.Range(0, 50) <= 1)
        {
            if (Random.Range(0, 50) <= 1)
            {
                currentForce = new Vector2(Random.Range(0.01f, 0.1f), Random.Range(0.01f, 0.1f)); 
            }
        }

        ApplyForce(currentForce);
    }
	
	// Update is called once per frame
	void Update () {

        //
        //if (!GetComponent<ButterflyBehavior>().isHidden)
        //{
            Flock();
            goalPos = manager.transform.position;
        //}	
	}
}
