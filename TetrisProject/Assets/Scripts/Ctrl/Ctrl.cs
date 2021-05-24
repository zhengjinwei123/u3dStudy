using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctrl : MonoBehaviour {

	[HideInInspector]
	public Model model;

	[HideInInspector]
	public View view;

	private FSMSystem fsm;

	[HideInInspector]
	public CameraManager cameraManager;

	[HideInInspector]
	public GameManager gameManager;

	[HideInInspector]
	public AudioManager audioManager;

	private void Awake()
	{
		model = GameObject.FindGameObjectWithTag("Model").GetComponent<Model>();
		view = GameObject.FindGameObjectWithTag("View").GetComponent<View>();

		cameraManager = GetComponent<CameraManager>();
		gameManager = GetComponent<GameManager>();
		audioManager = GetComponent<AudioManager>();
	}


	private void MakeFSM()
	{
		fsm = new FSMSystem();
		FSMState[] states = GetComponentsInChildren<FSMState>();
		foreach(FSMState state in states) {
			fsm.AddState(state, this);
		}

		// 设置默状态为菜单状态
		MenuState s = GetComponentInChildren<MenuState>();
		fsm.SetCurrentState(s);
	}

	// Use this for initialization
	void Start () {
		// 有限状态机
		MakeFSM();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
