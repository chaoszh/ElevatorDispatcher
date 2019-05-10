using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.UI;
using UnityEngine;

public class elevatorScript: MonoBehaviour
{
    #region [Basic Settings]
    public GameObject outsideButton;
    public GameObject floorButton;
    public GameObject floorShower;
    public RectTransform rect;
    public outsideButtonScript outScript;
    public GameObject Text;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        animator.GetComponent<Animator>();
        outScript = outsideButton.GetComponent<outsideButtonScript>();
        tasksup = outScript.tasksup;
        tasksdown = outScript.tasksdown;
    }

    // Update is called once per frame
    void Update()
    {
        string str = "";
        foreach (int i in tasks)
        {
            str = str + i.ToString() + ' ';
        }
        Text.GetComponent<Text>().text = str;
    }
    #endregion

    #region [Elevator Message]
    public int current;
    public int state;
    public bool isArrived;

    /// <summary>
    /// 每次运行到达新楼层，自动更新当前楼层数current和UI界面的floorShower
    /// </summary>
    /// <param name="c"></param>
    void SetCurrent(int c)
    {
        current = c;
        SetFloorShower(current);
    }

    /// <summary>
    /// 设置好下一步的状态，并根据此电梯继续运行。
    /// </summary>
    void SetState()
    {
        if (state = 0)
        {
            if (tasks.Count != 0)
            {
                if (min < current) state = 2;
                else if (max > current) state = 1;
            }
            else
            {
                if (tasksup.Count != 0)
                {
                    foreach (int i in tasksup)
                    {
                        ///pass!!
                    }
                }
            }
        }
        else if (state = 1)
        {
            if (max > current) state = 1;
            else state = 0;
        }
        else if (state = 2)
        {
            if (min < current) state = 2;
            else state = 0;
        }

    }
    #endregion

    #region [Tasks]
    public ArrayList tasks = new ArrayList();
    public ArrayList tasksup;
    public ArrayList tasksdown;
    int min;
    int max;
    
    //Add task
    void AddTask(int floor)
    {
        tasks.Add(floor);
        tasks.Sort();
        SetMinMax();
    }
    //update min&max
    void SetMinMax()
    {
        int size = tasks.Count;
        if (size == 0)
        {
            min = max = -1;
        }
        else
        {
            min = tasks[0];
            max = tasks[size - 1];
        }
    }

    #endregion

    #region [Control elevator to move]
    /// <summary>
    /// 电梯运行
    /// </summary>
    void Running()
    {
        isArrived = Move();
        if (isArrived)
        {
            bool flag = false;
            if (tasks.Contains(current))
            {
                flag = true;
                tasks.Remove(current);
                SetMinMax();
            }
            if (tasksup.Contains(current) && state == 1)
            {
                flag = true;
                tasksup.Remove(current);
            }
            else if (tasksdown.Contains(current) && state == 2)
            {
                flag = true;
                tasksdown.Remove(current);
            }

            if (flag)
            {
                OpenDoor();
                CloseDoor();
            }
            SetState();
        }
    }

    bool Move()
    {
        //on move
        if (state == 1)
        {
            MoveUp();
        }
        else if (state == 2)
        {
            MoveDown();
        }

        //on arrived
        if ((pos.y + y_dis) % 25 == 0)
        {
            SetCurrent((pos.y + y_dis) / 25);
            return true;
        }
        else
        {
            return false;
        }
    }

    void MoveUp()
    {
        Vector2 pos = rect.anchoredPosition;
        rect.anchoredPosition = new Vector2(pos.x, pos.y + 1);
        Thread.Sleep(2);
    }

    void MoveDown()
    {
        Vector2 pos = rect.anchoredPosition;
        rect.anchoredPosition = new Vector2(pos.x, pos.y - 1);
        Thread.Sleep(2);
    }
    #endregion

    #region [UI updated]
    void SetFloorShower(int current)
    {
        floorShower.GetComponent<Text>().text = current.ToString();
    }
    
    void SetStateShower()
    {

    }

    public Animator animator;

    public void OpenDoor()
    {
        if (isArrived == false) return;
        animator.SetBool("isOpen", true);
        Thread.Sleep(500);
    }

    public void CloseDoor()
    {
        if (isArrived == false) return;
        Thread.Sleep(500);
        animator.SetBool("isOpen", false);
    }
    #endregion
}
