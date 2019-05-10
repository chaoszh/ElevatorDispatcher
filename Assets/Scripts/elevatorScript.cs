using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.UI;
using UnityEngine;

public class elevatorScript: MonoBehaviour
{
    public GameObject outsideButton;
    public GameObject floorButton;
    public GameObject floorShower;
    public RectTransform rect;
    public Animator animator;
    public outsideButtonScript outScript;

    public GameObject Text;

    public int state = 0;
    public bool animated = false;
    public int stateReminder = 0;
    enum State{
        Wait,
        Ascend,
        Descend
    }
    /* state表示电梯运行的状况：
     * 0：等待
     * 1：上升
     * 2：下降
     */
    public int floor_current = 1;
    /* 
     * pos表示电梯现在所在的层数
     */
    public ArrayList floor = new ArrayList();
    /* 
     * floor表示电梯的目标层集合
     */
    public ArrayList floor_wait = new ArrayList();
    
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        animator = transform.GetComponent<Animator>();
        outScript = outsideButton.GetComponent<outsideButtonScript>();
        InvokeRepeating("run", 0, 0.03f);
    }

    // Update is called once per frame
    void Update()
    {
        string str = "";
        foreach (int i in floor)
        {
            str = str + i.ToString() + ' ';
        }
        Text.GetComponent<Text>().text = str;
    }


    public void AddTask(int f)
    {
        if (floor.Contains(f))
        {
            return;
        }

        if (state == 0)
        {
            if (f > floor_current) state = 1;
            else if (f < floor_current) state = 2;
            floor.Add(f);
            return;
        }

        int size = floor.Count;
        for (int i = 0; i < size; i++)
        {
            if (state == 1)
            {
                if (floor_current > f)
                {
                    floor_wait.Add(f);
                    debug();    //debug
                    return;
                }
                else if(floor_current == f)
                {
                    if(animated==true)
                    {
                        print("animated");
                    }
                    else
                    {
                        print("b");
                        floor_wait.Add(f);
                    }
                }
                else if ((int)floor[i] > f)
                {
                    floor.Insert(i, f);
                    debug();    //debug
                    return;
                }
            }
            else if (state == 2)
            {
                if (floor_current < f)
                {
                    floor_wait.Add(f);
                    debug();    //debug
                    return;
                }
                else if ((int)floor[i] < f)
                {
                    floor.Insert(i, f);
                    debug();    //debug
                    return;
                }
            }
        }
        floor.Add(f);
        debug();    //debug
        return;
    }
    public void RefillTask(int passtate)
    {
        if (passtate == 1)
        {
            if (floor_wait.Count != 0)
            {
                state = 2;
                foreach (int i in floor_wait)
                {
                    AddTask(i);
                    floor_wait.Remove(i);
                }
            }
            if (outScript.floor_waitdown_out.Count != 0)
            {
                state = 2;
                foreach (int i in outScript.floor_waitdown_out)
                {
                    if (i < floor_current)
                    {
                        AddTask(i);
                        outScript.floor_waitdown_out.Remove(i);
                    }
                }
            }

            if (floor.Count == 0)
            {
                int min = 99;
                foreach (int i in outScript.floor_waitup_out)
                {
                    if (i < min)
                    {
                        min = i;
                    }
                }
                AddTask(min);
                outScript.floor_waitdown_out.Remove(min);
            }
        }
        else if (passtate == 2)
        {
            if (floor_wait.Count != 0)
            {
                state = 1;
                foreach (int i in floor_wait)
                {
                    AddTask(i);
                    floor_wait.Remove(i);
                }
            }
            if (outScript.floor_waitup_out.Count != 0)
            {
                state = 1;
                foreach (int i in outScript.floor_waitup_out)
                {
                    if (i > floor_current)
                    {
                        AddTask(i);
                        outScript.floor_waitup_out.Remove(i);
                    }
                }
            }
            if (floor.Count == 0)
            {
                int max = 99;
                foreach (int i in outScript.floor_waitup_out)
                {
                    if (i > max)
                    {
                        max = i;
                    }
                }
                AddTask(max);
                outScript.floor_waitdown_out.Remove(max);
            }
        }
        else
        {
            state = 0;
        }
        return;
    }

    public float animatedTime = 0;
    void run()
    {
        //电梯停在某楼层
        if (animated == true)
        {
            animatedTime += Time.deltaTime;
            if (animatedTime > 1f)
            {
                animator.SetBool("isOpen", false);
            }
            if (animatedTime > 1.5f)
            {
                if (floor.Count == 0)
                {
                    RefillTask(state);
                    print("RefillTask:");
                    debug();
                    state = 0;
                }
                animated = false;
                animatedTime = 0;
            }
            return;
        }
        //电梯运行
        else
        {
            //整楼层时检测是否要停下

            if (rect.anchoredPosition.y % 25 == 0 && floor.Count != 0 && floor_current == (int)floor[0])
            {
                run_elevatorArrived();
            }
            else if (state == 1)
            {
                run_elevatorChangePosition(1);
            }
            else if (state == 2)
            {
                run_elevatorChangePosition(-1);
            }
        }

    }
    void run_elevatorChangePosition(int y_dis)
    {
        Vector2 pos = rect.anchoredPosition;
        rect.anchoredPosition = new Vector2(pos.x, pos.y + y_dis);

        if((pos.y+y_dis)%25==0)run_setFloorCurrent(floor_current + y_dis);
    }
    void run_setFloorCurrent(int f)
    {
        floor_current = f;
        floorShower.GetComponent<Text>().text = f.ToString();
    }
    void run_elevatorArrived()
    {
        //make buttons interactable
        string outbuttonState = state == 1 ? "up" : "down";
        if (floor_current!=1&&floor_current!=20)
        {
            outsideButton.transform.Find(floor_current.ToString()).transform.Find(outbuttonState).GetComponent<Button>().interactable = true;
        }
        floorButton.transform.Find(floor_current.ToString()).GetComponent<Button>().interactable = true;
        //update floor
        floor.RemoveAt(0);
        if (floor.Count == 0)
        {
            if (floor_current != 20) outsideButton.transform.Find(floor_current.ToString()).transform.Find("up").GetComponent<Button>().interactable = true;
            if (floor_current != 1) outsideButton.transform.Find(floor_current.ToString()).transform.Find("down").GetComponent<Button>().interactable = true;
            //add newa task
            //run_addNewTaskFromWaiting();
        }
        //start animation
        animated = true;
        animator.SetBool("isOpen", true);
    }

    








    void debug()
    {
        string str = "";
        foreach (int i in floor)
        {
            str = str + i.ToString() + ' ';
        }
        print(str);
    }
}
