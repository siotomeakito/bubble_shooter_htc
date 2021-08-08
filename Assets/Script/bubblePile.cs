using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bubblePile : MonoBehaviour
{
	public enum bubbleNumLevel
	{
		easy, normal, hard
	}
	public bubbleNumLevel numLevel;
	private int[] bubbleRowNum = { 9, 13, 17 };  // must less than GridOfBubble.maxBubbleRaw
	

	public enum compressLevel
	{
		easy, normal, hard
	}
	public compressLevel speedLevel;
	private float[] compressSpeed = { 0.05f, 0.1f, 0.2f };

	public enum bubbleTypeLevel
	{
		less, normal
	}
	public bubbleTypeLevel typeLevel;
	private int[] bubbleTypeNum = { 4, 6 };

	public GameObject burstParticle;
	public bubbleObj initialBubble;
	public Sprite[] allBubbleType = new Sprite[allTypeNumOfBubble];

	private bool includeSpecial;
	private const int allTypeNumOfBubble = (int)bubbleUI.bubbleType.end;
	private float probabilityOfSpecial = 0.05f;

	private Vector2 compressorPosition;
	private Vector2[] pileStartPosition = {new Vector2(0f, 6f), new Vector2(0f, 8f), new Vector2(0f, 10f)};
	private int burstCount = 0, fallCount = 0;

	private GameManager.GameState myGameState;

	public void setBubbleNumLevel(bubbleNumLevel _level)
	{
		numLevel = _level;
		return;
	}

	public void setCompressSpeedLevel(compressLevel _level)
	{
		speedLevel = _level;
		return;
	}

	public void setBubbleTypeLevel(bubbleTypeLevel _level)
	{
		typeLevel = _level;
		return;
	}



	void Awake()
	{
		compressorPosition = transform.position;
		numLevel = bubbleNumLevel.hard;
		speedLevel = compressLevel.normal;
		typeLevel = bubbleTypeLevel.normal;
		includeSpecial = true;

		myGameState = GameManager.GetGameState();
	}

	void FixedUpdate()
	{
		stateSwitch();

		if (myGameState == GameManager.GameState.playing)
		{
			transform.Translate(0f, -compressSpeed[(int)speedLevel] * Time.deltaTime, 0f);
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
				destroyBubblePile();
				break;
			case GameManager.GameState.playing:
				if (myGameState == GameManager.GameState.setting)
				{
					buildBubblePile();
				}
				break;
			case GameManager.GameState.pausing:
				break;
			case GameManager.GameState.ending:
				break;
		}

		myGameState = GameManager.GetGameState();
		return;
	}

	public void setSpecial(bool _include)
	{
		includeSpecial = _include;
		return;
	}

	public void randomBubble(ref bubbleObj _bubble)
	{
		int i = Random.Range(0, bubbleTypeNum[(int)typeLevel]);
		_bubble.setBubbleType((bubbleObj.bubbleType)i, allBubbleType[i]);
		return;
	}

	public void addSpecial(ref bubbleObj _bubble)    // 設定機率性出現特殊泡泡
	{
		if (!includeSpecial)
		{
			return;
		}

		if (Random.Range(0f, 1f) < probabilityOfSpecial)
		{
			int specialTemp = Random.Range((int)bubbleUI.bubbleType.special_0, (int)bubbleUI.bubbleType.end);
			_bubble.setBubbleType((bubbleObj.bubbleType)specialTemp, allBubbleType[specialTemp]);
		}

		return;
	}

	public void buildBubblePile()
	{
		if (!BubbleManager.gridBilt)
		{
			BubbleManager.InitialEmptyGrid();
		}

		transform.position = pileStartPosition[(int)numLevel];

		for (int i = 0; i < bubbleRowNum[(int)numLevel] * BubbleManager.bubbleCol - (bubbleRowNum[(int)numLevel] / 2); i++)
		{
			bubbleObj newBubble = Instantiate(initialBubble, BubbleManager.nodePosition[i] + (Vector2)transform.position, Quaternion.identity);
			newBubble.transform.parent = transform;
			randomBubble(ref newBubble);
			addSpecial(ref newBubble);
			newBubble.becomeStayingBubble(i);
		}

		return;
	}

	public void destroyBubblePile()
	{
		for (int i = 0; i < BubbleManager.bubbleNum; i++)
		{
			if (!BubbleManager.nodeBubble[i])
			{
				continue;
			}

			BubbleManager.nodeBubble[i].destroyBubble();
			BubbleManager.DeleteNodeBubble(i);
		}

		return;
	}

	public void playBurstParticle(int _id)
	{
		if (!burstParticle)
		{
			return;
		}

		if (_id < 0 || _id >= BubbleManager.bubbleNum)
		{
			return;
		}

		GameObject newBurstParticle = Instantiate(burstParticle, BubbleManager.nodePosition[_id]+ (Vector2)transform.position, Quaternion.identity);
		Destroy(newBurstParticle, 1.5f);
	}

	public void checkBurstEnd()
	{
		if (burstCount <= 0)
		{
			// 泡泡消除後檢查連接情形
			if (!BubbleManager.ScanBubble())
			{
				wonTheGame();
			}
			setFallCount(BubbleManager.CountFall());
		}

		return;
	}

	public void checkFallEnd()
	{
		if (fallCount <= 0)
		{
			// 可再加泡泡掉落後事件
		}

		return;
	}

	public void setBurstCount(int _n)
	{
		burstCount = _n;
		return;
	}

	public void setFallCount(int _n)
	{
		fallCount = _n;
		return;
	}

	public void reduceBurstCount()
	{
		burstCount--;
		return;
	}

	public void reduceFallCount()
	{
		fallCount--;
		return;
	}

	public void wonTheGame()
	{
		GameManager.SetResult(true);
	}
}
