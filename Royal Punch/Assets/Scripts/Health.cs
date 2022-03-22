using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float healthAmount;

    public void TakeDamage(float damageAmount){
        healthAmount -= damageAmount;
        if (healthAmount <= 0){
            Death();
        }
    }

    public bool DeathHealth(){
        return healthAmount <= 0;
    }

    public virtual void Death(){

    }
}
