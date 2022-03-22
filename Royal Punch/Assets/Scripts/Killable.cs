using UnityEngine;

public class Killable : MonoBehaviour
{
    Health health;

    void Awake(){
        
    }

    void Strat(){
        health = GetComponent<Health>();
    }

    public void DealDamage(float damageAmount){
        health.TakeDamage(damageAmount);
        if (health.DeathHealth()){
            Death();
        }
    }

    public virtual void Death(){
        
    }
}
