using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonFloorScript : MonoBehaviour
{
    public GameObject elevator;
    public ArrayList floor_wait = new ArrayList();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addTask(int i)
    {
        elevator.SendMessage("AddTask", i);
    }
}
