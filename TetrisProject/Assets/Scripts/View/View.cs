using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class View : MonoBehaviour {

	private Ctrl ctrl;
	private RectTransform logoName;
	private RectTransform menuUI;
	private RectTransform gameUI;
	private RectTransform gameOverUI;
	private RectTransform settingUI;
	private GameObject rankUI;

	private Text score;
	private Text highScore;
	private Text gameOverScore;

	private GameObject mute;

	private GameObject restartButton;
	// Use this for initialization
	void Awake () {

		ctrl = GameObject.FindGameObjectWithTag("Ctrl").GetComponent<Ctrl>();


		logoName = transform.Find("Canvas/LogoName") as RectTransform;
		menuUI = transform.Find("Canvas/MenuUI") as RectTransform;
		gameUI = transform.Find("Canvas/GameUI") as RectTransform;
		gameOverUI = transform.Find("Canvas/GameOverUI") as RectTransform;
		settingUI = transform.Find("Canvas/SettingUI") as RectTransform;
		rankUI = transform.Find("Canvas/RankUI").gameObject;

		restartButton = transform.Find("Canvas/MenuUI/RestartButton").gameObject;

		score = transform.Find("Canvas/GameUI/ScoreLabel/Text").GetComponent<Text>();
		highScore = transform.Find("Canvas/GameUI/HighScoreLabel/Text").GetComponent<Text>();
		gameOverScore = transform.Find("Canvas/GameOverUI/Backgroud/Text").GetComponent<Text>();

		mute = transform.Find("Canvas/SettingUI/AudioButton/Mute").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ShowMenu() {
		logoName.gameObject.SetActive(true);
		logoName.DOAnchorPosY(-255.7f, 0.5f);

		menuUI.gameObject.SetActive(true);
		menuUI.DOAnchorPosY(73.3f, 0.5f);

	}

	public void HideMenu() {
		logoName.DOAnchorPosY(255.7f, 0.5f).OnComplete(delegate { logoName.gameObject.SetActive(false); });

		menuUI.DOAnchorPosY(-73.3f, 0.5f).OnComplete(delegate { menuUI.gameObject.SetActive(false); });
	}

	public void UpdateGameUI(int score, int highScore) {
		this.score.text = score.ToString();
		this.highScore.text = highScore.ToString();
	}
	public void ShowGameUI(int score = 0, int highScore = 0) {

		this.score.text = score.ToString();
		this.highScore.text = highScore.ToString();
		gameUI.gameObject.SetActive(true);
		gameUI.DOAnchorPosY(-186.1f, 0.5f);
	}

	public void HideGameUI()
	{
		gameUI.DOAnchorPosY(186.1f, 0.5f).OnComplete(delegate { gameUI.gameObject.SetActive(false); });
	}

	public void ShowRestartButton() {
		restartButton.SetActive(true);
	}

	public void ShowGameOverUI(int score = 0) {
		gameOverUI.gameObject.SetActive(true);
		gameOverScore.text = score.ToString();
	}

	public void OnHomeButtonClick() {
		ctrl.audioManager.PlayCusor();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void HideGameOverUI() {
		gameOverUI.gameObject.SetActive(false);
	}

	public void OnSettingButtonClick() {

		ctrl.audioManager.PlayCusor();
		settingUI.gameObject.SetActive(true);
	}

	public void SetMuteActive(bool isActive) {
		mute.SetActive(isActive);
	}

	public void OnSettingUIClick() {
		ctrl.audioManager.PlayCusor();
		settingUI.gameObject.SetActive(false);
	}

	public void OnRankButtonClick() {
		ctrl.audioManager.PlayCusor();
		rankUI.SetActive(true);
	}

	public void OnRankUIClick() {
		ctrl.audioManager.PlayCusor();
		rankUI.SetActive(false);
	}
}
