using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {

	private SpriteRenderer sr;
	public GameObject explosionPrefab;
	public AudioClip DieAudio;

	public Sprite BrokenSprite;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Die() {
		PlayerManager.Instance.isDefeat = true;
		sr.sprite = BrokenSprite;
		Instantiate(explosionPrefab, transform.position, transform.rotation);

		AudioSource.PlayClipAtPoint(DieAudio, transform.position);
	}
}
