using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class scoreCounter : MonoBehaviour
{
	private int scoreOfBurstOneBubble = 100, scoreOfFallOneBubble = 50, scoreOfShootOneBubble = 10;
	private int burstCount, fallCount, shootCount, sumScore;

	void Awake()
	{
		resetScore();
	}

	void Update()
    {
		if (getCounter())
		{
			sumScore = burstCount * scoreOfBurstOneBubble + fallCount * scoreOfFallOneBubble + shootCount * scoreOfShootOneBubble;
			GetComponent<Text>().text = "" + sumScore;
			GameManager.SetUserScore(sumScore);
		}
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
