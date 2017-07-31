using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOfDayComponents : MonoBehaviour {
	public GameObject[] eyesToSpawn;
	public bool increaseEyeSpeed;
	public bool immediatelySpawnEyes;
	public GameObject[] timeSpecificEnemiesToSpawn;
	public bool increaseSpecificEnemySpeed;
	public bool immediatelySpawnSpecificEnemies;
	public GameObject[] bonusEnemiesToSpawn;
	public bool increaseBonusEnemySpeed;
	public bool immediatelySpawnBonusEnemies;
	public GameObject boss;
	public Texture backgroundTexture;
}
