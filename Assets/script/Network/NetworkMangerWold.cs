using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkMangerWold : NetworkManager
{
    // This method is called on the server when it is started public override void Start()
 
    IntanciateWorld intanciateWorld ;
    [SerializeField]   int nbrOfPlayer = 2;
    int []randVal;
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
        randVal = intanciateWorld.generateRandVal();
        intanciateWorld.instantiateWorld(randVal);
    }

    private void HandleClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer)
         ;
       // intanciateWorld.instantiateWorld(randVal);
    }

    
    [ClientRpc]
    private void ReceiveRandomValues(int[] receivedRandVal)
    {
        randVal = receivedRandVal;
        intanciateWorld.instantiateWorld(randVal);
    }
}
