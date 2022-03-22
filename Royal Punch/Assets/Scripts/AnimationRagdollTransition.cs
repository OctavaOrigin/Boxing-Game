using UnityEngine;
[RequireComponent(typeof(Animator))]

public class AnimationRagdollTransition : MonoBehaviour
{
    [SerializeField] Rigidbody []rigidBodies;
    Animator animator;
    private bool ragdollon = false;

    void Awake(){
        animator = GetComponent<Animator>();
    }

    public void SwitchToAnimation(){
        animator.enabled = true;
        foreach(Rigidbody rb in rigidBodies){
            rb.isKinematic = true;
        }
        ragdollon = false;
    }

    public void SwitchToRagdoll(){
        animator.enabled = false;
        foreach(Rigidbody rb in rigidBodies){
            rb.isKinematic = false;
        }
        ragdollon = true;
    }

    public bool RagdollOn(){
        return ragdollon;
    }
}
