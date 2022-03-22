using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    [HideInInspector]
    public RingCreation ringCreation;

    void Awake(){
        ringCreation = FindObjectOfType<RingCreation>();
    }

    void Start(){
        ringCreation.CreateRing();
    }

    
}
