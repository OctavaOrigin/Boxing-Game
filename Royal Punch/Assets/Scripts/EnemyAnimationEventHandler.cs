using UnityEngine;

public class EnemyAnimationEventHandler : MonoBehaviour
{
    public delegate void Action();
    public event Action stompPerformed;
    public event Action stompStarted;
    public event Action endAnimation;
    public event Action upperPunchChargeStart;
    public event Action upperPunchRelease;
    public event Action jumpPunchStart;
    public event Action jumpStartSecondHit;
    public event Action jumpPunchFirstHitPerformed;
    public event Action jumpPunchSecondHitPerformed;
    public event Action jumpStompStarted;
    public event Action jumpStompPerformed;

    void AnimationStompEnded() 
    {
        stompPerformed?.Invoke();
    }

    void AnimationStompStarted(){
        stompStarted?.Invoke();
    }

    void AnimationEndBossAnimation(){
        endAnimation?.Invoke();
    }

    void AnimationUpperPunchChargeStart(){
        upperPunchChargeStart?.Invoke();
    }

    void AnimationUpperPunchRelease(){
        upperPunchRelease?.Invoke();
    }

    void AnimationJumpPunchStarted(){
        jumpPunchStart?.Invoke();
    }

    void AnimationJumpPunchSecondHit(){
        jumpStartSecondHit?.Invoke();
    }

    void AnimationJumpPunchFirstHitPerformed(){
        jumpPunchFirstHitPerformed?.Invoke();
    }

    void AnimationJumpPunchSecondHitPerformed(){
        jumpPunchSecondHitPerformed?.Invoke();
    }

    void AnimationJumpStompStarted(){
        jumpStompStarted?.Invoke();
    }

    void AnimationJumpStompPerformed(){
        jumpStompPerformed?.Invoke();
    }
}
