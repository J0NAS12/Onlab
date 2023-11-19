using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    private static System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);

    private static double delta = 0;
    private static double startTime = 0;


    public static double getTime(){
        return (System.DateTime.UtcNow - epochStart).TotalSeconds - delta;
    }

    public static void synchronize(double timestamp){
        double endTime = (System.DateTime.UtcNow - epochStart).TotalSeconds;
        delta = ((startTime + endTime) / 2.0) - timestamp;
        Debug.Log("delta: "+ delta);
    }

    class ClockObject{
        public string method = "clock";
    }

    public static void startSync(){
        startTime = (System.DateTime.UtcNow - epochStart).TotalSeconds;
        GameValues.socket.Send(JsonUtility.ToJson(new ClockObject()));
        Debug.Log("startTime: " + startTime);
    }

}
