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

    public void FLOOR(int floor)
    {
        elevator.SendMessage("AddTasks", i);
    }
}
