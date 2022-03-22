using System.Collections;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    EnemyController controller;
    Animator animator;
    int bossSmashAnimation = 0;
    public bool doingBossAnim = false;
    string animationBool = "";
    EnemyAnimationEventHandler enemyAnimationEventHandler;

    Coroutine stopAnimCoroutine;

    void Awake(){
        animator = GetComponent<Animator>();
        controller = GetComponent<EnemyController>();
        enemyAnimationEventHandler = FindObjectOfType<EnemyAnimationEventHandler>();
    }

    void Start(){
        enemyAnimationEventHandler.stompStarted += StompStarted;
        enemyAnimationEventHandler.endAnimation += AEndBossAnimation;
        enemyAnimationEventHandler.upperPunchChargeStart += UpperPunchStart;
        enemyAnimationEventHandler.jumpPunchStart += JumpPunchStart;
        enemyAnimationEventHandler.jumpStartSecondHit += JumpPunchSecondHit;
        enemyAnimationEventHandler.jumpStompStarted += JumpStompStarted;
    }

    //event handler functions

    private void StompStarted(){
        StopAnimation(3);
    }

    private void AEndBossAnimation(){
        EndBossAnimation();
    }

    private void UpperPunchStart(){
        StopAnimation(3);
    }

    private void JumpPunchStart(){
        StopAnimation(3);
    }

    private void JumpPunchSecondHit(){
        StopAnimation(.5f);
    }

    private void JumpStompStarted(){
        StopAnimation(3);
    }

    //end of event handler functions

    public void CancelStoppedAnimation(){
        if (stopAnimCoroutine != null){
            StopCoroutine(stopAnimCoroutine);
            StartAnimation();
        }
    }

    public void AttackPlayer(){
        animator.SetBool("simpleAttack", true);
    }

    public void StopAttackingPlayer(){
        animator.SetBool("simpleAttack", false);
    }

    public void StopAnimation(float seconds){
        animator.speed = 0;
        stopAnimCoroutine = StartCoroutine(WaitBossAnimation(seconds));
    }

    public void StartAnimation(){
        animator.speed = 1;
    }

    IEnumerator WaitBossAnimation(float seconds){
        yield return new WaitForSeconds(seconds);
        StartAnimation();
        Debug.Log("Start animation after coroutine");
    }

    public void EndBossAnimation(){
        animator.SetBool("tired", true);
        DoTiredAnimation();
    }

    private void DoTiredAnimation(){
        StartCoroutine(WaitForTiredBossAnimation(controller.tiredBossTime));
    }

    IEnumerator WaitForTiredBossAnimation(float seconds){
        yield return new WaitForSeconds(seconds);
        doingBossAnim = false;
        animator.SetBool("tired", false);
        animator.SetBool(animationBool, false);
    }

    public void DoBossAnimation(){
        doingBossAnim = true;
        bossSmashAnimation++;
        if (bossSmashAnimation > 5) bossSmashAnimation = 1;
        switch (bossSmashAnimation){
            case 1: {
                animationBool = "stomp";
                BossStomp();
                return;
            }
            case 2: {
                animationBool = "upperHead";
                UpperHead();
                return;
            }
            case 3: {
                animationBool = "superPunch";
                SuperPunch();
                return;
            }
            case 4: {
                animationBool = "jumpDoublePunch";
                JumpDoublePunch();
                return;
            }
            case 5: {
                animationBool = "jumpStamp";
                JumpStamp();
                return;
            }
            
        }
    }

    private void BossStomp(){
        animator.SetBool(animationBool, true);
    }

    private void UpperHead(){
        animator.SetBool(animationBool, true);
    }

    private void SuperPunch(){
        animator.SetBool(animationBool, true);
    }

    private void JumpDoublePunch(){
        animator.SetBool(animationBool, true);
    }

    private void JumpStamp(){
        animator.SetBool(animationBool, true);
    }
}
