using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

	private float speed = 3;
	private Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded") == false)
		{
			return;
		}
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		if (Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0) {
			// 按键控制方向
			transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime, Space.World);
			// 旋转控制
			transform.rotation = Quaternion.LookRotation(new Vector3(h, 0, v));

			// 动画控制
			float res = Mathf.Max(Mathf.Abs(h), Mathf.Abs(v));
			anim.SetFloat("Forward", res);
		}
	}
}
