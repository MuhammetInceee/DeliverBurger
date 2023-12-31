using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager 
{
    public static UnityEvent OnLevelStart = new UnityEvent();
    public static UnityEvent OnOpenWinPanel = new UnityEvent();
    public static UnityEvent OnOpenFailPanel = new UnityEvent();
    public static UnityEvent OnStructurePartDroped = new UnityEvent();
    public static UnityEvent OnFoodThrowed = new UnityEvent();
    public static TotalStructurePartNotifyEvent OnTotalStructurePartNotify = new TotalStructurePartNotifyEvent();
    public static TotalFoodCountNotifyEvent OnTotalFoodCountNotify = new TotalFoodCountNotifyEvent();

    public class TotalStructurePartNotifyEvent : UnityEvent<int> { }
    public class TotalFoodCountNotifyEvent : UnityEvent<int> { }
}
