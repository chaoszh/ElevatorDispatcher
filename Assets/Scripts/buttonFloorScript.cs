using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonFloorScript : MonoBehaviour
{
    public GameObject elevator;
    public elevatorScript elevatorS;
    public ArrayList floor_wait = new ArrayList();
    // Start is called before the first frame update
    void Start()
    {
        elevatorS = elevator.GetComponent<elevatorScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addTask(int i)
    {
        elevator.SendMessage("AddTask", i);
    }

    public void button_open()
    {
        if (elevatorS.animated == true)
        {
            elevatorS.animatedTime = 0f;
        }
    }

    public void button_close()
    {
        if (elevatorS.animated == true)
        {
            elevatorS.animatedTime = 1f;
        }
    }

    public void button_alarm()
    {
    }
}
