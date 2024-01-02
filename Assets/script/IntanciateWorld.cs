using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class IntanciateWorld : MonoBehaviour
{
    // Start is called before the first frame update
    static public int Worldsize = 150;

    [SerializeField] GameObject floor;
    [SerializeField] GameObject tree0;
    [SerializeField] GameObject tree1;
    [SerializeField] GameObject tree2;
    [SerializeField] GameObject tree3;
    [SerializeField] GameObject tree4;
    [SerializeField] GameObject fallen_tree0;
    [SerializeField] GameObject fallen_tree1;
    [SerializeField] GameObject flower0;
    [SerializeField] GameObject flower1;
    [SerializeField] GameObject flower2;
    [SerializeField] GameObject flower3;
    [SerializeField] GameObject bush0;
    [SerializeField] GameObject bush1;
    [SerializeField] GameObject grass0;
    [SerializeField] GameObject grass1;
    [SerializeField] GameObject plant;
    [SerializeField] GameObject rock0;
    [SerializeField] GameObject rock1;
    [SerializeField] GameObject rock2;
    [SerializeField] GameObject rock3;
    

    [SerializeField] private int tree0_densitie;
    [SerializeField] private int tree1_densitie;
    [SerializeField] private int tree2_densitie;
    [SerializeField] private int tree3_densitie;
    [SerializeField] private int tree4_densitie;
    [SerializeField] private int fallen_tree0_densitie;
    [SerializeField] private int fallen_tree1_densitie;
    [SerializeField] private int flower0_densitie;
    [SerializeField] private int flower1_densitie;
    [SerializeField] private int flower2_densitie;
    [SerializeField] private int flower3_densitie;
    [SerializeField] private int bush0_densitie;
    [SerializeField] private int bush1_densitie;
    [SerializeField] private int grass0_densitie;
    [SerializeField] private int grass1_densitie;
    [SerializeField] private int plant_densitie;
    [SerializeField] private int rock0_densitie;
    [SerializeField] private int rock1_densitie;
    [SerializeField] private int rock2_densitie;
    [SerializeField] private int rock3_densitie;
    

    [SerializeField] int max_d = 5000; //

    // SUM densities (should) <= 100;
    private int nbrOfAsset = 20;
    // Grass random material
    [SerializeField] private Material grassM0;
    [SerializeField] private Material grassM1;
    [SerializeField] private Material grassM2;
    [SerializeField] private Material grassM3;
    [SerializeField] private Material grassM4;
    //[SerializeField] private Material grassM5;


    int[] randVal = new int[Worldsize * Worldsize];
    GameObject[] Level = new GameObject[] {};

    public int []generateRandVal()
    {
        int i = 0;
        while (i < Worldsize * Worldsize) {
            randVal[i] = Random.Range(0, max_d);
            i++;
        }
        return randVal;
    }

    public int []getRandVal()
    {
        return randVal;
    }
     [SerializeField] private GameObject level;

    public void instantiateWorld(int[] randValue)
    {
        Vector3 v = new Vector3(0, 0, 0);
        float rand;
       
        int[] d = new int[]               {0, tree0_densitie, tree1_densitie, tree2_densitie, tree3_densitie, tree4_densitie, fallen_tree0_densitie, fallen_tree1_densitie, flower0_densitie, flower1_densitie, flower2_densitie, flower3_densitie, bush0_densitie, bush1_densitie, grass0_densitie, grass1_densitie, plant_densitie, rock0_densitie, rock1_densitie, rock2_densitie, rock3_densitie, max_d};
        GameObject[] buff = new GameObject[] {tree0,          tree1,          tree2,          tree3,          tree4,          fallen_tree0,          fallen_tree1,          flower0,          flower1,          flower2,          flower3,          bush0,          bush1,          grass0,          grass1,          plant,          rock0,          rock1,          rock2,          rock3};
        Material[] buffM = {grassM0, grassM1, grassM2, grassM3, grassM4};
        int ret;
        GameObject buffFloor;
        GameObject buffG;
       
        //Light light;
        // check if densiti is > 100;
        for(int z = 0; z < Worldsize; z++)
            for(int x = 0; x < Worldsize; x++) {
                v.Set(x, 0, z);
                buffFloor = Instantiate(floor,v ,Quaternion.identity);
                buffFloor.transform.GetChild(0).GetComponent<MeshRenderer>().material = buffM[Random.Range(0, buffM.Length - 1)];
                buffFloor.tag = "Level";
                buffFloor.transform.parent = level.transform;
                rand = randValue[x + (z * Worldsize)];
                ret = 0;
                for (int i = 0; i < nbrOfAsset; i++) {
                    if (rand >=  ret && rand <= (d[i + 1] + ret)) {
                        v.Set(x, 0.5f, z);
                        buffG = Instantiate(buff[i], v ,Quaternion.identity);
                        buffG.transform.parent = level.transform;
                        buffG.tag = "Level";
                        break; // NOT NESSERARY BUT OPTI
                    }
                    ret += d[i];
                }
            }
        var navMeshSurface = gameObject.AddComponent<NavMeshSurface>();
        string layerToIgnore = "NonBlockantPath";
        int layerToIgnoreMask = 1 << LayerMask.NameToLayer(layerToIgnore);
        
        if (navMeshSurface != null)
        {
            navMeshSurface.layerMask = ~layerToIgnoreMask; // Use ~ to invert the mask (exclude the specified layer)
            navMeshSurface.BuildNavMesh();
        }
        else
        {
            Debug.LogError("NavMeshSurface component is not attached to the gameObject.");
        }

//        Lightmapping.Bake();
    }

    public void destroyWorld()
    {
        Debug.Log("Destroying world" + Level.Length);
        // get all object with tag "Level"
        Level = GameObject.FindGameObjectsWithTag("Level");
        Debug.Log("Destroying world" + Level.Length);
        foreach (GameObject obj in Level)
        {
            Destroy(obj);
        }
    }

}
