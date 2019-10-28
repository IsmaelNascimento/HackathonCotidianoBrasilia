using UnityEngine;
using System.Collections;

public class movimentaCamera : MonoBehaviour {

    Vector3 offSet;
    public GameObject jogador;

	// Use this for initialization
	void Start () {
        offSet = transform.position - jogador.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = jogador.transform.position + offSet;
	}
}
