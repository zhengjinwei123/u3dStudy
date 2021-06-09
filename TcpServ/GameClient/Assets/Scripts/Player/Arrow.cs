using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

	public int speed = 5;
	private Rigidbody rgd;
	// Use this for initialization
	void Start () {
		rgd = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		rgd.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
	}
}
