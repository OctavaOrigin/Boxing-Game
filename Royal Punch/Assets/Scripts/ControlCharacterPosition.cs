using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCharacterPosition : MonoBehaviour
{
    EnemyController EnemyTarget;
    Bounds ringBounds;
    View view;
    PlayerController controller;
    public float fightDistance;

    void Awake(){
        view = FindObjectOfType<View>();
        controller = GetComponent<PlayerController>();
    }

    void Start(){
        ringBounds = view.ringCreation.GetRingSize();
        EnemyTarget = controller.EnemyTarget;
        fightDistance = controller.playerExtend.x * 3;
    }

    public void KeepPlayerFromEnemy(){

        Vector3 vector = EnemyTarget.transform.position - transform.position;
        float angle = Vector3.Angle(vector, Vector3.right);
        if (transform.position.z < 0) angle = 360 - angle;
        angle *= Mathf.Deg2Rad;

        float newX = -controller.playerExtend.x * 2 * Mathf.Cos(angle);
        float newZ = controller.playerExtend.x * 2 * Mathf.Sin(angle);

        float sign = Mathf.Sign(transform.position.x);
        float xPos = Mathf.Clamp(Mathf.Abs(transform.position.x), Mathf.Abs(newX), ringBounds.max.x - controller.playerExtend.x);
        xPos *= sign;

        sign = Mathf.Sign(transform.position.z);
        float zPos = Mathf.Clamp(Mathf.Abs(transform.position.z), Mathf.Abs(newZ), ringBounds.max.z - controller.playerExtend.y);
        zPos *= sign;

        transform.position = new Vector3(xPos, transform.position.y, zPos);
    }

    public bool AttackDistanceReached(){
        return Vector3.Distance(EnemyTarget.transform.position, transform.position) < fightDistance;
    }

    public float AttackRadius(){
        return fightDistance;
    }

    public bool OnTheGround(){
        
        return true;
    }
}
