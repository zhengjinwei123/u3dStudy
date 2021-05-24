using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public float moveSpeed = 3;
	private Vector3 bulletEulerAngles;
	private float v = -1;
	private float h;

	private SpriteRenderer sr;
	public Sprite[] tankSprite;// 上 右 下 左
	public GameObject bulletPrefab;
	public GameObject explosionPrefab;


	private float timeVal;
	private float timeValChangeDirection;

	private void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
	}

	// Use this for initialization
	void Start()
	{

	}

	private void Attack()
	{
		Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.eulerAngles + bulletEulerAngles));
		timeVal = 0;
	}

	private void Move()
	{
		if (timeValChangeDirection >= 4)
		{
			int num = Random.Range(0, 8);
			if (num > 5)
			{
				v = -1;
				h = 0;
			}
			else if (num == 0)
			{
				v = 1;
				h = 0;
			}
			else if (num > 0 && num <= 2)
			{
				h = -1;
				v = 0;
			}
			else if (num > 2 && num <= 4)
			{
				h = 1;
				v = 0;
			}

			timeValChangeDirection = 0;
		}
		else {
			timeValChangeDirection += Time.fixedDeltaTime;
		}

		transform.Translate(Vector3.up*v*moveSpeed * Time.fixedDeltaTime, Space.World);

		if (v < 0)
		{
			// down
			sr.sprite = tankSprite[2];
			bulletEulerAngles = new Vector3(0, 0, -180);
		}
		else if (v > 0) {
			// up
			sr.sprite = tankSprite[0];
			bulletEulerAngles = new Vector3(0, 0, 0);
		}

		if (v != 0) {
			return;
		}
		transform.Translate(Vector3.right* h * moveSpeed * Time.fixedDeltaTime, Space.World);
		if (h < 0)
		{
			// left
			sr.sprite = tankSprite[3];
			bulletEulerAngles = new Vector3(0, 0, 90);
		}
		else if (h > 0) {
			// right
			sr.sprite = tankSprite[1];
			bulletEulerAngles = new Vector3(0, 0, -90);
		}

	}

	void FixedUpdate()
	{
		Move();

		if (timeVal >= 3)
		{
			// attack
			Attack();
		}
		else
		{
			timeVal += Time.fixedDeltaTime;
		}
	}

	// Update is called once per frame
	void Update()
	{
	
	}

	private void Die()
	{
		PlayerManager.Instance.playerScore++;
		Instantiate(explosionPrefab, transform.position, transform.rotation);

		Destroy(gameObject);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		
		if (collision.gameObject.tag == "Enemy") {
			timeValChangeDirection = 4;
		}
	}
}
