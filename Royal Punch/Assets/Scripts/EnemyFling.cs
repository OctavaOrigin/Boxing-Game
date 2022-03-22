using UnityEngine;
using UnityEngine.Events;

public class EnemyFling : MonoBehaviour
{
    [SerializeField] GameObject controller;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject origin;

    [SerializeField] bool fling;
    [SerializeField] float flingingSpeed;
    
    private float flingAmount = 30f;
    private float flingCount = 0f;
    private bool doFling = false;
    private int flingSign = 1;

    Vector3 direction;

    public void Fling(){
        direction = controller.transform.position - enemy.transform.position;
        direction = new Vector3(direction.x, 0, direction.z);
        doFling = true;
        flingSign = 1;
        if (flingCount < 0) flingCount = 0;
    }

    void FixedUpdate(){
        
        if (fling){
            fling = false;
            Fling();
        }

        if (!doFling) return;

        if (SmallAngle() && flingSign == -1)
            doFling = false;

        if (flingCount > 30)
            flingSign = -1;


        if (flingSign == 1){
            Vector3 flingTowards = controller.transform.position + direction;
            float newX = flingTowards.x * Mathf.Cos(-90 * Mathf.Deg2Rad) - flingTowards.z * Mathf.Sin(-90 * Mathf.Deg2Rad);
            float newZ = flingTowards.z * Mathf.Cos(-90 * Mathf.Deg2Rad) + flingTowards.x * Mathf.Sin(-90 * Mathf.Deg2Rad);
            flingTowards = new Vector3(newX, flingTowards.y, newZ);
            controller.transform.RotateAround(controller.transform.position, flingTowards, Time.deltaTime * flingingSpeed);
            flingCount += Time.deltaTime * flingingSpeed * flingSign;
        }else{
            controller.transform.rotation = Quaternion.RotateTowards(controller.transform.rotation, origin.transform.rotation, Time.deltaTime * flingingSpeed);
            flingCount += Time.deltaTime * flingingSpeed * flingSign;
        }
        
    }

    private bool SmallAngle(){
        float angle = Quaternion.Angle(controller.transform.rotation, origin.transform.rotation);
        return angle < 1;
    }
}
