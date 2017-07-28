using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreservableValues : MonoBehaviour {
	static int timeOfDay;
	static float score;
	static float timer;

	public static int GetTimeOfDay()
	{
		return timeOfDay;
	}

	public static void SetTimeOFDay(int newTimeOfDay)
	{
		timeOfDay = newTimeOfDay;
	}

	public static float GetScore()
	{
		return score;
	}

	public static void SetScore(float newScore)
	{
		score = newScore;
	}

	public static float GetTimer() {
		return timer;
	}

	public static void SetTimer(float newTimer) {
		timer = newTimer;
	}

	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}
}
