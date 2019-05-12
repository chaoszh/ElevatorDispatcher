using System;
using System.Collections;
using System.Collections.Generic;
//using System.Threading;
using UnityEngine;
using UnityEngine.UI;

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
        animator = GetComponent<Animator>();
        outScript = outsideButton.GetComponent<outsideButtonScript>();

        //线程
        //Thread thread = new Thread(new ThreadStart(Running));

        this.InvokeRepeating("Move", 0, 0.02f);
    }

    // Update is called once per frame
    void Update()
    {
        string str = "tasks: ";
        foreach (int i in tasks)
        {
            str = str + i.ToString() + ' ';
        }
        str += "\nup:";
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
    }
    #endregion

    #region [Elevator Message]
    public int current;
    public int state=1;
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
        if (state == 0)
        {
            if (tasks.Count != 0)
            {
                if (min < current) state = 2;
                else if (max > current) state = 1;
            }
        }
        else if (state == 1)
        {
            if (max > current) state = 1;
            else state = 0;
        }
        else if (state == 2)
        {
            if (min < current && min != -1) state = 2;
            else state = 0;
        }

        SetStateShower();
    }
    #endregion

    #region [Tasks]
    public ArrayList tasks = new ArrayList();
    public ArrayList tasksup = new ArrayList();
    public ArrayList tasksdown = new ArrayList();
    public int min;
    public int max;
    
    //Add task
    void AddTasks(int floor)
    {
        tasks.Add(floor);
        tasks.Sort();
        SetMinMax();
    }
    void AddTasksup(int floor)
    {
        tasksup.Add(floor);
        tasksup.Sort();
        SetMinMax();
    }
    void AddTaskdown(int floor)
    {
        tasksdown.Add(floor);
        tasksdown.Sort();
        SetMinMax();
    }

    //Remove task
    void RemoveTasks(int floor)
    {
        tasks.Remove(floor);
        SetButtonInteractable(current, "tasks");
        SetMinMax();
    }
    void RemoveTasksup(int floor)
    {
        tasksup.Remove(floor);
        SetButtonInteractable(current, "tasksup");
        SetMinMax();
    }
    void RemoveTasksdown(int floor)
    {
        tasks.Remove(floor);
        SetButtonInteractable(current, "tasksdown");
        SetMinMax();
    }

    //update min&max
    void SetMinMax()
    {
        int size1 = tasks.Count;
        int size2 = tasksup.Count;
        int size3 = tasksdown.Count;
        if (size1 == 0 && size2 == 0 && size3 == 0)
        {
            max = min = -1;
        }
        else
        {
            max = Max((int)tasks[size1 - 1], (int)tasksup[size2 - 1], (int)tasksdown[size3 - 1]);
            min = Min((int)tasks[0], (int)tasksup[0], (int)tasksdown[0]);
        }
    }

    private int Min(int a, int b, int c)
    {
        if (a < b)
        {
            if (a < c) return a;
            else return c;
        }
        else
        {
            if (b < c) return b;
            else return c;
        }
    }
    private int Max(int a, int b, int c)
    {
        if (a > b)
        {
            if (a > c) return a;
            else return c;
        }
        else
        {
            if (b > c) return b;
            else return c;
        }
    }
    #endregion

    #region [Control elevator to move]
    public bool isAbleToDoors = false;
    public bool isArrived = false;
    /// <summary>
    /// 电梯运行
    /// </summary>
    void OnArrived()
    {
        bool flag = false;

        if (tasksup.Contains(current) && state == 1)
        {
            flag = true;
            RemoveTasksup(current);
        }
        else if (tasksdown.Contains(current) && state == 2)
        {
            flag = true;
            RemoveTasksdown(current);
        }

        if (tasks.Contains(current))
        {
            flag = true;
            RemoveTasks(current);
        }

        if (flag)
        {
            isArrived = true;   //电梯不会移动
            isAbleToDoors = true;
            OpenDoor();
            this.DelayToDo(1.7f, () =>
            {
                CloseDoor();
                isAbleToDoors = false;
            });
            this.DelayToDo(2.5f, () =>
            {
                isArrived = false;
            });
        }

        SetState();
    }

    void Move()
    {
        if (isArrived) return;

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
        Vector2 pos = rect.anchoredPosition;
        if (pos.y % 25 == 0)
        {
            SetCurrent((int)(pos.y / 25 + 1));
            OnArrived();
        }
    }

    void MoveUp()
    {
        Vector2 pos = rect.anchoredPosition;
        rect.anchoredPosition = new Vector2(pos.x, pos.y + 1);
    }

    void MoveDown()
    {
        Vector2 pos = rect.anchoredPosition;
        rect.anchoredPosition = new Vector2(pos.x, pos.y - 1);
    }
    #endregion

    #region [UI updated]
    void SetFloorShower(int current)
    {
        floorShower.GetComponent<Text>().text = current.ToString();
    }
    
    void SetStateShower()
    {
        if (state == 0)
        {

        }
        else if (state == 1)
        {
            
        }
        else if (state == 2)
        {

        }
    }

    void SetButtonInteractable(int floor, string str)
    {
        if (str == "tasks")
        {
            floorButton.transform.Find(floor.ToString()).GetComponent<Button>().interactable = true;
        }
        else if(str == "tasksup")
        {
            outsideButton.transform.Find(floor.ToString()).transform.Find("up").GetComponent<Button>().interactable = true;
        }
        else if (str == "tasksdown")
        {
            outsideButton.transform.Find(floor.ToString()).transform.Find("down").GetComponent<Button>().interactable = true;
        }
    }

    public Animator animator;

    public void OpenDoor()
    {
        print("opendoor");
        if (isAbleToDoors == false) return;
        animator.SetBool("isOpen", true);
    }

    public void CloseDoor()
    {
        if (isAbleToDoors == false) return;
        animator.SetBool("isOpen", false);
    }
    #endregion

    #region [Delay Tool]
    /// <summary>
    /// 延时指定秒数，执行某些代码
    /// </summary>
    /// <param name="t">延时秒数</param>
    /// <returns></returns>
    IEnumerator Wait(float t)
    {
        yield return new WaitForSeconds(t);//运行到这，暂停t秒

        //t秒后，继续运行下面代码
        print("Time over.");
    }

    #endregion

}

#region [Delay Tool]
public static class StaticUtils
{
    public static Coroutine DelayToDo(this MonoBehaviour mono, float delayTime, Action action, bool ignoreTimeScale = false)
    {
        Coroutine coroutine = null;
        if (ignoreTimeScale)
        {
            coroutine = mono.StartCoroutine(DelayIgnoreTimeToDo(delayTime, action));
        }
        else
        {
            coroutine = mono.StartCoroutine(DelayToInvokeDo(delayTime, action));

        }
        return coroutine;
    }

    public static IEnumerator DelayToInvokeDo(float delaySeconds, Action action)
    {
        yield return new WaitForSeconds(delaySeconds);
        action();
    }

    public static IEnumerator DelayIgnoreTimeToDo(float delaySeconds, Action action)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + delaySeconds)
        {
            yield return null;
        }
        action();
    }

    public static bool IsNullOrEntry(this string str)
    {
        return String.IsNullOrEmpty(str);
    }
}
#endregion