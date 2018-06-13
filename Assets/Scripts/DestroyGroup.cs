using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGroup : MonoBehaviour {

    public float DestroyInSeconds;
    
    // Use this for initialization
	void Start () {
        Destroy(gameObject, DestroyInSeconds);
	}
	
}
