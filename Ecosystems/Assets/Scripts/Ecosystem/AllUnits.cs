using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllUnits : MonoBehaviour {


    public List<GameObject> units;
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
        units = new List<GameObject>();

        for (int i = 0; i < unitCount; i++)
        {
            Vector3 unitPos = Random.insideUnitCircle;
            GameObject gO;
            gO = Instantiate(unitPrefab, this.transform.position + unitPos, Quaternion.identity, GameObject.Find("Butterflies").transform) as GameObject;
            gO.GetComponent<Unit>().manager = this.gameObject;
            gO.GetComponent<ButterflyBehavior>().manager = this.gameObject;
            units.Add(gO);

        }
        range = new Vector3(5, 5, 5);
    }
}
