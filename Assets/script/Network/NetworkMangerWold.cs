using System.Collections;
using System.Collections.Generic;
using UnityEngine;/*
using Unity.Netcode;

public class NetworkMangerWold : NetworkManager
{
    // This method is called on the server when it is started public override void Start()
 
    IntanciateWorld intanciateWorld ;
    [SerializeField]   int nbrOfPlayer = 2;
    private NetworkVariable <int []> randVal = new NetworkVariable<int[]>(writePerm: NetworkVariableWritePermission.Server);    
    private NetworkVariable <string> test;  
    void Start()
    {  
        OnServerStarted += HandleServerStarted;
        OnClientConnectedCallback += HandleClientConnected;

        intanciateWorld = FindObjectOfType<IntanciateWorld>();

        if (intanciateWorld == null)
        {
            Debug.LogError("IntanciateWorld component not found in the scene.");
        }
    }
 

    // This method is called on the server when it is started
    private void HandleServerStarted()
    {
        //randVal.Value =
        //if (!NetworkManager.Singleton.IsServer)
        //    return;
        //test = new NetworkVariable<string>(writePerm: NetworkVariableWritePermission.Server);
        //test.Value = "test";
        intanciateWorld.instantiateWorld( intanciateWorld.generateRandVal());//randVal.Value);
    }

    private void HandleClientConnected(ulong clientId)
    {
        intanciateWorld.instantiateWorld( intanciateWorld.generateRandVal());
        //intanciateWorld.instantiateWorld(randVal.Value);
    }

    
    
}
*/