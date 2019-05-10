using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class outsideButtonScript : MonoBehaviour
{
    public GameObject[] _e = new GameObject[5];
    public elevatorScript[] e = new elevatorScript[5];
    public GameObject Text;
    public ArrayList floor_waitup_out = new ArrayList();
    public ArrayList floor_waitdown_out = new ArrayList();
    // Start is called before the first frame update
    void Start()
    {
        e[0] = _e[0].GetComponent<elevatorScript>();
        e[1] = _e[1].GetComponent<elevatorScript>();
        e[2] = _e[2].GetComponent<elevatorScript>();
        e[3] = _e[3].GetComponent<elevatorScript>();
        e[4] = _e[4].GetComponent<elevatorScript>();

        //test:
        //InvokeRepeating("printTest", 1, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        string str = "floor_wait_up:";
        foreach (int i in floor_waitup_out)
        {
            str = str + i.ToString() + ' ';
        }
        str += "\nfloor_wait_down:";
        foreach (int i in floor_waitdown_out)
        {
            str = str + i.ToString() + ' ';
        }
        Text.GetComponent<Text>().text = str;
    }

    public void UP(int f)
    {
        print("haha");
        int tarEle = findProperElevator(f, 1);
        debug(f, tarEle);   //debug
        if (tarEle != 5)
        {
            _e[tarEle].SendMessage("AddTask", f);
        }
        else
        {
            floor_waitup_out.Add(f);
        }
    }

    public void DOWN(int f)
    {
        int tarEle = findProperElevator(f, 2);
        debug(f, tarEle);   //debug
        if (tarEle != 5)
        {
            _e[tarEle].SendMessage("AddTask", f);
        }
        else
        {
            floor_waitdown_out.Add(f);
        }
    }

    int findProperElevator(int f, int wantState)
    {
        //targetEle==5:no such elevator meet your demand.

        print("sadfiudhfaksdhasdkfhaskjd!!!!!!!!");
        int targetEle = 5;
        if (wantState == 1)     //wants up
        {
            print("sadfiudhfaksdhasdkfhaskjd!!!!!!!!");
            bool find_up = false;
            int difference_up = 99;
            int difference_wait = 99;
            for (int i = 0; i < 5; i++)
            {
                print("sadfiudhfaksdhasdkfhaskjd!!!!!!!!");
                if (e[i].state == 1)
                {
                    print("i'm in 1");
                    if (e[i].floor_current < f || (e[i].floor_current == f && e[i].animated == true && e[i].animatedTime < 1.1f)) 
                    {
                        print("i'm in 2");
                        int dif = Math.Abs(f - e[i].floor_current);
                        if (dif < difference_up)
                        {
                            print("i'm in 3");
                            find_up = true;
                            targetEle = i;
                            difference_up = dif;
                        }
                    }
                }
                else if (e[i].state == 0)
                {
                    if (find_up == false)
                    {
                        int dif = Math.Abs(f - e[i].floor_current);
                        if (dif < difference_wait)
                        {
                            targetEle = i;
                            difference_wait = dif;                        }
                    }
                }
            }
        }
        else if(wantState==2)   //wants dowm
        {
            bool find_down = false;
            int difference_down = 99;
            int difference_wait = 99;
            for (int i = 0; i < 5; i++)
            {
                if (e[i].state == 2)
                {
                    if (e[i].floor_current > f || (e[i].floor_current == f && e[i].animated == true && e[i].animatedTime < 1.1f))
                    {
                        int dif = Math.Abs(e[i].floor_current - f);
                        if (dif < difference_down)
                        {
                            find_down = true;
                            targetEle = i;
                            difference_down = dif;
                        }
                    }
                }

                if (e[i].state == 0)
                {
                    if (find_down == false)
                    {
                        int dif = Math.Abs(e[i].floor_current - f);
                        if (dif < difference_wait)
                        {
                            targetEle = i;
                            difference_wait = dif;
                        }
                    }
                }
            }
        }

        return targetEle;
    }
    void debug(int f, int e)
    {
        print("i dispatch Floor(" + f + ") to Ele(" + e + ')');
    }
}


