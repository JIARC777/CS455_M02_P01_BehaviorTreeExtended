using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen;
    public bool isLocked;

    Vector3 openDoorPos = new Vector3(0, 130, 0);
    Vector3 closedDoorPos = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Awake()
    {
        if (isOpen)
            transform.eulerAngles = openDoorPos;
        else
            transform.eulerAngles = closedDoorPos;
         }
    public bool Open()
    {
        if (!isOpen && !isLocked)
        {
            transform.eulerAngles = openDoorPos;
            Debug.Log("Door is Opening");
            return true;
        }
        return false;
    }
}
