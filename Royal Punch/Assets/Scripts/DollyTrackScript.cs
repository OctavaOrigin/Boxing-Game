using UnityEngine;
using UnityEngine.Playables;

public class DollyTrackScript : MonoBehaviour
{
    PlayableDirector playableDirector;

    void Awake(){
        playableDirector = GetComponent<PlayableDirector>();
    }

    public void PlayWin(){
        playableDirector.Play();
    }
}
