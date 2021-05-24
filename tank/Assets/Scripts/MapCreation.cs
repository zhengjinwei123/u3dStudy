using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreation : MonoBehaviour {

	// Use this for initialization

	public GameObject heart;
	public GameObject wall;
	public GameObject barriar;
	public GameObject born;
	public GameObject river;
	public GameObject grass;
	public GameObject airBarriar;

	private List<Vector3> itemPositionList = new List<Vector3>();

	private void Awake()
	{
		InitMap();
	}

	void Start () {
		
	}

	private void InitMap() {
		// 老家
		createItem(heart, new Vector3(0, -8, 0), Quaternion.identity);

		// 老家的围墙
		createItem(wall, new Vector3(-1, -8, 0), Quaternion.identity);
		createItem(wall, new Vector3(1, -8, 0), Quaternion.identity);
		for (int i = -1; i < 2; i++) {
			createItem(wall, new Vector3(i, -7, 0), Quaternion.identity);
		}
		// 外围墙
		for (int i = -11; i < 12; i++)
		{
			createItem(airBarriar, new Vector3(i, 9, 0), Quaternion.identity);
		}
		for (int i = -11; i < 12; i++)
		{
			createItem(airBarriar, new Vector3(i, -9, 0), Quaternion.identity);
		}
		for (int i = -8; i < 9; i++)
		{
			createItem(airBarriar, new Vector3(-11, i, 0), Quaternion.identity);
		}
		for (int i = -8; i < 9; i++)
		{
			createItem(airBarriar, new Vector3(11, i, 0), Quaternion.identity);
		}

		// 玩家
		GameObject go = Instantiate(born, new Vector3(-2, -8, 0), Quaternion.identity);
		go.GetComponent<Born>().createPlayer = true;

		// 敌人
		createItem(born, new Vector3(-10, 8, 0), Quaternion.identity);
		createItem(born, new Vector3(0, 8, 0), Quaternion.identity);
		createItem(born, new Vector3(10, 8, 0), Quaternion.identity);

		InvokeRepeating("createEnemy", 4, 5);
		// 地图
		for (int i = 0; i < 60; i++)
		{
			createItem(wall, createRandomPosition(), Quaternion.identity);
		}
		for (int i = 0; i < 20; i++)
		{
			createItem(barriar, createRandomPosition(), Quaternion.identity);
		}
		for (int i = 0; i < 20; i++)
		{
			createItem(river, createRandomPosition(), Quaternion.identity);
		}
		for (int i = 0; i < 20; i++)
		{
			createItem(grass, createRandomPosition(), Quaternion.identity);
		}

	}

	private void createItem(GameObject createGameObject, Vector3 position, Quaternion rotation) {
		GameObject item = Instantiate(createGameObject, position, rotation);
		item.transform.SetParent(gameObject.transform);
		itemPositionList.Add(position);
	}

	private Vector3 createRandomPosition() {

		while (true) {
			Vector3 pos = new Vector3(Random.Range(-9, 10), Random.Range(-7, 8), 0);
			if (!hasThenPosition(pos)) {
				return pos;
			}
		}
	}

	private bool hasThenPosition(Vector3 pos) {
		for (int i = 0; i < itemPositionList.Count; ++i) {
			if (pos == itemPositionList[i]) {
				return true;
			}
		}
		return false;
	}


	private void createEnemy() {
		int num = Random.Range(0, 3);
		Vector3 enemyPos = new Vector3();
		if (num == 0)
		{
			enemyPos = new Vector3(-10, 8, 0);
		}
		else if (num == 1) {
			enemyPos = new Vector3(0, 8, 0);
		} else  {
			enemyPos = new Vector3(10, 8, 0);
		}

		createItem(born, enemyPos, Quaternion.identity);
	}


	// Update is called once per frame
	void Update () {
		
	}
}
