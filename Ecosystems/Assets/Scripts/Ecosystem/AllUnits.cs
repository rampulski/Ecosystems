using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllUnits : MonoBehaviour {


    public GameObject[] units;
    public GameObject unitPrefab;
    public int unitCount;
    public Vector3 range;

    public bool seekgoal;
    public bool obedient;
    public bool willfull;

    [Range (0, 200)]
    public int neighboorDistance;

    [Range(0, 2)]
    public float maxForce;

    [Range(0, 5)]
    public float maxVelocity;

    void Start () {
        units = new GameObject[unitCount];

        for (int i = 0; i < unitCount; i++)
        {
            Vector3 unitPos = Random.insideUnitCircle;
            units[i] = Instantiate(unitPrefab, this.transform.position + unitPos, Quaternion.identity, GameObject.Find("Butterflies").transform) as GameObject;
            units[i].GetComponent<Unit>().manager = this.gameObject;
            units[i].GetComponent<ButterflyBehavior>().manager = this.gameObject;

        }
        range = new Vector3(5, 5, 5);
    }
}
