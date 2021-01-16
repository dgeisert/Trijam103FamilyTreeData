using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard_SampleScript : MonoBehaviour
{
	public static Leaderboard_SampleScript Instance;

	public TextMeshProUGUI scoreText, rankText;
	public string leaderboardName;

	void Awake()
	{
		Instance = this;
		GameSparks.Api.Messages.NewHighScoreMessage.Listener += HighScoreMessageHandler; // assign the New High Score message
	}

	void HighScoreMessageHandler(GameSparks.Api.Messages.NewHighScoreMessage _message)
	{
		Debug.Log("NEW HIGH SCORE \n " + _message.LeaderboardName);
	}

	public void PostScoreBttn()
	{
		Debug.Log("Posting Score To Leaderboard...");
		rankText.text = "Posting Score To Leaderboard...";
		scoreText.text = "";
		new GameSparks.Api.Requests.LogEventRequest()
			.SetEventKey("SUBMIT_SCORE_" + leaderboardName)
			.SetEventAttribute("SCORE", Game.Score.ToString())
			.Send((response) =>
			{

				if (!response.HasErrors)
				{
					Debug.Log("Score Posted Sucessfully...");
				}
				else
				{
					Debug.Log("Error Posting Score...");
				}
			});
		GetLeaderboard();
	}

	public void GetLeaderboard()
	{
		Debug.Log("Fetching Leaderboard Data...");

		new GameSparks.Api.Requests.LeaderboardDataRequest()
			.SetLeaderboardShortCode(leaderboardName + "_LEADERBOARD")
			.SetEntryCount(25) // we need to parse this text input, since the entry count only takes long
			.Send((response) =>
			{

				if (!response.HasErrors)
				{
					Debug.Log("Found Leaderboard Data...");
					rankText.text = System.String.Empty; // first clear all the data from the output
					scoreText.text = System.String.Empty;
					foreach (GameSparks.Api.Responses.LeaderboardDataResponse._LeaderboardData entry in response.Data) // iterate through the leaderboard data
					{
						int rank = (int) entry.Rank; // we can get the rank directly
						string playerName = entry.UserName;
						string score = entry.JSONData["SCORE"].ToString(); // we need to get the key, in order to get the score
						rankText.text += rank + "\n"; // addd the score to the output text
						scoreText.text += score + "\n";
					}
				}
				else
				{
					Debug.Log("Error Retrieving Leaderboard Data...");
				}

			});
	}
}