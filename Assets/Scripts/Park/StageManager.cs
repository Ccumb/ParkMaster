using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private Goal[] goals;
    private int notClearNum;

    // Start is called before the first frame update
    void Start()
    {
        goals = FindObjectsOfType<Goal>();
        notClearNum = goals.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckEveryCarInGoal()
    {
        for(int i = 0; i < goals.Length; i++)
        {
            if(goals[i].IsCarHere() == false)
            {
                return;
            }
        }

        ClearGame();
    }

    private void ClearGame()
    {
        CarManager[] carManagers = FindObjectsOfType<CarManager>();

        for(int i = 0; i < carManagers.Length; i++)
        {
            carManagers[i].TurnOffCollider();
        }
    }
}
