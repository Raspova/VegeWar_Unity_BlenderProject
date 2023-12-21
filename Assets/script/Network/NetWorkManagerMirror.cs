using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
public class NetWorkManagerMirror :  Mirror.NetworkManager
{
    // Start is called before the first frame update

    int[] randValue;
    [SerializeField]
    private MyClientBehaviour clientBehaviour;
    //IntanciateWorld intanciateWorld ;
    public override void Start()
    {
        base.Start();
        //clientBehaviour = FindObjectOfType<MyClientBehaviour>();
   //     intanciateWorld = FindObjectOfType<IntanciateWorld>();
//
//        if (intanciateWorld == null)
//        {
//            Debug.LogError("IntanciateWorld component not found in the scene.");
//        }
//        if (clientBehaviour == null)
//        {
//            Debug.LogError("MyClientBehaviour component not found in the scene.");
//        }
    }

    public override void OnStartServer()
    {
      //  base.OnStartServer();
      //  randValue = intanciateWorld.generateRandVal();
      //  clientBehaviour.randvalueSet(randValue);
    }
    //public override void OnStartClient()
    //{
    //    base.OnStartClient();
    //    if (randValue == null)
    //    {
    //        Debug.LogError("randValue not set");
    //    }
    //    else
    //    {
    //        Debug.Log("randValue set");
    //        Debug.Log("Random   " + string.Join(", ", randValue));
    //    }
    //    intanciateWorld.instantiateWorld(randValue);
    //}
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
      //  base.OnServerConnect(conn);
      //  Debug.Log("Client connected to server: " + conn.connectionId + (conn));//+
        //clientBehaviour.printrand();
        //conn.identity.GetComponent<MyClientBehaviour>().randvalueSet(randValue);
       
      
    }
 
}
