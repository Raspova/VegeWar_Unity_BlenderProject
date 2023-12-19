using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
public class NetWorkManagerMirror :  Mirror.NetworkManager
{
    // Start is called before the first frame update
    IntanciateWorld intanciateWorld ;

    int[] randValue;

    public override void Start()
    {
        base.Start();
        intanciateWorld = FindObjectOfType<IntanciateWorld>();

        if (intanciateWorld == null)
        {
            Debug.LogError("IntanciateWorld component not found in the scene.");
        }
       
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        randValue = intanciateWorld.generateRandVal();
       // intanciateWorld.instantiateWorld(randValue);

    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
       // RpcClientConnect(conn, randValue);
        SendRandomValueToClient(conn, randValue);

    }

    void SendRandomValueToClient(NetworkConnection conn, int[] randomValue)
    {
        // Find the object with MyClientBehaviour attached

        if (conn.identity == null) { Debug.LogError("CANN NULL") ;return; }
        MyClientBehaviour clientBehaviour = conn.identity.GetComponent<MyClientBehaviour>();

        if (clientBehaviour != null)
        {
            // Call the [ClientRpc] method on the specific client
            clientBehaviour.RpcReceiveRandomValue(randomValue);
        }
    }
    
  // [TargetRpc]
  // private void RpcClientConnect(NetworkConnectionToClient conn ,int [] randvalue)
  // {
  //     intanciateWorld.instantiateWorld(randvalue);
  // } 
    //public override void OnStartClient()
    //{
    //    base.OnStartClient();
//
    //    intanciateWorld.instantiateWorld(randValue);
    //}
}
