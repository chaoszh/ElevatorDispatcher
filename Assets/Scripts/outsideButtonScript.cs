using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outsideButtonScript : MonoBehaviour
{
    public GameObject[] e = new GameObject[5];
    public elevatorScript[] es = new elevatorScript[5];
    // Start is called before the first frame update
    void Start()
    {
        es[0] = e[0].GetComponent<elevatorScript>();
        es[1] = e[1].GetComponent<elevatorScript>();
        es[2] = e[2].GetComponent<elevatorScript>();
        es[3] = e[3].GetComponent<elevatorScript>();
        es[4] = e[4].GetComponent<elevatorScript>();

        //test:
        //InvokeRepeating("printTest", 1, 1f);
    }

    void printTest()
    {
        //test
        /*
        print("e[0]_floot_current=" + es[0].floor_current);
        print("e[1]_floot_current=" + es[1].floor_current);
        print("e[2]_floot_current=" + es[2].floor_current);
        print("e[3]_floot_current=" + es[3].floor_current);
        print("e[4]_floot_current=" + es[4].floor_current);
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UP(int i)
    {
        int tarEle = findProperElevator();
        e[tarEle].SendMessage("AddTask", i);
    }

    public void DOWN(int i)
    {
        int tarEle = findProperElevator();
        e[tarEle].SendMessage("AddTask", i);
    }

    int findProperElevator()
    {
        return 0;
    }
}
