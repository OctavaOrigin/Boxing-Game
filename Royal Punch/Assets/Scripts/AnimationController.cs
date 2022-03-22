using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animator;
    Animation currentAnimation;

    void Awake(){
        animator = GetComponent<Animator>();
    }
    public void HandleMovementAnimation(Vector2 direction){
        animator.SetFloat("VelocityX", direction.x);
        animator.SetFloat("VelocityZ", direction.y);
    }

    public void SimpleAttack(){
        animator.SetBool("isAttacking", true);
    }

    public void StopAllAttacks(){
        animator.SetBool("isAttacking", false);
    }

    public void GetUp(){
        animator.Play("StandUp");
        animator.SetBool("Standup", true);
    }

    public void GetUpComplete(){
        animator.SetBool("Standup", false);
    }

    public void PlayWinAnimations(){
        animator.Play("WinHandsUp");
    }
}
