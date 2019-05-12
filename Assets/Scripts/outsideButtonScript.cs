using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class outsideButtonScript : MonoBehaviour
{
    public GameObject[] _e = new GameObject[5];
    private elevatorScript[] e = new elevatorScript[5];
    public ArrayList tasksup = new ArrayList();
    public ArrayList tasksdown = new ArrayList();
    public GameObject Text;
    // Start is called before the first frame update
    void Start()
    {
        e[0] = _e[0].GetComponent<elevatorScript>();
        e[1] = _e[1].GetComponent<elevatorScript>();
        e[2] = _e[2].GetComponent<elevatorScript>();
        e[3] = _e[3].GetComponent<elevatorScript>();
        e[4] = _e[4].GetComponent<elevatorScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //update text content
        string str = "up:";
        foreach (int i in tasksup)
        {
            str = str + i.ToString() + ' ';
        }
        str += "\ndown:";
        foreach (int i in tasksdown)
        {
            str = str + i.ToString() + ' ';
        }
        Text.GetComponent<Text>().text = str;

        //dispatch waiting tasks
        foreach (int i in tasksup)
        {
            tasksup.Remove(i);
            UP(i);
        }
        foreach (int i in tasksdown)
        {
            tasksdown.Remove(i);
            DOWN(i);
        }
    }

    public void UP(int floor)
    {
        int target = 5;
        bool findup = false;
        bool findbreak = false;
        int difup = 99;
        int difbreak = 99;
        for(int i = 0; i < 5; i++)
        {
            if (findup == false && e[i].state == 0)
            {
                findbreak = true;
                int dif = Math.Abs(floor - e[i].current);
                if (dif < difbreak)
                {
                    target = i;
                    difbreak = dif;
                }
            }
            else if (e[i].state == 1)
            {
                if (e[i].current < floor || (e[i].current == floor && e[i].isAbleToDoors == true))
                {
                    findup = true;
                    int dif = Math.Abs(floor - e[i].current);
                    if (dif < difup)
                    {
                        target = i;
                        difup = dif;
                    }
                }
            }
        }

        if (findup || findbreak)
        {
            _e[target].SendMessage("AddTasksup", floor);
        }
        else
        {
            tasksup.Add(floor);
        }
    }

    public void DOWN(int floor)
    {
        int target = 5;
        bool finddown = false;
        bool findbreak = false;
        int difdown = 99;
        int difbreak = 99;
        for (int i = 0; i < 5; i++)
        {
            if (finddown == false && e[i].state == 0)
            {
                findbreak = true;
                int dif = Math.Abs(floor - e[i].current);
                if (dif < difbreak)
                {
                    target = i;
                    difbreak = dif;
                }
            }
            else if (e[i].state == 2)
            {
                if (e[i].current > floor || (e[i].current == floor && e[i].isAbleToDoors == true))
                {
                    finddown = true;
                    int dif = Math.Abs(floor - e[i].current);
                    if (dif < difdown)
                    {
                        target = i;
                        difdown = dif;
                    }
                }
            }
        }

        if (finddown || findbreak)
        {
            _e[target].SendMessage("AddTasksdown", floor);
        }
        else
        {
            tasksdown.Add(floor);
        }
    }
}
