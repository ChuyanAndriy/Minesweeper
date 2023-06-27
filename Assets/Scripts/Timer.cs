using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timeCounter;

    private TimeSpan _timePlaying;
    private bool _isTimerGoing;

    private float _elapsedTime;

    /// <summary>
    /// Method that starts the timer.
    /// </summary>
    public void BeginTimer()
    {
        _isTimerGoing = true;
        _elapsedTime = 0f;

        StartCoroutine(UpdateTimer());
    }

    /// <summary>
    /// Method that stops the timer.
    /// </summary>
    public void EndTimer()
    {
        _isTimerGoing = false;
    }

    /// <summary>
    /// Method that counts the time and displays it.
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateTimer()
    {
        while (_isTimerGoing)
        {
            _elapsedTime += Time.deltaTime;
            _timePlaying = TimeSpan.FromSeconds(_elapsedTime);
            string timePlayingStr = "Time: " + _timePlaying.ToString("mm':'ss'.'ff");
            timeCounter.text = timePlayingStr;

            yield return null;
        }
    }

    private void Start()
    {
        timeCounter.text = "Time: 00:00.00";
        _isTimerGoing = false;
    }
}
