using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {


    public static GameManager instance = null;

    [Range (1,50)]
    public float plant_reproductionRate;
    [Range (0,100)]
    public int plant_reproductionChance;
    [Range(1, 100)]
    public int plant_seedsPerBurstCount;
    [Range(0.5f, 3f)]
    public float plant_size;


    private void Awake()
    {
        if (instance== null)
        {
            instance = this;
        }
        else if (instance!=this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        InitGame();
    }

    void InitGame()
    {

    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
