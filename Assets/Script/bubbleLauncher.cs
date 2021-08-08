using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class bubbleLauncher : MonoBehaviour
{
	public enum bubbleTypeLevel
	{
		less, normal
	}
	public bubbleTypeLevel typeLevel;
	private int[] bubbleTypeNum = { 4, 6 };

	public shooterController mainShooter;
	public bubbleObj initialBubble;
	public bubbleUI nextBubble, readyBubble;
	public Sprite[] allBubbleType = new Sprite[typeOfBubble];

	public bool includeSpecial;
	private const int typeOfBubble = (int)bubbleUI.bubbleType.end;
	private float probabilityOfSpecial = 0.05f;
	private Vector2 readyPosition;

	private GameManager.GameState myGameState;

	public void setBubbleTypeLevel(bubbleTypeLevel _level)
	{
		typeLevel = _level;
		return;
	}

	void Awake()
	{
		typeLevel = bubbleTypeLevel.normal;
		readyPosition = readyBubble.transform.position;
		includeSpecial = true;
		nextBubble.gameObject.SetActive(false);
		readyBubble.gameObject.SetActive(false);

		myGameState = GameManager.GetGameState();
	}

	void Start()
    {
		
	}
	
    void Update()
    {
		stateSwitch();

		if (myGameState == GameManager.GameState.playing)
		{
			if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
			{
				shootBubble(mainShooter.transform.rotation.eulerAngles.z, mainShooter.getSpeed());
			}

			if (Input.GetMouseButtonDown(1))
			{
				changeReadyBubble();
			}
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
				gameEnd();
				break;
			case GameManager.GameState.playing:
				if (myGameState == GameManager.GameState.setting)
				{
					gameStart();
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

	public void gameStart()
	{
		nextBubble.gameObject.SetActive(true);
		readyBubble.gameObject.SetActive(true);
		nextBubble.setBubbleState(bubbleUI.bubbleState.next);
		randomBubble(ref nextBubble);
		readyBubble.setBubbleState(bubbleUI.bubbleState.ready);
		randomBubble(ref readyBubble);
		return;
	}
	
	public void gameEnd()
	{
		nextBubble.gameObject.SetActive(false);
		readyBubble.gameObject.SetActive(false);
		return;
	}
	
	private void OnTriggerStay2D(Collider2D _collision)
	{
		if (!_collision.GetComponent<bubbleObj>())
		{
			return;
		}

		if (_collision.GetComponent<bubbleObj>().getBubbleState() == bubbleUI.bubbleState.staying)
		{
			lostTheGame();
		}
	}

	public void setSpecial(bool _include)
	{
		includeSpecial = _include;
		return;
	}

	public void randomBubble(ref bubbleUI _bubble)
	{
		int i = Random.Range(0, bubbleTypeNum[(int)typeLevel]);
		_bubble.setBubbleType((bubbleUI.bubbleType)i, allBubbleType[i]);
		return;
	}

	public void addSpecial(ref bubbleUI _bubble)    // 設定機率性出現特殊泡泡
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

	public void shootBubble(float _rotation, float _speed)
	{
		bubbleObj flyingBubble = Instantiate(initialBubble, readyBubble.transform.position, readyBubble.transform.rotation);
		flyingBubble.setBubbleState(bubbleUI.bubbleState.flying);
		flyingBubble.setBubbleType(readyBubble.getBubbleType(), allBubbleType[(int)readyBubble.getBubbleType()]);
		readyBubble.setBubbleType(nextBubble.getBubbleType(), allBubbleType[(int)nextBubble.getBubbleType()]);
		randomBubble(ref nextBubble);
		addSpecial(ref nextBubble);
		flyingBubble.GetComponent<Rigidbody2D>().AddForce(new Vector2(-Mathf.Sin(_rotation * Mathf.Deg2Rad), Mathf.Cos(_rotation * Mathf.Deg2Rad)).normalized * _speed);
		return;
	}

	public void changeReadyBubble()
	{
		bubbleUI.bubbleType typeTemp = nextBubble.getBubbleType();
		nextBubble.setBubbleType(readyBubble.getBubbleType(), allBubbleType[(int)readyBubble.getBubbleType()]);
		readyBubble.setBubbleType(typeTemp, allBubbleType[(int)typeTemp]);
		return;
	}

	public void lostTheGame()
	{
		GameManager.SetResult(false);
	}
}
