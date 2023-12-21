using UnityEngine;
using Mirror;
using System.Linq;
using UnityEditor.Experimental;

public class MyClientBehaviour : NetworkBehaviour
{
    IntanciateWorld intanciateWorld ; 
    private SyncList<int> rand = new SyncList<int>();
    private int[] randValueBrut;
  
  
    public void randvalueSet()
    {
        int []randValue = intanciateWorld.generateRandVal();
        for (int i = 0; i < randValue.Length; i++)
        {
            rand.Add(randValue[i]);
        }
       
    }

    

    public override void OnStartServer()
    {
        base.OnStartServer();
        intanciateWorld = FindObjectOfType<IntanciateWorld>();
        randvalueSet();

    }
    override public void OnStartClient()
    {
        intanciateWorld = FindObjectOfType<IntanciateWorld>();
        Debug.Log("Random   " + string.Join(", ", rand.ToArray()));
        intanciateWorld.instantiateWorld(rand.ToArray());
    }

    public override void OnStopClient()
    {
        base.OnStopServer();
        //intanciateWorld = FindObjectOfType<IntanciateWorld>();
        intanciateWorld.destroyWorld();
    }

    public  void Start()
    {
        

        //intanciateWorld = FindObjectOfType<IntanciateWorld>();
//
        //if (intanciateWorld == null)
        //{
        //    Debug.LogError("IntanciateWorld component not found in the scene.");
        //}
    
    }


}
