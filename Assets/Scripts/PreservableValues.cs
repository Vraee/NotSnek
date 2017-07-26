using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreservableValues : MonoBehaviour {
	static int timeOfDay;
	static float score;

	public int GetTimeOfDay()
	{
		return timeOfDay;
	}

	public static void SetTimeOFDay(int newTimeOfDay)
	{
		timeOfDay = newTimeOfDay;
	}

	public float GetScore()
	{
		return score;
	}

	public static void SetScore(float newScore)
	{
		score = newScore;
	}

	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}
}
