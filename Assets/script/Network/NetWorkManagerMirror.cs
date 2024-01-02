using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
public class NetWorkManagerMirror : NetworkManager
{
    [SerializeField] private GameObject workerPrefab;
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        GameObject worker = null;

        for (int i = 0; i < 3; i++) {
            if (numPlayers == 1)
                worker = Instantiate(workerPrefab, new Vector3(20 + i , 1 , 20 + i) , Quaternion.identity);
            else if (numPlayers == 2)
                worker = Instantiate(workerPrefab, new Vector3(130 + i , 1 , 130 + i) , Quaternion.identity);
            if (worker != null)
                NetworkServer.Spawn(worker , conn);
        }
    }
}
