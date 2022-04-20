using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* GUIDE TO NEW UNIT
    CREATE OWN AVATAR, SET ANiMATION LOOP
    ADD TO UNIT TAG/LAYER TO THE PREFAB
    set animator in prefab 
    PUT HEALBAR AT THE TOP OF THE HIEARKI
*/

public class UnitsManager : MonoBehaviour
{
    [SerializeField] GameObject AvocadoUnit;
    [SerializeField] int nAvocado = 25; 
    
    [SerializeField] GameObject KakaRotoUnit;
    [SerializeField] int nKakarot = 50;
    
    [SerializeField] RectTransform boxSelectionGaph;
    static public List<PlayerRts> allUnits = new List<PlayerRts>();
    static public List<PlayerRts> selectUnits = new List<PlayerRts>();
    Rect boxSelection;
    int UnitLayer;
    int FloorLayer;

    void initNUnits(GameObject unit, int n ,Vector3 pos, int teamId)
    {
        GameObject buff;
        
        for (int i = 0; i < n; i++) {
            buff = Instantiate(unit, pos, Quaternion.identity);
            pos.x += 1;
            buff.GetComponentInChildren<PlayerRts>().TeamId = teamId;
        }
    }
    void Start()
    {
        UnitLayer = LayerMask.GetMask("Units");
        FloorLayer = LayerMask.GetMask("Floor");
    
        initNUnits(AvocadoUnit, nAvocado, new Vector3(1,1,1), 0);
        initNUnits(KakaRotoUnit, nKakarot, new Vector3(35,1,35), 1);
        
        DrawSelectionBox();

    }

    void deselectAll()
    {
        foreach (PlayerRts unit in selectUnits)
            unit.changeSelect();
        selectUnits.Clear();
    }
    
    void attakTargetSelectedUnit(PlayerRts target)
    {
        foreach (PlayerRts unit in selectUnits)
            if (unit.TeamId != target.TeamId) // CHECK ALLY
                unit.setTarget(target);
    }

    void moveSelletedUnits(Vector3 v)
    {
        int offset = (int) Mathf.Sqrt(selectUnits.Count - 1);
        float xmod = 0;
        float ymod = 0;
        Vector3 voffset = Vector3.zero;

        foreach (PlayerRts unit in selectUnits) {
            voffset.Set(v.x + xmod, v.y, v.z + ymod);
            unit.moveTo(voffset);
            if (xmod == offset) {
                xmod = 0;
                ymod++;
            } else xmod++;
        }
    }

    private Vector2 BoxStartingPos = Vector2.zero;
    private Vector2 BoxEndPos = Vector2.zero;

    void DrawSelectionBox()
    {
        boxSelectionGaph.position = (BoxStartingPos + BoxEndPos) / 2;
        boxSelectionGaph.sizeDelta = new Vector2(
        Mathf.Abs(BoxStartingPos.x - BoxEndPos.x),
        Mathf.Abs(BoxStartingPos.y - BoxEndPos.y));
    }

    RaycastHit hit;
    Ray ray;
    //Collider[] colliderBuff;
    bool inRange;
  
    static public void  unitDied(PlayerRts deadOne)
    {
        foreach (PlayerRts player in allUnits)
            if (player.target == deadOne)
                player.targetDead();
        Destroy(deadOne.gameObject);
    }

    //List<PlayerRts> enemyInView = null;
    void PlayersLogic()
    {
         
        foreach (PlayerRts player in allUnits) {
            
            if (player.state == PlayerRts.state_t.WALLKING) {
                player.CheckArrived();
                return;
            }
            p = player.ClosestEnemy();
            if (player.state == PlayerRts.state_t.IDLLE && p)
                player.setTarget(p); // STAT TO ATTAK ; ALLERT EMOTE
            
                // && Vector3.Distance(player.transform.position, enemy.transform.position) < 2
            if (player.state == PlayerRts.state_t.ATTAKING) {
                inRange = false;
                foreach (PlayerRts enemy in player._enemyInView)
                    if (enemy == player.target
                   ) {
                        inRange = true;
                        if (player.AttackTimer(Time.deltaTime))
                            if (player.attackTarget()<= 0) {
                                unitDied(player.target);
                                PlayersLogic();
                                return;
                            }
                    }
                if (!inRange && Vector3.Distance(player.target.transform.position, player.myDestinationPos) > player._range / 2) // check moving player;
                    player.runTo(player.target.transform.position);
            }
        }
    }/*
    void PlayersLogic()
    {
        foreach (PlayerRts players in allUnits) {
            if (players.state == PlayerRts.state_t.WALLKING)
                players.CheckArrived();
            if (players.state == PlayerRts.state_t.WALLKING || players.state == PlayerRts.state_t.ATTAKING) {
                colliderBuff = Physics.OverlapSphere(players.transform.position, players._detectionRange, EnemyLayer);
                foreach (Collider enemi in colliderBuff)
                    if (enemi.gameObject.GetComponent<PlayerRts>().state == PlayerRts.state_t.IDLLE)
                        enemi.gameObject.GetComponent<PlayerRts>().setTarget(players);
            }
                
            else if (players.state == PlayerRts.state_t.ATTAKING) {
                inRange = false;
                colliderBuff = Physics.OverlapSphere(players.transform.position, players._range, EnemyLayer);
                foreach (Collider enemi in colliderBuff)
                    if (enemi.gameObject.GetInstanceID() == players.target.ID){
                        inRange = true;
                        if (players.AttackTimer(Time.deltaTime)) {
                            if (players.attackTarget()<= 0) {
                                unitDied(players.target);
                                PlayersLogic();
                                return;
                            }
                        }     
                    }
                if (!inRange && Vector3.Distance(players.target.transform.position, players.targetPos) > players._range / 2)
                    players.runTo(players.target.transform.position);
            }
        }
    }*/


    PlayerRts p;
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 250, UnitLayer)) {
                p = hit.collider.gameObject.GetComponent<PlayerRts>();
                if (!Input.GetKey(KeyCode.LeftShift))
                    deselectAll();
                p.changeSelect();
                selectUnits.Add(p);
            } else if (!Input.GetKey(KeyCode.LeftShift))
                deselectAll();
            BoxEndPos = Input.mousePosition;
            BoxStartingPos = Input.mousePosition;;
        } 

        if (Input.GetMouseButton(0)) {
            BoxEndPos = Input.mousePosition;
            DrawSelectionBox();
        }
        if (Input.GetMouseButtonUp(0)) {
            boxSelection.xMin = BoxEndPos.x < BoxStartingPos.x  ? BoxEndPos.x : BoxStartingPos.x;
            boxSelection.xMax = BoxEndPos.x >= BoxStartingPos.x  ? BoxEndPos.x : BoxStartingPos.x;
            boxSelection.yMin = BoxEndPos.y < BoxStartingPos.y  ? BoxEndPos.y : BoxStartingPos.y;
            boxSelection.yMax = BoxEndPos.y >= BoxStartingPos.y  ? BoxEndPos.y : BoxStartingPos.y;
            foreach (PlayerRts players in allUnits)
                if (boxSelection.Contains(Camera.main.WorldToScreenPoint(players.gameObject.transform.position)))
                    if (!selectUnits.Contains(players)) {// if my uniit / filer after 
                        players.changeSelect();
                        selectUnits.Add(players);
                    }
                        
            BoxStartingPos = Vector2.zero;
            BoxEndPos = Vector2.zero;
            DrawSelectionBox();
        }
        if (Input.GetMouseButtonDown(1)) {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 250, UnitLayer)){
                p = hit.collider.gameObject.GetComponent<PlayerRts>();
                attakTargetSelectedUnit(p);
            } // ADD LAYER TERAIN
            else if (Physics.Raycast(ray, out hit, 250, FloorLayer)) // ADD LAYER TERAIN
                moveSelletedUnits(hit.point);
        }
        PlayersLogic();     
    }
}
