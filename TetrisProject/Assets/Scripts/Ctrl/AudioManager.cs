using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	private Ctrl ctrl;


	public AudioClip cursor;
	public AudioClip drop;
	public AudioClip control;
	public AudioClip lineClear;

	private AudioSource audioSource;

	private bool isMute = false;

	private void Awake()
	{
		ctrl = GetComponent<Ctrl>();
		audioSource = GetComponent<AudioSource>();
	}

	public void PlayCusor() {
		PlayAudio(cursor);
	}
	public void PlayControl ()
	{
		PlayAudio(control);
	}
	public void PlayLineClear()
	{
		PlayAudio(lineClear);
	}


	public void PlayDrop() {
		PlayAudio(drop);
	}

	private void PlayAudio(AudioClip clip) {
		if (isMute) return;

		audioSource.clip = clip;
		audioSource.Play();
	}

	public void OnAudioButtonClick() {
		isMute = !isMute;

		ctrl.view.SetMuteActive(isMute);
		PlayCusor();
	}
}
