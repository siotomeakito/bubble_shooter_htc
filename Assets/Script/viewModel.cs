using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class viewModel : MonoBehaviour
{
	public bubbleLauncher myBubbleLauncher;
	public bubblePile myBubblePile;
	public shooterController myShooterController;
	public scoreCounter score;
	public Canvas settingUI;
	public Canvas playingUI;
	public Canvas pausingUI;
	public Canvas endingUI;
	public SpriteRenderer backGround;
	public Text userName, endingScore;
	public Image winPic, losePic;

	private GameManager.GameState myGameState;

	void Awake()
	{
		myGameState = GameManager.GetGameState();
	}

	void Update()
	{
		stateSwitch();
	}

	private void stateSwitch()
	{
		if (!settingUI || !playingUI || !pausingUI || !endingUI)
		{
			return;
		}

		if (GameManager.GetGameState() != myGameState)
		{
			switch (GameManager.GetGameState())
			{
				case GameManager.GameState.setting:
					settingUI.gameObject.SetActive(true);
					playingUI.gameObject.SetActive(false);
					pausingUI.gameObject.SetActive(false);
					endingUI.gameObject.SetActive(false);
					score.resetScore();
					if (backGround)
					{
						backGround.sortingOrder = 20;
					}
					break;
				case GameManager.GameState.playing:
					settingUI.gameObject.SetActive(false);
					playingUI.gameObject.SetActive(true);
					pausingUI.gameObject.SetActive(false);
					endingUI.gameObject.SetActive(false);
					if (backGround)
					{
						backGround.sortingOrder = 0;
					}
					break;
				case GameManager.GameState.pausing:
					settingUI.gameObject.SetActive(false);
					playingUI.gameObject.SetActive(false);
					pausingUI.gameObject.SetActive(true);
					endingUI.gameObject.SetActive(false);
					if (backGround)
					{
						backGround.sortingOrder = 20;
					}
					break;
				case GameManager.GameState.ending:
					settingUI.gameObject.SetActive(false);
					playingUI.gameObject.SetActive(false);
					pausingUI.gameObject.SetActive(false);
					endingUI.gameObject.SetActive(true);
					if (endingScore)
					{
						endingScore.text = "" + GameManager.GetUserScore();
					}

					if (GameManager.GetResult())
					{
						whenWin();
					}
					else
					{
						whenLost();
					}

					break;
			}
		}

		myGameState = GameManager.GetGameState();
		return;
	}

	public void GameStart()
	{
		GameManager.SetGameState(GameManager.GameState.playing);
		return;
	}

	public void GamePause()
	{
		GameManager.SetGameState(GameManager.GetGameState() == GameManager.GameState.playing ? GameManager.GameState.pausing : GameManager.GameState.playing);
		return;
	}

	public void GameEnd()
	{
		GameManager.SetGameState(GameManager.GameState.setting);
		return;
	}

	public void ShootSpeed(int _i)
	{
		if (!myShooterController)
		{
			return;
		}

		switch (_i)
		{
			case 0:
				myShooterController.setSpeedLevel(shooterController.shootingSpeedLevel.slow);
				break;
			case 1:
				myShooterController.setSpeedLevel(shooterController.shootingSpeedLevel.normal);
				break;
			case 2:
				myShooterController.setSpeedLevel(shooterController.shootingSpeedLevel.fast);
				break;
		}
	}

	public void BubbleNum(int _i)
	{
		if (!myBubblePile)
		{
			return;
		}

		switch (_i)
		{
			case 0:
				myBubblePile.setBubbleNumLevel(bubblePile.bubbleNumLevel.easy);
				break;
			case 1:
				myBubblePile.setBubbleNumLevel(bubblePile.bubbleNumLevel.normal);
				break;
			case 2:
				myBubblePile.setBubbleNumLevel(bubblePile.bubbleNumLevel.hard);
				break;
		}
	}

	public void CompressorSpeed(int _i)
	{
		if (!myBubblePile)
		{
			return;
		}

		switch (_i)
		{
			case 0:
				myBubblePile.setCompressSpeedLevel(bubblePile.compressLevel.easy);
				break;
			case 1:
				myBubblePile.setCompressSpeedLevel(bubblePile.compressLevel.normal);
				break;
			case 2:
				myBubblePile.setCompressSpeedLevel(bubblePile.compressLevel.hard);
				break;
			case 3:
				myBubblePile.setCompressSpeedLevel(bubblePile.compressLevel.extreme);
				break;
		}
	}

	public void BubbleType(int _i)
	{
		if (!myBubblePile || !myBubbleLauncher)
		{
			return;
		}

		switch (_i)
		{
			case 0:
				myBubblePile.setBubbleTypeLevel(bubblePile.bubbleTypeLevel.less);
				myBubbleLauncher.setBubbleTypeLevel(bubbleLauncher.bubbleTypeLevel.less);
				break;
			case 1:
				myBubblePile.setBubbleTypeLevel(bubblePile.bubbleTypeLevel.normal);
				myBubbleLauncher.setBubbleTypeLevel(bubbleLauncher.bubbleTypeLevel.normal);
				break;
		}
	}

	public void SpecialBubble(int _i)
	{
		if (!myBubblePile || !myBubbleLauncher)
		{
			return;
		}

		switch (_i)
		{
			case 0:
				myBubblePile.setSpecial(true);
				myBubbleLauncher.setSpecial(true);
				break;
			case 1:
				myBubblePile.setSpecial(false);
				myBubbleLauncher.setSpecial(false);
				break;
		}
	}

	public void SetUserName(string _s)
	{
		if (userName)
		{
			userName.text = _s;
		}

		GameManager.SetUserName(_s);
		return;
	}

	public void ResetUserName()
	{
		GameManager.SetUserName("");
		return;
	}

	private void whenWin()
	{
		// 勝利事件 可新增特效等
		if (winPic && losePic)
		{
			winPic.gameObject.SetActive(true);
			losePic.gameObject.SetActive(false);
		}
	}

	private void whenLost()
	{
		// 失敗事件 可新增特效等
		if (winPic && losePic)
		{
			winPic.gameObject.SetActive(false);
			losePic.gameObject.SetActive(true);
		}
	}
}
