using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Species : MonoBehaviour {
    // parent class for all elements using DNA in ecosystem
    public string DNA;
    public string current_dna;
    //
    string speciesName;
    //
    public int lifeTime;
    public float reproductionRate; 
    protected float tmp_reproRate;
    public int clock;
    public Color myColor;
    public float speed;
    public int complexity;
    public float size;
    public float temerity;
    public float craziness;
    public float sociability;


    protected string Encode ()
    {
        // create table of values 0-255 from DNA values
        int[] adn_table = new int[13];
        // biological clock
        adn_table[0] = Mathf.RoundToInt(Map(lifeTime, 0, 100, 0, 255));
        adn_table[2] = Mathf.RoundToInt(Map(clock, 1, 24, 0, 255));
        adn_table[3] = Mathf.RoundToInt(Map(myColor.r,0,1,0,255));
        adn_table[4] = Mathf.RoundToInt(Map(myColor.g, 0, 1, 0, 255));
        adn_table[5] = Mathf.RoundToInt(Map(myColor.b, 0, 1, 0, 255));
        adn_table[6] = Mathf.RoundToInt(Map(myColor.a, 0, 1, 0, 255));
        adn_table[7] = Mathf.RoundToInt(Map(speed, 0, 10, 0, 255));
        adn_table[1] = Mathf.RoundToInt(Map(reproductionRate, 0, 1, 0, 255));
        adn_table[8] = complexity;
        adn_table[9] = Mathf.RoundToInt(Map(size, 0, 10, 0, 255));
        adn_table[10] = Mathf.RoundToInt(Map(temerity, 0, 1, 0, 255));
        adn_table[11] = Mathf.RoundToInt(Map(craziness, 0, 10, 0, 255));
        adn_table[12] = Mathf.RoundToInt(Map(sociability, 0, 1, 0, 255));

        // encode DNA string
        string res = "";
        for (int d = 0; d < adn_table.Length; d++)
        {
            int aux = adn_table[d];
            for (int i = 0; i < 4; i++)
            {
                int q = aux / (int)Mathf.Pow(4, 3 - i);
                switch (q)
                {
                    case 0:
                        res += "A";
                        break;
                    case 1:
                        res += "T";
                        break;
                    case 2:
                        res += "C";
                        break;
                    case 3:
                        res += "G";
                        break;
                }
                aux = aux % (int)Mathf.Pow(4, 3 - i);
            }
        }
        return res;
    }

    protected void Decode (string data)
    {
        int[] decodedDNA = new int[13];
        for (int i = 0; i < decodedDNA.Length; i++) {
          int res = 0;
          for (int j=0; j < 4; j++){
            switch (data[(i*4)+j]) {
              case 'A': res += Mathf.RoundToInt(0 * Mathf.Pow(4,3-j));
                        break;
              case 'T': res += Mathf.RoundToInt(1 * Mathf.Pow(4,3-j));
                        break;
              case 'C': res += Mathf.RoundToInt(2 * Mathf.Pow(4,3-j));
                        break;
              case 'G': res += Mathf.RoundToInt(3 * Mathf.Pow(4,3-j));
                        break;
            }
          }
          decodedDNA[i] = res;
        }

        // populate adn vars with decoded values
        lifeTime = Mathf.RoundToInt(Map(decodedDNA[0], 0, 255, 0, 100));
        reproductionRate = Map(decodedDNA[1], 0, 255, 0, 1);
        clock = Mathf.RoundToInt(Map(decodedDNA[2], 0, 255, 1, 24));
        // physic parameters
        float coloR = Map(decodedDNA[3], 0, 255, 0, 1);
        float coloG = Map(decodedDNA[4], 0, 255, 0, 1);
        float coloB = Map(decodedDNA[5], 0, 255, 0, 1);
        float coloA = Map(decodedDNA[6], 0, 255, 0, 1);
        myColor = new Color(coloR, coloG, coloB, coloA);
        speed = Map(decodedDNA[7], 0, 255, 0, 10);
        complexity = decodedDNA[8];
        size = Map(decodedDNA[9], 0, 255, 0, 10);
        // behaviours
        temerity = Map(decodedDNA[10], 0, 255, 0, 1);
        craziness = Map(decodedDNA[11], 0, 255, 0, 10);
        sociability = Map(decodedDNA[12], 0, 255, 0, 1);

    }

    public float Map(float val, float from_min, float from_max, float to_min, float to_max)
    {
        // map value from range to range
        return (val - from_min) * (to_max - to_min) / (from_max - from_min) + to_min;
    }
}
