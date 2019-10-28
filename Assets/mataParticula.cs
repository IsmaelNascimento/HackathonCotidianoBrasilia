using UnityEngine;
using System.Collections;

public class mataParticula : MonoBehaviour {

	// Use this for initialization
	void Start () {
        InvokeRepeating("mataParticle", 1, 3.0f);
	}
	
	// Update is called once per frame
	void mataParticle()
    {
        Destroy(this.gameObject);
    }
}
