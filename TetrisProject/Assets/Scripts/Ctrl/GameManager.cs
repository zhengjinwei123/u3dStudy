using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private bool isPause = true; // 游戏是否暂停

	public Shape[] shapes;

	public Color[] colors;

	private Shape currentShape = null;

	private Ctrl ctrl;

	private Transform blockHolder;


	private void Awake()
	{
		ctrl = GetComponent<Ctrl>();
		blockHolder = transform.Find("BlockHolder");
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isPause) {
			return;
		}

		if (currentShape == null) {
			SpawnShape();
		}
	}

	public void FallDown() {
		currentShape = null;

		if (ctrl.model.isDataUpdate) {
			ctrl.view.UpdateGameUI(ctrl.model.Score, ctrl.model.HighScore);

			ctrl.model.isDataUpdate = false;
		}

		// 銷毀block
		foreach (Transform t in blockHolder) {
			if (t.childCount <= 1) {
				Destroy(t.gameObject);
			}
		}

		if (ctrl.model.IsGameOver()) {
			PauseGame();
			ctrl.view.ShowGameOverUI(ctrl.model.Score);
		}
	}

	void SpawnShape() {
		int index = Random.Range(0, shapes.Length);
		int indexColor = Random.Range(0, colors.Length);
		currentShape = GameObject.Instantiate(shapes[index]);
		currentShape.transform.parent = blockHolder;
		currentShape.Init(colors[indexColor], ctrl, this);
	}

	public void StartGame() {
		isPause = false;

		if (currentShape != null)
		{
			currentShape.Resume();
		}
	}

	public void PauseGame() {
		isPause = true;
		if (currentShape != null) {
			currentShape.Pause();
		}
	}
}
