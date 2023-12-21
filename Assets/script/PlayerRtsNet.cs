using System.Collections.Generic;
using UnityEngine;
using static PlayerRtsNet.state_t;    
using Mirror;


public class PlayerRtsNet : NetworkBehaviour
{
// STATS
    [SerializeField] private int _totalHealth;
    [SerializeField] private int _attack;
    [Range(0.1f, 5)]
    [SerializeField] private float _attack_time;
    public int _viewRange;
    public float _detectionRange;
  
    public float _range;
    private struct unitStats {
        public int totalHealth;
        public int health;
        public int attack;
        public float attack_time; // (in sec)
        public float range; // of attack
        public int viewRange;
        public float detectionRange; // Agro
    };
    unitStats stats;
//
    private UnityEngine.AI.NavMeshAgent navigator;
    //public Transform player_transform;
    private UnityEngine.Animator animator;
    private int running = Animator.StringToHash("running");
    private int triggerAttak = Animator.StringToHash("attack");
    private Outline outline;
    private  bool isSelected = false;
    public int TeamId {get; set;}
    public enum state_t {
        IDLLE,
        WALLKING,
        ALERT,
        ATTAKING,
    };

    [HideInInspector]
    public PlayerRtsNet target;
    public state_t state  {get; set;}

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject healthBarGui;
    int UnitLayer;
    //int EnemyLayerID;
    //int EnemyLayer;

    public Vector3 myDestinationPos;
    float timerAttack = 0f;
    //public int ID;    
    void Start()
    {
        UnitLayer = LayerMask.GetMask("Units");
        //EnemyLayerID = LayerMask.NameToLayer("EnemeyUnit");
        //EnemyLayer = LayerMask.GetMask("EnemeyUnit");

        navigator = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator =  GetComponentInChildren<UnityEngine.Animator>();
        outline = GetComponent<Outline>();
        UnitsManagerNet.allUnits.Add(this);
        outline.enabled =  false;
        state = IDLLE;
        myDestinationPos = transform.position;
        //ID = gameObject.GetInstanceID();
// STATS INIT
        stats.totalHealth = _totalHealth;
        stats.health = _totalHealth;
        stats.attack =_attack;
        stats.attack_time = _attack_time;
        stats.range = _range;
        stats.viewRange = _viewRange;
        stats.detectionRange = _detectionRange;
//
        healthBarGui.SetActive(false);
        healthBar.SetMaxHealth(stats.totalHealth);
    }

    public bool AttackTimer(float delta)
    {
        timerAttack += delta;
        if (timerAttack > stats.attack_time) {
            timerAttack  = 0;
            return true;
        }
        return false;
    }

    public PlayerRtsNet ClosestEnemy()
    {
        List<PlayerRtsNet > _enemyInView = enemyInView();
        float bestDistance = 9999.0f;
        PlayerRtsNet bestCollider = null;
    
        foreach (PlayerRtsNet enemy in _enemyInView)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
    
            if (distance < bestDistance
            &&  !enemy.isDead()) {
                bestDistance = distance;
                bestCollider = enemy;
            }
        }
        return bestCollider;
    } 
    [HideInInspector]
    public List<PlayerRtsNet> _enemyInView = new List<PlayerRtsNet>();
    
    public List<PlayerRtsNet> enemyInView()
    {
        Collider[] unitInView = Physics.OverlapSphere(transform.position, stats.viewRange, UnitLayer);
        PlayerRtsNet buff;

        _enemyInView.Clear();
        foreach (Collider unit in unitInView) {
            buff = unit.gameObject.GetComponent<PlayerRtsNet>();//Map
                if (buff.TeamId != TeamId) // or ALLY
                    _enemyInView.Add(buff);
        }
        return _enemyInView;
    }
    
    public void targetDead()
    {
        PlayerRtsNet nextTarget = ClosestEnemy();
        
        target = null;
        if (nextTarget != null)
            setTarget(nextTarget);
        else {
            state = IDLLE;
            animator.SetBool(running, false);
        }
    }

    public bool isDead()
    {
        return (stats.health <= 0);
    }

    public int takeDamage(int damage)
    {
        if (stats.health == stats.totalHealth)
            healthBarGui.SetActive(true); // false when heal 
        stats.health -= damage;
        healthBar.SetHealth(stats.health);
        return stats.health;
    }

    public int attackTarget()
    {
        animator.SetBool(running, false);
        animator.SetTrigger(triggerAttak);
        transform.LookAt(target.transform.position); // learp
        return target.takeDamage(stats.attack); // if neg level up;
        //;
    }

    public void runTo(Vector3 pos)
    {
        navigator.destination = pos;
        myDestinationPos = pos;
        animator.SetBool(running, true);
    }

    public void setTarget(PlayerRtsNet _target)
    {
        if (_target == null)
            return;
        target = _target;
        state = ATTAKING;
        runTo(target.transform.position);
    }

    public void CheckArrived()
    {
        if (!navigator.pathPending
        && navigator.remainingDistance <= navigator.stoppingDistance
        && (!navigator.hasPath || navigator.velocity.sqrMagnitude == 0f)) {
            animator.SetBool(running, false);
            state = IDLLE;
        }
    }

    public void changeSelect()
    {
        isSelected = !isSelected;
        outline.enabled = isSelected;
        if (stats.health != stats.totalHealth)
             healthBarGui.SetActive(true);
        else healthBarGui.SetActive(isSelected);
    }

    public void moveTo(Vector3  hitpoint)
    {
        state = WALLKING;
        runTo(hitpoint);
    }

    private void OnDestroy() {
        UnitsManagerNet.allUnits.Remove(this);
        if (UnitsManagerNet.selectUnits.Contains(this))
            UnitsManagerNet.selectUnits.Remove(this);
    }

}

