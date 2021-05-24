using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour {
	private Camera mainCamera;

	private void Awake()
	{
		mainCamera = Camera.main;
	}

	public void ZoomIn() {
		mainCamera.DOOrthoSize(16.01f, 0.5f);
	}

	public void ZoomOut() {
		mainCamera.DOOrthoSize(23.2f, 0.5f);
	}
}
