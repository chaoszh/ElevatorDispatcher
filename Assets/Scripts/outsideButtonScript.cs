using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outsideButtonScript : MonoBehaviour
{
    public GameObject elevator1;
    public GameObject elevator2;
    public GameObject elevator3;
    public GameObject elevator4;
    public GameObject elevator5;
    public GameObject[] elevator = new GameObject[5];
    // Start is called before the first frame update
    void Start()
    {
        elevator[0] = elevator1;
        elevator[1] = elevator2;
        elevator[2] = elevator3;
        elevator[3] = elevator4;
        elevator[4] = elevator5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UP(int i)
    {
        int tarEle = findProperElevator();
        elevator[tarEle].SendMessage("AddTask", i);
    }

    public void DOWN(int i)
    {
        int tarEle = findProperElevator();
        elevator[tarEle].SendMessage("AddTask", i);
    }

    int findProperElevator()
    {
        return 0;
    }
}
