using UnityEngine;

public class AllignRagdollAndCharacterPosition : MonoBehaviour
{

    [SerializeField] GameObject Player;
    [SerializeField] GameObject Body;
    [SerializeField] GameObject Spine;
    [SerializeField] GameObject Armature;

    [SerializeField] GameObject Follower;
    [SerializeField] GameObject Hips;

    [SerializeField] GameObject Cube1;
    [SerializeField] GameObject Cube2;
    [SerializeField] GameObject Cube3;
    private Vector3 smallVector;
    
    public bool changePosition;
    private bool allowed;
    Vector3 targetPostion;
    Vector3 velocity;

    public delegate void Action();
    public event Action readyToStandUp;

    void Start(){
        changePosition = false;
        allowed = false;
        smallVector = new Vector3(0.02f, 0.02f, 0.02f);
    }

    void Update()
    {
        if (changePosition) {
            changePosition = false;
            Hips.transform.parent = null;
            Cube1.transform.parent = null;
            Cube2.transform.parent = null;
            Cube3.transform.parent = null;
            allowed = true;
            targetPostion =  new Vector3(Follower.transform.position.x, Player.transform.position.y, Follower.transform.position.z);
        }
        if (allowed)
            Move();
        
    }

    private void Move(){
        Player.transform.position = Vector3.SmoothDamp(Player.transform.position, targetPostion, ref velocity, 0.5f);

        if (velocity.magnitude < smallVector.magnitude){
            Hips.transform.parent = Armature.transform;
            Cube1.transform.parent = Body.transform;
            Cube2.transform.parent = Body.transform;
            Cube3.transform.parent = Body.transform;
            allowed = false;
            ReadyToStandUp();
        }
    }

    public void ChangePosition(){
        changePosition = true;
    }

    private void ReadyToStandUp(){
        readyToStandUp?.Invoke();
    }
}
