using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
	public enum GameState
	{
		setting, playing, pausing, ending
	}
	private static GameState myGameState = GameState.setting;
	private static string userName = "";
	private static int userScore = 0;
	private static bool winGame = false;

	public static void SetGameState(GameState _state)
	{
		myGameState = _state;
		return;
	}

	public static GameState GetGameState()
	{
		return myGameState;
	}

	public static void SetUserName(string _s)
	{
		userName = _s;
		return;
	}

	public static string GetUserName()
	{
		return userName;
	}

	public static void SetUserScore(int _i)
	{
		userScore = _i;
		return;
	}

	public static int GetUserScore()
	{
		return userScore;
	}

	public static void SetResult(bool _win)
	{
		winGame = _win;
		SetGameState(GameState.ending);
		return;
	}

	public static bool GetResult()
	{
		return winGame;
	}
}
