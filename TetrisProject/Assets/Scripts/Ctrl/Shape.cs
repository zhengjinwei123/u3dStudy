using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour {

	private Transform pivot;
	private bool isPause = false;

	private float timer = 0;
	private float stepTime = 0.8f;
	private int multiple = 16; // 加速倍數
	private bool isSpeedUp = false;

	private GameManager gameManager;
	private Ctrl ctrl;

	private void Awake()
	{
		pivot = transform.Find("Pivot");
	}

	private void Update()
	{
		if (isPause) { return;  }

		timer += Time.deltaTime;
		if (timer > stepTime) {
			timer = 0;
			Fall();
		}

		InputControl();
	}

	public void Pause() {
		isPause = true;
	}

	public void Resume() {
		isPause = false;
	}

	private void Fall()
	{
		Vector3 pos = transform.position;
		pos.y -= 1;

		transform.position = pos;

		if (ctrl.model.IsValidMapPosition(this.transform) == false) {
			pos.y += 1;
			transform.position = pos;

			isPause = true;
		
			bool isLineClear = ctrl.model.PlaceShape(transform);
			if (isLineClear) {
				ctrl.audioManager.PlayLineClear();
			}

			gameManager.FallDown();
			return;
		}

		ctrl.audioManager.PlayDrop();
	}


	private void InputControl() {
		//if (isSpeedUp) return;

		float h = 0;

		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			h = -1;
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow)) {
			h = 1;
		}

		if (h != 0)
		{
			Vector3 pos = transform.position;
			pos.x += h;
			transform.position = pos;
			if (ctrl.model.IsValidMapPosition(this.transform) == false)
			{
				pos.x -= h;
				transform.position = pos;
			}
			else {
				ctrl.audioManager.PlayControl();
			}
		}

		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			transform.RotateAround(pivot.position, Vector3.forward, -90);
			if (ctrl.model.IsValidMapPosition(this.transform) == false)
			{
				transform.RotateAround(pivot.position, Vector3.forward, 90);
			}
			else {
				ctrl.audioManager.PlayControl();
			}
		}

		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			isSpeedUp = true;

			stepTime /= multiple;
		}
	}

	public void Init(Color color, Ctrl ctrl, GameManager gameManager) {
		this.ctrl = ctrl;
		this.gameManager = gameManager;

		foreach (Transform t in transform) {
			if (t.tag == "Block") {
				t.GetComponent<SpriteRenderer>().color = color;
			}
		}
	}
}
