using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BubbleManager
{
	public static bool gridBilt = false;
	public const float bubbleRadius = 0.3f;
	public const int minNumOfChain = 3;

	public const int bubbleCol = 9, maxBubbleRaw = 41, bubbleNum = bubbleCol * maxBubbleRaw - (maxBubbleRaw / 2);
	public static Vector2[] nodePosition = new Vector2[bubbleNum];
	public static bubbleObj[] nodeBubble = new bubbleObj[bubbleNum];

	private static int numOfBurst = 0, numOfShoot = 0, numOfFall = 0;

	// �إߺ���
	// --------------------------------------------------------------------------------
	public static void InitialEmptyGrid()
	{
		for (int i = 0; i < bubbleNum; i++)
		{
			if (i % (bubbleCol * 2 - 1) < bubbleCol)
			{
				// ��������������	<---
				//  ������������
				nodePosition[i] = new Vector2(
											(i % (bubbleCol * 2 - 1) - (bubbleCol / 2)) * 2 * bubbleRadius, 
											-(i / (bubbleCol * 2 - 1)) * 4 * bubbleRadius * Mathf.Sin(60 * Mathf.Deg2Rad) - bubbleRadius
											);
			}
			else
			{
				// ��������������
				//  ������������	<---
				nodePosition[i] = new Vector2(
											(i % (bubbleCol * 2 - 1) - bubbleCol - (bubbleCol / 2)) * 2 * bubbleRadius + bubbleRadius,
											-(i / (bubbleCol * 2 - 1)) * 4 * bubbleRadius * Mathf.Sin(60 * Mathf.Deg2Rad) - 2 * Mathf.Sin(60 * Mathf.Deg2Rad) * bubbleRadius - bubbleRadius
											);
			}
			
		}
		
		gridBilt = true;
		numOfBurst = 0;
		numOfShoot = 0;
		numOfFall = 0;
		return;
	}
	// --------------------------------------------------------------------------------

	// �M��Q���w�w�P��̪񪺪Ŧ�
	// --------------------------------------------------------------------------------
	public static int GetClosestPositionID(int _oldBubbleID, Vector2 _newPosition)
	{
		float minDistance = bubbleRadius * 1000;
		int closestPosition = bubbleNum, tempID;

		tempID = _oldBubbleID - bubbleCol;
		if (tempID >= 0 &&                                          //  ����
			!nodeBubble[tempID] && 									// ���桳
			_oldBubbleID % (bubbleCol * 2 - 1) != 0)				//  ����
		{
			if (minDistance > Vector2.Distance(_newPosition, nodePosition[tempID]))
			{
				minDistance = Vector2.Distance(_newPosition, nodePosition[tempID]);
				closestPosition = tempID;
			}
		}

		tempID = _oldBubbleID - (bubbleCol - 1);
		if (tempID >= 0 &&                                          //  ����
			!nodeBubble[tempID] &&									// ���桳
			_oldBubbleID % (bubbleCol * 2 - 1) != 8)				//  ����
		{
			if (minDistance > Vector2.Distance(_newPosition, nodePosition[tempID]))
			{
				minDistance = Vector2.Distance(_newPosition, nodePosition[tempID]);
				closestPosition = tempID;
			}
		}
		
		tempID = _oldBubbleID - 1;
		if (tempID >= 0 &&                                          //  ����
			!nodeBubble[tempID] &&									// ���桳
			_oldBubbleID % (bubbleCol * 2 - 1) != 0 &&				//  ����
			_oldBubbleID % (bubbleCol * 2 - 1) != 9)
		{
			if (minDistance > Vector2.Distance(_newPosition, nodePosition[tempID]))
			{
				minDistance = Vector2.Distance(_newPosition, nodePosition[tempID]);
				closestPosition = tempID;
			}
		}
		
		tempID = _oldBubbleID + 1;
		if (tempID < bubbleNum &&                                   //  ����
			!nodeBubble[tempID] &&									// ���桴
			_oldBubbleID % (bubbleCol * 2 - 1) != 8 &&				//  ����
			_oldBubbleID % (bubbleCol * 2 - 1) != 16)
		{
			if (minDistance > Vector2.Distance(_newPosition, nodePosition[tempID]))
			{
				minDistance = Vector2.Distance(_newPosition, nodePosition[tempID]);
				closestPosition = tempID;
			}
		}
		
		tempID = _oldBubbleID + (bubbleCol - 1);
		if (tempID < bubbleNum &&                                   //  ����
			!nodeBubble[tempID] &&									// ���桳
			_oldBubbleID % (bubbleCol * 2 - 1) != 0)				//  ����
		{
			if (minDistance > Vector2.Distance(_newPosition, nodePosition[tempID]))
			{
				minDistance = Vector2.Distance(_newPosition, nodePosition[tempID]);
				closestPosition = tempID;
			}
		}
		
		tempID = _oldBubbleID + bubbleCol;
		if (tempID < bubbleNum &&                                   //  ����
			!nodeBubble[tempID] &&									// ���桳
			_oldBubbleID % (bubbleCol * 2 - 1) != 8)				//  ����
		{
			if (minDistance > Vector2.Distance(_newPosition, nodePosition[tempID]))
			{
				minDistance = Vector2.Distance(_newPosition, nodePosition[tempID]);
				closestPosition = tempID;
			}
		}

		return closestPosition;
	}
	// --------------------------------------------------------------------------------

	// �P�P��w�w�إ����Y
	// --------------------------------------------------------------------------------
	public static void BecomeNeighbor(int _id, ref bubbleObj _newBubble)
	{
		nodeBubble[_id] = _newBubble;
		_newBubble.setID(_id);
		int tempID;
		
		tempID = _id - bubbleCol;
		if (tempID >= 0 &&                      //  ����
			nodeBubble[tempID] &&				// ���桳
			_id % (bubbleCol * 2 - 1) != 0)		//  ����
		{
			nodeBubble[tempID].setNeighbor(5, _newBubble);
			_newBubble.setNeighbor(0, nodeBubble[tempID]);
		}
		
		tempID = _id - (bubbleCol - 1);
		if (tempID >= 0 &&                      //  ����
			nodeBubble[tempID] &&				// ���桳
			_id % (bubbleCol * 2 - 1) != 8)		//  ����
		{
			nodeBubble[tempID].setNeighbor(4, _newBubble);
			_newBubble.setNeighbor(1, nodeBubble[tempID]);
		}
		
		tempID = _id - 1;
		if (tempID >= 0 &&                      //  ����
			nodeBubble[tempID] &&				// ���桳
			_id % (bubbleCol * 2 - 1) != 0 &&	//  ����
			_id % (bubbleCol * 2 - 1) != 9)
		{
			nodeBubble[tempID].setNeighbor(3, _newBubble);
			_newBubble.setNeighbor(2, nodeBubble[tempID]);
		}
		
		tempID = _id + 1;
		if (tempID < bubbleNum &&               //  ����
			nodeBubble[tempID] &&				// ���桴
			_id % (bubbleCol * 2 - 1) != 8 &&	//  ����
			_id % (bubbleCol * 2 - 1) != 16)
		{
			nodeBubble[tempID].setNeighbor(2, _newBubble);
			_newBubble.setNeighbor(3, nodeBubble[tempID]);
		}
		
		tempID = _id + (bubbleCol - 1);
		if (tempID < bubbleNum &&               //  ����
			nodeBubble[tempID] &&				// ���桳
			_id % (bubbleCol * 2 - 1) != 0)		//  ����
		{
			nodeBubble[tempID].setNeighbor(1, _newBubble);
			_newBubble.setNeighbor(4, nodeBubble[tempID]);
		}
		
		tempID = _id + bubbleCol;
		if (tempID < bubbleNum &&               //  ����
			nodeBubble[tempID] &&				// ���桳
			_id % (bubbleCol * 2 - 1) != 8)		//  ����
		{
			nodeBubble[tempID].setNeighbor(0, _newBubble);
			_newBubble.setNeighbor(5, nodeBubble[tempID]);
		}

		return;
	}
	// --------------------------------------------------------------------------------

	// �����P�˪��w�w
	// --------------------------------------------------------------------------------
	public static void MatchBubble(int _id)
	{
		int chainCounter = 0;

		numOfShoot++;

		// �S��w�w
		if ((nodeBubble[_id].getBubbleType() >= bubbleUI.bubbleType.special_0))
		{
			for (int i = 0; i < (int)bubbleUI.bubbleType.special_0; i++)
			{
				chainCounter = 0;
				BubbleChain((bubbleUI.bubbleType)i, _id, ref chainCounter);
		
				if (chainCounter >= minNumOfChain)
				{
					DoBurst();
				}

				ResetDFS();
			}

			return;
		}
		// ----------

		BubbleChain(nodeBubble[_id].getBubbleType(), _id, ref chainCounter);

		if (chainCounter >= minNumOfChain)
		{
			DoBurst();
		}
		
		ResetDFS();

		return;
	}

	// �HDFS�T�{�s�����ۦP�w�w
	private static void BubbleChain(bubbleUI.bubbleType _type, int _id, ref int _i)
	{
		if (!nodeBubble[_id])
		{
			return;
		}

		if (nodeBubble[_id].getChainChecker() != bubbleObj.DFS_checker.none)
		{
			return;
		}

		if (nodeBubble[_id].getBubbleType() != _type && nodeBubble[_id].getBubbleType() < bubbleUI.bubbleType.special_0)
		{
			nodeBubble[_id].setChainChecker(bubbleObj.DFS_checker.excluded);
			return;
		}

		nodeBubble[_id].setChainChecker(bubbleObj.DFS_checker.included);
		_i++;

		for (int i = 0; i < 6; i++)
		{
			if (!nodeBubble[_id].getNeighbor(i))
			{
				continue;
			}

			BubbleChain(_type, nodeBubble[_id].getNeighbor(i).getID(), ref _i);

			// bomb
			if (nodeBubble[_id].getBubbleType() == bubbleUI.bubbleType.special_1)
			{
				nodeBubble[_id].getNeighbor(i).setChainChecker(bubbleObj.DFS_checker.included);
			}
			// ----------
		}

		return;
	}
	// --------------------------------------------------------------------------------
	
	// �ˬd�w�w�s�����p
	// --------------------------------------------------------------------------------
	public static bool ScanBubble()
	{
		bool checker = false;		// �Y�̤W��w�L�w�w(����������) �^��false
		for (int i = 0; i < bubbleCol; i++)
		{
			if (!checker && nodeBubble[i])
			{
				checker = true;
			}

			BubbleConnection(i);
		}
		
		DoFall();
		ResetDFS();
		return checker;
	}

	// �HDFS�T�{�w�w�O�_�P�̤W��s��
	private static void BubbleConnection(int _id)
	{
		if (!nodeBubble[_id])
		{
			return;
		}

		if (nodeBubble[_id].getConnectionChecker() != bubbleObj.DFS_checker.none)
		{
			return;
		}
		
		nodeBubble[_id].setConnectionChecker(bubbleObj.DFS_checker.included);

		for (int i = 0; i < 6; i++)
		{
			if (!nodeBubble[_id].getNeighbor(i))
			{
				continue;
			}

			BubbleConnection(nodeBubble[_id].getNeighbor(i).getID());
		}

		return;
	}
	// --------------------------------------------------------------------------------

	public static bool CheckState()
	{
		for (int i = 0; i < bubbleNum; i++)
		{
			if (!nodeBubble[i])
			{
				continue;
			}

			if (nodeBubble[i].getBubbleState() != bubbleUI.bubbleState.staying)
			{
				return false;
			}
		}

		return true;
	}

	// �NDFS���G�ର �������q
	public static void DoBurst()
	{
		for (int i = 0; i < bubbleNum; i++)
		{
			if (!nodeBubble[i])
			{
				continue;
			}

			if (nodeBubble[i].getChainChecker() == bubbleObj.DFS_checker.included)
			{
				numOfBurst++;
				nodeBubble[i].setBubbleState(bubbleUI.bubbleState.bursting);
			}
		}

		return;
	}

	// �έp�����w�w
	public static int CountBurst()
	{
		int sum = 0;
		for (int i = 0; i < bubbleNum; i++)
		{
			if (!nodeBubble[i])
			{
				continue;
			}

			if (nodeBubble[i].getBubbleState() == bubbleUI.bubbleState.bursting)
			{
				sum++;
			}
		}

		return sum;
	}

	// �NDFS���G�ର �Y�����q
	public static void DoFall()
	{
		for (int i = 0; i < bubbleNum; i++)
		{
			if (!nodeBubble[i])
			{
				continue;
			}

			if (nodeBubble[i].getConnectionChecker() == bubbleObj.DFS_checker.none)
			{
				numOfFall++;
				nodeBubble[i].setBubbleState(bubbleUI.bubbleState.falling);
			}
		}

		return;
	}

	// �έp�Y���w�w
	public static int CountFall()
	{
		int sum = 0;
		for (int i = 0; i < bubbleNum; i++)
		{
			if (!nodeBubble[i])
			{
				continue;
			}

			if (nodeBubble[i].getBubbleState() == bubbleUI.bubbleState.falling)
			{
				sum++;
			}
		}

		return sum;
	}

	public static int GetNumOfBurst()
	{
		return numOfBurst;
	}

	public static int GetNumOfShoot()
	{
		return numOfShoot;
	}

	public static int GetNumOfFall()
	{
		return numOfFall;
	}

	public static void ResetCounter()
	{
		numOfBurst = 0;
		numOfShoot = 0;
		numOfFall = 0;
		return;
	}

	public static void ResetDFS()
	{
		for (int i = 0; i < bubbleNum; i++)
		{
			if (!nodeBubble[i])
			{
				continue;
			}

			nodeBubble[i].setChainChecker(0);
			nodeBubble[i].setConnectionChecker(0);
		}

		return;
	}

	public static void DeleteNodeBubble(int _id) {

		if (_id < 0 || _id >= bubbleNum)
		{
			return;
		}

		if (!nodeBubble[_id])
		{
			return;
		}

		for (int i = 0; i < 6; i++)
		{
			if (!nodeBubble[_id].getNeighbor(i))
			{
				continue;
			}

			nodeBubble[_id].getNeighbor(i).setNeighbor(5 - i, null);
		}

		nodeBubble[_id] = null;

		return;
	}

}
