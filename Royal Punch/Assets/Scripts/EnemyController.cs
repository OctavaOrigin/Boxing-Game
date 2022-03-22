using UnityEngine;
using System.Threading.Tasks;

public class EnemyController : Health
{
    bool distanceReachedAndInAttackRadius = false;
    [HideInInspector]
    public EnemyAnimationController animationController;
    [HideInInspector]
    public EnemyAnimationEventHandler enemyAnimationEventHandler;
    PlayerController player;
    [SerializeField] float turningSpeed = 2;
    public enum state{
        attack,
        idle
    }
    state enemyState;

    [SerializeField] int attackAngle;
    float bossHitTimer = 0;
    bool stopCount = false;
    bool doingBossAnim = false;
    [SerializeField] public float tiredBossTime;
    [HideInInspector]
    public EnemyFling enemyFling;

    public float bossStompDistance;
    [HideInInspector]
    public PullPlayer pullPlayer;

    public bool StopMoving;

    void Awake(){
        player = FindObjectOfType<PlayerController>();
        animationController = GetComponent<EnemyAnimationController>();
        enemyFling = GetComponent<EnemyFling>();
        bossStompDistance = 9f;
        enemyAnimationEventHandler = FindObjectOfType<EnemyAnimationEventHandler>();
        pullPlayer = GetComponent<PullPlayer>();
    }

    void Start(){
        enemyState = state.idle;
        enemyAnimationEventHandler.upperPunchChargeStart += UpperPunchChargeStart;
        StopMoving = false;
    }

    void FixedUpdate(){

        if (StopMoving) return;

        distanceReachedAndInAttackRadius = player.controlCharacterPosition.AttackDistanceReached() && InAttackRadius();
        if (animationController.doingBossAnim) return;
        CountBossSuperHits();
        LookAtEnemy();
        if (distanceReachedAndInAttackRadius){
            bossHitTimer -= Time.fixedDeltaTime;
            if (enemyState != state.attack){
                enemyState = state.attack;
                ReadyToAttack();
            }
        }else{
            if (enemyState != state.idle){
                enemyState = state.idle;
                StopSimpleAttack();
            }
        }
    }

    private void LookAtEnemy(){
        float targetAngle = Quaternion.LookRotation(player.transform.position).eulerAngles.y;
        float currentAngle = transform.localEulerAngles.y;

        float newY = Mathf.MoveTowardsAngle(currentAngle, targetAngle, turningSpeed);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, newY, transform.localEulerAngles.z);
    }

    private void ReadyToAttack(){
        animationController.AttackPlayer();
    }

    private void StopSimpleAttack(){
        animationController.StopAttackingPlayer();
    }

    private bool InAttackRadius(){
        float direction = transform.rotation.y;
        Debug.DrawRay(transform.position, transform.forward * player.controlCharacterPosition.AttackRadius());

        float newXPlus = transform.forward.x * Mathf.Cos(attackAngle * Mathf.Deg2Rad) - transform.forward.z * Mathf.Sin(attackAngle * Mathf.Deg2Rad);
        float newZPlus = transform.forward.z * Mathf.Cos(attackAngle * Mathf.Deg2Rad) + transform.forward.x * Mathf.Sin(attackAngle * Mathf.Deg2Rad);

        float newXMinus = transform.forward.x * Mathf.Cos(-attackAngle * Mathf.Deg2Rad) - transform.forward.z * Mathf.Sin(-attackAngle * Mathf.Deg2Rad);
        float newZMinus = transform.forward.z * Mathf.Cos(-attackAngle * Mathf.Deg2Rad) + transform.forward.x * Mathf.Sin(-attackAngle * Mathf.Deg2Rad);

        Vector3 plusVector = new Vector3(newXPlus, 0, newZPlus).normalized;
        Vector3 minusVector = new Vector3(newXMinus, 0, newZMinus).normalized;

        Debug.DrawRay(transform.position, plusVector * player.controlCharacterPosition.AttackRadius());
        Debug.DrawRay(transform.position, minusVector * player.controlCharacterPosition.AttackRadius());
    
        Vector3 playerPosNorm = player.transform.position.normalized;
        playerPosNorm = new Vector3(playerPosNorm.x, 0, playerPosNorm.z);

        int angles = (int)Vector3.Angle(plusVector, playerPosNorm) + (int)Vector3.Angle(minusVector, playerPosNorm);

        return angles <= attackAngle*2 + 1;
    }

    private void CountBossSuperHits(){
        bossHitTimer += Time.fixedDeltaTime;
        if (bossHitTimer > 3){
            bossHitTimer = 0;
            animationController.DoBossAnimation();
        }
    }

    public void HitPlayer(){
        player.TakeDamage(20);
        player.enemyFling.Fling();
    }

    private async void UpperPunchChargeStart(){
        pullPlayer.PullPlayerEnable(player.gameObject);
        float time = 0;
        while (time < 3){
            time += Time.deltaTime;
            if (distanceReachedAndInAttackRadius){
                Debug.Log("stop pulling");
                pullPlayer.PullPlayerDisable();
                animationController.CancelStoppedAnimation();
                player.EnemyUpperHitPlayer();
                time = 3;
            }
            await Task.Yield();
        }
        pullPlayer.PullPlayerDisable();
    }

    public override void Death(){
        player.Win();
        Destroy(this.gameObject);
    }

    public void Win(){
        StopMoving = true;
        animationController.StopAttackingPlayer();
    }
}
