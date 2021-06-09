﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

	public Transform target;
	public Vector3 offset = new Vector3(0, 7.78f, -22.6f);
	private float smoothing = 2;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetPosition = target.position + offset;
		transform.position =  Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
		transform.LookAt(target);
	}
}
