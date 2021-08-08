using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class scoreCounter : MonoBehaviour
{
	private int scoreOfBurstOneBubble = 100, scoreOfFallOneBubble = 50, scoreOfShootOneBubble = 10;
	private int burstCount, fallCount, shootCount, sumScore;

	private GameManager.GameState myGameState;

	void Awake()
	{
		resetScore();

		myGameState = GameManager.GetGameState();
	}

	void OnEnable()
	{
		resetScore();
	}

	void Update()
    {
		stateSwitch();

		if (myGameState == GameManager.GameState.playing && getCounter())
		{
			sumScore = burstCount * scoreOfBurstOneBubble + fallCount * scoreOfFallOneBubble + shootCount * scoreOfShootOneBubble;
			GetComponent<Text>().text = "" + sumScore;
			GameManager.SetUserScore(sumScore);
		}
    }

	private void stateSwitch()
	{
		if (GameManager.GetGameState() == myGameState)
		{
			return;
		}

		switch (GameManager.GetGameState())
		{
			case GameManager.GameState.setting:
				resetScore();
				break;
			case GameManager.GameState.playing:
				break;
			case GameManager.GameState.pausing:
				break;
			case GameManager.GameState.ending:
				break;
		}

		myGameState = GameManager.GetGameState();
		return;
	}

	private bool getCounter()
	{
		if (shootCount == BubbleManager.GetNumOfShoot() && burstCount == BubbleManager.GetNumOfBurst() && fallCount == BubbleManager.GetNumOfFall())
		{
			return false;
		}

		burstCount = BubbleManager.GetNumOfBurst();
		fallCount = BubbleManager.GetNumOfFall();
		shootCount = BubbleManager.GetNumOfShoot();

		return true;
	}

	public int getScore()
	{
		return sumScore;
	}

	public void resetScore()
	{
		burstCount = 0;
		fallCount = 0;
		shootCount = 0;
		sumScore = 0;
		GameManager.SetUserScore(0);
		BubbleManager.ResetCounter();
		GetComponent<Text>().text = "" + 0;
		return;
	}
}
