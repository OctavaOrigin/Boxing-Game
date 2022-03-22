using UnityEngine;

public class PullPlayer : MonoBehaviour
{
    GameObject ObjectToPull;
    private bool pull = false;
    [SerializeField] float pullingForce;

    public void PullPlayerEnable(GameObject objectToPull){
        pull = true;
        ObjectToPull = objectToPull;
    }

    public void PullPlayerDisable(){
        pull = false;
        ObjectToPull = null;
    }

    void FixedUpdate(){
        if (pull){
            ObjectToPull.transform.position += GetPullDirection();
        }
    }

    private Vector3 GetPullDirection(){
        Vector3 direction = transform.position - ObjectToPull.transform.position;
        Debug.DrawRay(ObjectToPull.transform.position, ObjectToPull.transform.position + direction * 10);
        direction = direction.normalized;
        return direction * pullingForce * Time.deltaTime;
    }
}
