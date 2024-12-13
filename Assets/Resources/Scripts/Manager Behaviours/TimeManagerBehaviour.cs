using UnityEngine;

public class TimeManagerBehaviour : MonoBehaviour
{

    public float time; // Measured in seconds
    public int timeMultiplier;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time = 0;
        
    }

    public void incrementTime(float amount){
        time += amount;
    }

    public float getTime(){
        return time;
    }

    // Convert the time to a string of year, month, day, hour, minute, second
    public string getTimeAsString(){
        int year = (int)time / 31536000;
        int month = (int)(time - year * 31536000) / 2592000;
        int day = (int)(time - year * 31536000 - month * 2592000) / 86400;
        int hour = (int)(time - year * 31536000 - month * 2592000 - day * 86400) / 3600;
        int minute = (int)(time - year * 31536000 - month * 2592000 - day * 86400 - hour * 3600) / 60;
        int second = (int)(time - year * 31536000 - month * 2592000 - day * 86400 - hour * 3600 - minute * 60);
        return year + " years, " + month + " months, " + day + " days, " + hour + " hours, " + minute + " minutes, " + second + " seconds";
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * timeMultiplier;
        
    }
}
