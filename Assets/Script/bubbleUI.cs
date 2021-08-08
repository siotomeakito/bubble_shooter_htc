using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bubbleUI : MonoBehaviour
{
	public enum bubbleState
	{
		none, next, ready, flying, staying, falling, bursting
	}
	protected bubbleState myState;

	public enum bubbleType
	{
		normal_0, normal_1, normal_2, normal_3, normal_4, normal_5,
		special_0, special_1,
		end
	}
	protected bubbleType myType;
	
	public void setBubbleState(bubbleState _state)
	{
		myState = _state;
		return;
	}

	public bubbleState getBubbleState()
	{
		return myState;
	}

	public void setBubbleType(bubbleType _type, Sprite _picture)
	{
		myType = _type;
		GetComponent<SpriteRenderer>().sprite = _picture;
		return;
	}

	public bubbleType getBubbleType()
	{
		return myType;
	}
	
	void Awake()
	{
		myState = bubbleState.none;
		myType = 0;
	}
}
