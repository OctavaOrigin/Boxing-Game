using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Health
{
    EnemyAnimationEventHandler enemyAnimationEventHandler;
    SceneScript sceneScript;
    [HideInInspector] public EnemyController EnemyTarget;
    AnimationController animationController;
    CharacterController controller;
    Vector2 movementDirection;
    View view;
    Bounds ringBounds;
    CapsuleCollider capsuleCollider;
    Transform body;
    public ControlCharacterPosition controlCharacterPosition;
    public EnemyFling enemyFling;
    public AnimationRagdollTransition animRagdollTrans;
    
    public float speed;
    public Vector2 playerExtend;
    private float turningSpeed = 10f;
    private float newYBody = 0f;
    private Vector2 currentMovementDir;
    [SerializeField] bool standUp;

    [SerializeField] bool die = false;
    [SerializeField] GameObject spine;
    AllignRagdollAndCharacterPosition alligner;
    private bool stopMoving;
    
    WinScript winScript;

    void Awake(){
        winScript = FindObjectOfType<WinScript>();
        controlCharacterPosition = GetComponent<ControlCharacterPosition>();
        view = FindObjectOfType<View>();
        animationController = GetComponent<AnimationController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        controller = new CharacterController();
        body = transform.Find("Body");
        EnemyTarget = FindObjectOfType<EnemyController>();
        enemyFling = GetComponent<EnemyFling>();
        animRagdollTrans = GetComponent<AnimationRagdollTransition>();
        movementDirection = new Vector2();
        currentMovementDir = new Vector2();

        playerExtend = new Vector2(capsuleCollider.bounds.extents.x, capsuleCollider.bounds.extents.z);

        controller.Player.Enable();
        controller.Player.Move.performed += Move;
        controller.Player.Move.canceled += MoveStop;

        enemyAnimationEventHandler = FindObjectOfType<EnemyAnimationEventHandler>();
        sceneScript = FindObjectOfType<SceneScript>();
        alligner = FindObjectOfType<AllignRagdollAndCharacterPosition>();
        alligner.readyToStandUp += StandUp;
    }

    void Start(){
        ringBounds = view.ringCreation.GetRingSize();
        animRagdollTrans.SwitchToAnimation();
        capsuleCollider.enabled = false;
        stopMoving = false;
        enemyAnimationEventHandler.stompPerformed += EnemyStompPlayer;
        enemyAnimationEventHandler.jumpStompPerformed += EnemyStompPlayer;
        enemyAnimationEventHandler.jumpPunchFirstHitPerformed += EnemyStompPlayer;
        enemyAnimationEventHandler.jumpPunchSecondHitPerformed += EnemyStompPlayer;
    }

    void FixedUpdate(){

        if (stopMoving) return;
        

        LookAtEnemy();
        HandleMovement();
        
        controlCharacterPosition.KeepPlayerFromEnemy();
        TurnCharacterAndCam();

        if (controlCharacterPosition.AttackDistanceReached()){
            Attack();
        }else{
            StopAttacking();
        }
    }

    public void Move(InputAction.CallbackContext context){
        movementDirection = context.ReadValue<Vector2>();
    }
    private void TurnCharacterAndCam(){
        Vector3 angles = body.transform.localEulerAngles;
        newYBody = 90f * movementDirection.x;
        newYBody = Mathf.MoveTowardsAngle(angles.y, newYBody, turningSpeed);
        body.transform.localEulerAngles = new Vector3(angles.x, newYBody, angles.z);
        
        newYBody = 90f * movementDirection.x;
        currentMovementDir.y = angles.y;
        if (angles.y > 90) currentMovementDir.y -= 360;
        currentMovementDir.x = Mathf.MoveTowards(currentMovementDir.y, newYBody, turningSpeed);
        currentMovementDir.x = currentMovementDir.x / 90f;
        currentMovementDir.y = movementDirection.y;
        animationController.HandleMovementAnimation(currentMovementDir);
    }

    private void MoveStop(InputAction.CallbackContext context){
        movementDirection = Vector3.zero;
    }

    private void HandleMovement(){
        Vector3 target = new Vector3(movementDirection.x, 0, movementDirection.y);
        
        transform.Translate(target * Time.fixedDeltaTime * speed);
    }

    private void LookAtEnemy(){
        transform.LookAt(EnemyTarget.transform.position);
        Debug.DrawRay(transform.position, EnemyTarget.transform.position - transform.position, Color.red);
        //Debug.Log(spine.transform.position);
    }

    public void Attack(){
        animationController.SimpleAttack();
    }

    public void HitEnemy(){
        EnemyTarget.TakeDamage(10);
        EnemyTarget.enemyFling.Fling();
    }

    public void StopAttacking(){
        animationController.StopAllAttacks();
    }

    private void EnemyStompPlayer(){
        if (sceneScript.EnemyStompReachPlayer()){
            animRagdollTrans.SwitchToRagdoll();
            spine.GetComponent<Rigidbody>().velocity = transform.forward * -1 * 30;
            TakeDamage(30);
            StartCoroutine(HugeDamageDone());
        }
    }

    public void EnemyUpperHitPlayer(){
        animRagdollTrans.SwitchToRagdoll();
        spine.GetComponent<Rigidbody>().velocity = (transform.forward * -1 * 20) + transform.up * 50;
        StartCoroutine(HugeDamageDone());
    }

    //event
    private void StandUp(){
        animRagdollTrans.SwitchToAnimation();
        animationController.GetUp();
    }

    IEnumerator HugeDamageDone(){
        yield return new WaitForSeconds(2);
        alligner.ChangePosition();
    }

    public void Win(){
        Debug.Log("Win");
        winScript.Win();
        stopMoving = true;
    }

    public override void Death()
    {
        EnemyTarget.Win();
        if (!animRagdollTrans.RagdollOn()){
            animRagdollTrans.SwitchToRagdoll();
            spine.GetComponent<Rigidbody>().velocity = transform.forward * -1 * 30;
        }
    }
}
