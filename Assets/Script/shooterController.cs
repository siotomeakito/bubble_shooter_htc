using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooterController : MonoBehaviour
{
	public enum shootingSpeedLevel {
		slow, normal, fast
	}
	public shootingSpeedLevel speedLevel;
	public float[] shootingSpeed = {300f, 500f, 800f};

	public float maxAngle = 60;

	private GameManager.GameState myGameState;

	public void setSpeedLevel(shootingSpeedLevel _level)
	{
		speedLevel = _level;
		return;
	}

	public float getSpeed()
	{
		return shootingSpeed[(int)speedLevel];
	}

	void Awake()
	{
		speedLevel = shootingSpeedLevel.normal;

		myGameState = GameManager.GetGameState();
	}
	
    void FixedUpdate()
    {
		stateSwitch();

		if (myGameState == GameManager.GameState.playing)
		{
			shooterRotation();
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

	public void shooterRotation()
	{
		Vector2 myDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		float myAngle = Mathf.Atan2(myDirection.y, myDirection.x) * Mathf.Rad2Deg - 90;
		if (myAngle >= -maxAngle && myAngle <= maxAngle)
		{
			transform.rotation = Quaternion.AngleAxis(myAngle, Vector3.forward);
		}
	}
}
