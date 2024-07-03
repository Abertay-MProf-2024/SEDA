using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

enum Month
{
    January,
    February,
    March,
    April,
    May,
    June,
    July,
    August,
    September,
    October,
    November,
    December
}

class TimedEvent
{
    public Action action;
    public int timeToRun = 1;
    public bool isRepeating = false;
    public int timeLeft = 1;
}

public class TimeSystem : MonoBehaviour
{
    // the active instance of the time system
    private static TimeSystem instance;

    // The lists of timed events to be run on ticks
    static List<TimedEvent> dailyEvents = new List<TimedEvent>();
    static List<TimedEvent> monthlyEvents = new List<TimedEvent>();

    // UI display
    [SerializeField] TextMeshProUGUI dayDisplay;
    [SerializeField] TextMeshProUGUI monthDisplay;
    [SerializeField] TextMeshProUGUI timeRemainingDisplay;

    // UI prefabs
    [SerializeField] GameObject gameOverPrefab;
    [SerializeField] GameObject levelCompletePrefab;
    [SerializeField] GameObject winScreenPrefab;

    int day = 1;
    float timeElapsed = 0f;
    float tickTime = 1f;

    Month month = Month.January;
    int numOfDaysInMonth = 31;

    int year = 1;


    /** The time manager is a singleton
     *  There is only one time manager active at any given time
     */
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /** Initialise displays and start the daily tick */
    private void Start()
    {
        SetDay();
        SetMonth();
        SetTimeRemainingDisplay();
        AddMonthlyEvent(CountDownLevelTime);
        AddMonthlyEvent(Inventory.SetWeather);
        StartCoroutine(DailyTick());
    }

    // Update is called once per frame
    void Update()
    {
        // Timer
        timeElapsed += Time.deltaTime;
    }

    /** Call this method to add an event to the Time Manager's daily tick queue */
    public static void AddDailyEvent(Action action, int days=1, bool repeat=true)
    {
        dailyEvents.Add(new TimedEvent{ action = action, timeToRun = days, isRepeating = repeat, timeLeft = days });
    }

    /** Call this method to add an event to the Time Manager's monthly tick queue */
    public static void AddMonthlyEvent(Action action, int months=1, bool repeat=true)
    {
        monthlyEvents.Add(new TimedEvent { action = action, timeToRun = months, isRepeating = repeat, timeLeft = months });
    }

    /** Runs the countdowns for every daily event in the list
     *  Runs each event when the countdown is finished
     */
    void RunEvents(List<TimedEvent> events)
    {
        for (int i = 0; i < events.Count; i++)
        {
            if (events[i].timeLeft > 1)
            {
                events[i].timeLeft--;
            }
            else
            {
                events[i].action();

                if (!events[i].isRepeating)
                {
                    events.RemoveAt(i);
                }
                else
                {
                    events[i].timeLeft = events[i].timeToRun;
                }

                continue;
            }
        }
    }

    /** The coroutine for the daily tick */
    IEnumerator DailyTick()
    {
        while(true)
        {
            SetDay();
            RunEvents(dailyEvents);
            yield return new WaitForSeconds(tickTime);
        }
    }

    /** Set day, based on timeElapsed, and  */
    void SetDay()
    {
        // Set day
        day = Mathf.FloorToInt(timeElapsed * tickTime) + 1;    // Day starts at 1
        dayDisplay.text = day.ToString();

        SetMonth();
    }

    /** Set month based on numofDaysInMonth */
    void SetMonth()
    {
        monthDisplay.text = month.ToString();

        if (day > numOfDaysInMonth)
        {
            timeElapsed -= (day - 1);
            day = 1;
            dayDisplay.text = day.ToString();
            IncrementMonth();
        }
    }

    /** Increment month (and year, rolling over from December to January) */
    void IncrementMonth()
    {
        month++;
        RunEvents(monthlyEvents);

        if (month > Month.December)
        {
            year++;
            month = Month.January;
        }

        monthDisplay.text = month.ToString();

        AssignDaysInMonth();
    }


    /** Change the number of days per month */
    void AssignDaysInMonth()
    {
        if (month == Month.February)
        {
            if (year % 4 == 0) 
                numOfDaysInMonth = 29;
            else numOfDaysInMonth = 28;
        }
        else if (month == Month.April || month == Month.June || month == Month.September || month == Month.November)
        {
            numOfDaysInMonth = 30;
        }
        else
        {
            numOfDaysInMonth = 31;
        }
    }

    void CountDownLevelTime()
    {
        Inventory.levelTime--;
        SetTimeRemainingDisplay();

        if (Inventory.levelTime < 1 )
        {
            if (LevelManager.AreSuccessConditionsMet())
            {
                // Win the level
                GameManager.levelsCompleted++;

                if (GameManager.levelsCompleted >= 3)
                {
                    Instantiate(winScreenPrefab);
                }
                else
                {
                    Instantiate(levelCompletePrefab);
                }
            }    
            else
            {
                // Lose the level
                Instantiate(gameOverPrefab);
            }
            // TODO: Stop Countdown
        }
    }

    void SetTimeRemainingDisplay()
    {
        timeRemainingDisplay.text = Inventory.levelTime + " months";
    }

    public static void Pause()
    {
        Time.timeScale = 0;
    }

    public static void Unpause()
    {
        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        dailyEvents = new List<TimedEvent>();
        monthlyEvents = new List<TimedEvent>();
    }
}
