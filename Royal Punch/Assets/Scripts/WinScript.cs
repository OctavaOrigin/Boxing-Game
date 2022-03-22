using UnityEngine;

public class WinScript : MonoBehaviour
{
    AnimationController playerAnimController;
    DollyTrackScript dollyTrackScript;


    void Awake(){
        playerAnimController = FindObjectOfType<AnimationController>();
        dollyTrackScript = FindObjectOfType<DollyTrackScript>();
    }

    public void Win(){

        dollyTrackScript.gameObject.transform.parent = null;
        dollyTrackScript.PlayWin();

        playerAnimController.PlayWinAnimations();
        playerAnimController.StopAllAttacks();

    }
}
