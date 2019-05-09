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
        InvokeRepeating("run", 0, 0.03f);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void AddTask(int f)
    {
        if (floor.Contains(f)) return;

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
                    return;
                }
                if ((int)floor[i] >= f)
                {
                    floor.Insert(i, f);
                    return;
                }
            }
            else if (state == 2)
            {
                if (floor_current < f)
                {
                    floor_wait.Add(f);
                    return;
                }
                if ((int)floor[i] <= f)
                {
                    floor.Insert(i, f);
                    return;
                }
            }
        }
        floor.Add(f);
        return;
    }

    float animatedTime = 0;
    void run()
    {
        //电梯停在某楼层
        if (animated == true)
        {
            animatedTime += Time.deltaTime;
            if (animatedTime > 1.0f)
            {
                animated = false;
                changeAnimation();
                animatedTime = 0;
            }
            return;
        }
        //电梯运行
        else
        {
            //整楼层时检测是否要停下
            if (rect.anchoredPosition.y % 25 == 0)
            {
                if (floor.Count != 0 && floor_current == (int)floor[0])
                {
                    run_elevatorArrived();
                }
            }
            if (state == 1)
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
        outsideButton.transform.Find(floor_current.ToString()).transform.Find(outbuttonState).GetComponent<Button>().interactable = true;
        floorButton.transform.Find(floor_current.ToString()).GetComponent<Button>().interactable = true;
        //update floor
        floor.RemoveAt(0);
        if (floor.Count == 0)
        {
            state = 0;
        }
        //start animation
        animated = true;
        changeAnimation();
    }
    void changeAnimation()
    {
        print("I’m opened at FLOOR:"+floor_current);
    }
}
