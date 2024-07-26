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

public enum Season
{
    Winter,
    Spring,
    Summer,
    Autumn
}

class TimedEvent
{
    public MonoBehaviour script;
    public Action action;
    public int timeToRun = 1;
    public bool isRepeating = false;
    public int timeLeft = 1;
    public int priority = 0;
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
    public GameObject tutorialSystem;
    [SerializeField] BuildingTypeSelect buildingtypeselect;
    [SerializeField] int MaintainedSoilHealth;

    int day = 1;
    float timeElapsed = 0f;
    float tickTime = 1f;

    Month month = Month.January;
    int numOfDaysInMonth = 31;

    static Season season = Season.Winter;

    int year = 1;

    static int pauseMenus = 0;

    bool isLevelOver = false;


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
        buildingtypeselect.CloseButton();

        if (tutorialSystem)
            Pause();

        SetDay();
        SetMonth();
        SetTimeRemainingDisplay();
        AddMonthlyEvent(this, CountDownLevelTime, 1, true, 5);
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

    /** Call this method to add an event to the Time Manager's monthly tick queue
     *  Priority for monthly events:
     *      1- Resource Collection, Building Upkeep
     *      2- SoilGradeChange
     *      3- HealthBar Update Change
     *      4- Reset Soil Grade Change
     *      5- Level End/Win Conditions
     *      6- Weather for the following month
     *      
    */

    public static void AddMonthlyEvent(MonoBehaviour script, Action action, int months=1, bool repeat=true, int eventPriority=0)
    {
        monthlyEvents.Add(new TimedEvent { script = script, action = action, timeToRun = months, isRepeating = repeat, timeLeft = months, priority = eventPriority });
    }

    public void SkipMonth()
    {
        month++;
    }

    /** Runs the countdowns for every daily event in the list
     *  Runs each event when the countdown is finished
     */
    void RunEvents(List<TimedEvent> events)
    {
        List<int> indexesToRemove = new List<int>();

        events.Sort((x, y) => x.priority.CompareTo(y.priority));

        for (int i = 0; i < events.Count; i++)
        {
            // Check to see if an event's script has been deleted
            // If it has, mark the index for removal from the queue
            if (events[i].script == null)
            {
                indexesToRemove.Add(i);
                continue;
            }

            if (events[i].timeLeft > 1)
            {
                events[i].timeLeft--;
            }
            else
            {
                events[i].action();

                if (!events[i].isRepeating)
                {
                    indexesToRemove.Add(i);
                    continue;
                }
                else
                {
                    events[i].timeLeft = events[i].timeToRun;
                }
            }
        }

        for (int i = events.Count - 1; i > 0; i--)
        {
            foreach (int index in indexesToRemove)
            {
                if (i == index)
                {
                    events.RemoveAt(i);
                }
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

        //also check if soil health is maintained
        CheckSoilHealth();
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
    public void IncrementMonth()
    {
        month++;
        RunEvents(monthlyEvents);

        if (isLevelOver)
        {
            CheckSuccessConditions();
        }

        if (month > Month.December)
        {
            year++;
            month = Month.January;
        }

        monthDisplay.text = month.ToString();

        AssignDaysInMonth();
        SetSeason();
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

    /**
     *  Set season, and update UI icon and text
     *  Season is set at the end of each month, so the function checks for the month before the season begins
     */
    
    void SetSeason()
    {
        if (month == Month.February)
        {
            season = Season.Spring;
        }
        else if (month == Month.May)
        {
            season = Season.Summer;
        }
        else if (month == Month.August)
        {
            season = Season.Autumn;
        }
        else if (month == Month.November)
        {
            season = Season.Winter;
        }
    }

    void CountDownLevelTime()
    {
        Inventory.levelTime--;
        SetTimeRemainingDisplay();

        if (Inventory.levelTime < 1 )
        {
            isLevelOver = true;
        }
    }

    void CheckSuccessConditions()
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
            Inventory.HealthBarChange();
            Terrainsystem.ResetValuesSoilGrade();
            Terrainsystem.tilecount = 0;
        }
        // TODO: Stop Countdown
    }

     void CheckSoilHealth()
    {
        StartCoroutine(CheckSoilStep2());
        
    }

    IEnumerator CheckSoilStep2()
    {
        yield return new  WaitForSeconds(3);
        if (Inventory.healthBar < MaintainedSoilHealth)
        {
            Instantiate(gameOverPrefab);
            Terrainsystem.tilecount = 0;
        }
        
    }

    void SetTimeRemainingDisplay()
    {
        timeRemainingDisplay.text = Inventory.levelTime + " months";
    }

    public static void Pause()
    {
        Time.timeScale = 0;

        pauseMenus++;
    }

    public static void Unpause()
    {
        pauseMenus--;

        if (pauseMenus <= 0)
            Time.timeScale = 1f;
    }

    public static Season GetCurrentSeason()
    {
        return season;
    }

    private void OnDestroy()
    {
        dailyEvents = new List<TimedEvent>();
        monthlyEvents = new List<TimedEvent>();
    }
}
