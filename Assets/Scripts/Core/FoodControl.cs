using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodControl : MonoBehaviour
{
    private int totalFood;
    private bool isLevelSuccess;

    private void Start()
    {
        totalFood = transform.childCount +1;
        EventManager.OnTotalFoodCountNotify.Invoke(totalFood);
    }

    private void OnEnable()
    {
        EventManager.OnFoodThrowed.AddListener(()=>StartCoroutine(CheckHumans()));
        EventManager.OnOpenWinPanel.AddListener(() => isLevelSuccess = true);
    }
    private void OnDisable()
    {
        EventManager.OnFoodThrowed.RemoveListener(() => StartCoroutine(CheckHumans()));
        EventManager.OnOpenWinPanel.RemoveListener(() => isLevelSuccess = true);

    }

    private IEnumerator CheckHumans()
    {
        yield return new WaitForSeconds(8);
        totalFood--;
        if (totalFood == 0 && !isLevelSuccess) {

            EventManager.OnOpenFailPanel.Invoke();
        }
    }
}
