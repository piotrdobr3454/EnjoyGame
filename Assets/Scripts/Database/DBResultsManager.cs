﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBResultsManager : DBManager
{
    Result result;
    Result[] results;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(GetTop10());
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(authHeader);
            Debug.Log(userId);
        }
    }

    public IEnumerator SaveResults(int[] results)
    {
        string score = "{\"score\":\"" + results[8] + "\"," +
                        "\"stageNumber\":\"" + PlayerPrefs.GetInt("stage", 1) + "\"," +
                        "\"easyCorrect\":\"" + results[0] + "\"," +
                        "\"mediumCorrect\":\"" + results[2] + "\"," +
                        "\"hardCorrect\":\"" + results[4] + "\"," +
                        "\"easyTotal\":\"" + results[0] + results[1] + "\"}," +
                        "\"mediumTotal\":\"" + results[2] + results[3] + "\"}," +
                        "\"hardTotal\":\"" + results[4] + results[5] + "\"}";
        yield return StartCoroutine(PostData("/users/"+userId+"/results", score));
        Debug.Log("Score submitted!");
    }

    public IEnumerator GetUserResults(int stage=-1)
    {
        if (userId == null)
            Debug.Log("Log in first");
        else
        {
            string resultString = "";
            if (stage == -1)
            {
                yield return StartCoroutine(GetData("/users/" + userId + "/summary-report-all", callback: data => resultString = data));
                results = JsonHelper.FromJson<Result>(resultString);
            }
            else {
                yield return StartCoroutine(GetData("/users/" + userId + "/summary-report?stageNumber=" + stage, callback: data => resultString = data));
                result = JsonUtility.FromJson<Result>(resultString);

                int easy = result.easyCorrect;
                int medium = result.mediumCorrect;
                int hard = result.hardCorrect;
                int total = easy + medium + hard;
                int easyPercent = easy / total * 100;
                int mediumPercent = medium / total * 100;
                int hardPercent = hard / total * 100;
                string[] arr = { total.ToString(), easyPercent.ToString(), mediumPercent.ToString(), hardPercent.ToString() };

                PlayerPrefs.SetString("pastResults", string.Join(",", arr));
            }
        }
    }

    public IEnumerator GetTop10()
    {
        if (userId == null)
            Debug.Log("Log in first");
        else
        {
            string resultString = "";
            yield return StartCoroutine(GetData("/results/top10", callback: data => resultString = data));
            results = JsonHelper.FromJson<Result>(resultString);

            for (int i=0; i<results.Length; i++)
            {
                Result r = results[i];
                Debug.Log(r.score);
            }
        }
    }
}

[System.Serializable]
public class Result
{
    public string userId;
    public int score;
    public int stageNumber;
    public int easyCorrect;
    public int mediumCorrect;
    public int hardCorrect;
    public int easyTotal;
    public int mediumTotal;
    public int hardTotal;
}