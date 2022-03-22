using UnityEngine;

public class SceneScript : MonoBehaviour
{
    [SerializeField] PlayerController Player;
    [SerializeField] EnemyController Enemy;
    EnemyAnimationEventHandler enemyAnimationEventHandler;
    Animator animator;

    float bossStompDistance;

    void Awake(){
        Player = FindObjectOfType<PlayerController>();
        Enemy = FindObjectOfType<EnemyController>();
        enemyAnimationEventHandler = FindObjectOfType<EnemyAnimationEventHandler>();
        animator = GetComponent<Animator>();
    }

    void Start(){
        bossStompDistance = Enemy.bossStompDistance;
        enemyAnimationEventHandler.stompStarted += RedDangerZoneCircleAnimation;
        enemyAnimationEventHandler.stompPerformed += DisableRedDangerZone;
        enemyAnimationEventHandler.jumpPunchStart += RedDangerZoneCircleAnimation;
        enemyAnimationEventHandler.jumpPunchSecondHitPerformed += DisableRedDangerZone;
        enemyAnimationEventHandler.jumpStompStarted += RedDangerZoneCircleAnimation;
        enemyAnimationEventHandler.jumpStompPerformed += DisableRedDangerZone;
    }

    public bool EnemyStompReachPlayer(){
        Vector2 playerPos = new Vector2(Player.transform.position.x, Player.transform.position.z);
        Vector2 enemyPos = new Vector2(Enemy.transform.position.x, Enemy.transform.position.z);
        float dist = Vector2.Distance(playerPos, enemyPos);

        if (dist < bossStompDistance){
            return true;
        }
        return false;
    }

    private void RedDangerZoneCircleAnimation(){
        animator.Play("StompDangerZone");
    }

    private void DisableRedDangerZone(){
        animator.Play("New State");
    }

    
}
