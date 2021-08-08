using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D), typeof(Rigidbody2D))]
public class bubbleObj : bubbleUI
{
	private int myID;
	public bubbleObj[] neighborBubble = new bubbleObj[6];
	// upper left -> 0, upper right -> 1, left -> 2, right -> 3, lower left -> 4, lower right -> 5;
	//  ０１
	// ２Ｘ３
	//  ４５

	private bool burstDone, fallDone;
	private float fallingTimer = 2f;
	private float flyingTimer = 4f;

	public enum DFS_checker {
		none, included, excluded
	}

	private DFS_checker chainChecker;
	private DFS_checker connectionChecker;

	public void setChainChecker(DFS_checker _state)
	{
		chainChecker = _state;
		return;
	}

	public DFS_checker getChainChecker()
	{
		return chainChecker;
	}

	public void setConnectionChecker(DFS_checker _state)
	{
		connectionChecker = _state;
		return;
	}

	public DFS_checker getConnectionChecker()
	{
		return connectionChecker;
	}

	public void setID(int _id)
	{
		myID = _id;
		return;
	}

	public int getID()
	{
		return myID;
	}

	public void setNeighbor(int _n, bubbleObj _newNeighbor)
	{
		neighborBubble[_n] = _newNeighbor;
		return;
	}

	public bubbleObj getNeighbor(int _n)
	{
		return neighborBubble[_n] ? neighborBubble[_n] : null;
	}

	void Awake()
	{
		myID = -1;
		myState = bubbleState.none;
		myType = 0;
		chainChecker = DFS_checker.none;
		connectionChecker = DFS_checker.none;
		burstDone = false;
		fallDone = false;
	}

	void FixedUpdate()
	{
		if (getBubbleState() == bubbleState.bursting)
		{
			if (!burstDone)
			{
				burstBehavior();
			}
			
			destroyBubble();
		}

		if (getBubbleState() == bubbleState.falling)
		{
			if (!fallDone)
			{
				fallBehavior();
			}

			fallingTimer -= Time.deltaTime;

			if (fallingTimer <= 0)
			{
				destroyBubble();
			}
		}

		if (getBubbleState() == bubbleState.flying)
		{
			flyingTimer -= Time.deltaTime;

			if (flyingTimer <= 0)
			{
				destroyBubble();
			}
		}

	}

	private void OnCollisionEnter2D(Collision2D _collision)
	{
		if (getBubbleState() != bubbleState.flying)
		{
			return;
		}

		if (!_collision.gameObject.GetComponent<bubbleObj>())
		{
			return;
		}

		bubbleObj bubbleObjTemp = _collision.gameObject.GetComponent<bubbleObj>();

		if (bubbleObjTemp.getBubbleState() != bubbleState.staying)
		{
			return;
		}

		if (bubbleObjTemp.getID() < 0 || bubbleObjTemp.getID() >= BubbleManager.bubbleNum)
		{
			return;
		}

		transform.parent = _collision.transform.parent;     // Must be before becomeStayingBubble()

		becomeStayingBubble(correctPosition(bubbleObjTemp));
		BubbleManager.MatchBubble(getID());

		if (transform.parent.GetComponent<bubblePile>()) {
			transform.parent.GetComponent<bubblePile>().setBurstCount(BubbleManager.CountBurst());
		}
	}
	
	// 堆積不同的泡泡
	// --------------------------------------------------------------------------------
	private int correctPosition(bubbleObj _oldBubble)
	{
		int givenPositionID = BubbleManager.GetClosestPositionID(_oldBubble.getID(), transform.localPosition);
		transform.localPosition = BubbleManager.nodePosition[givenPositionID];
		return givenPositionID;
	}
	
	public void becomeStayingBubble(int _id)
	{
		setBubbleState(bubbleState.staying);
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
		bubbleObj myBubbleInfo = this;
		BubbleManager.BecomeNeighbor(_id, ref myBubbleInfo);
		return;
	}
	// --------------------------------------------------------------------------------

	// 消除行為
	public void burstBehavior()
	{
		BubbleManager.DeleteNodeBubble(getID());		//	先斷開Neighbor再scan
		bubblePile bubblePileTemp = transform.parent.GetComponent<bubblePile>();
		if (bubblePileTemp)
		{
			bubblePileTemp.reduceBurstCount();
			bubblePileTemp.checkBurstEnd();
			bubblePileTemp.playBurstParticle(getID());
		}
	}


	// 墜落行為
	public void fallBehavior()
	{
		GetComponent<SpriteRenderer>().sortingOrder = 2;
		GetComponent<Collider2D>().enabled = false;
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
		GetComponent<Rigidbody2D>().gravityScale = 1f;
		if (transform.parent.GetComponent<bubblePile>())
		{
			transform.parent.GetComponent<bubblePile>().reduceFallCount();
		}
	}
	
	public void destroyBubble()
	{
		BubbleManager.DeleteNodeBubble(getID());
		Destroy(this.gameObject);
	}
}
