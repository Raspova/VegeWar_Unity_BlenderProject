using UnityEngine;
using Mirror;

public class MyClientBehaviour : NetworkBehaviour
{
    [ClientRpc]
    public void RpcReceiveRandomValue(int[] randomValue)
    {
        Debug.Log("Received random values on the client: " + string.Join(", ", randomValue));
    }
}
