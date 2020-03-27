using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : AbstractKinematic
{
    public Arrive arrive;
    public Door door;
    public bool forceUser;
   // float detectionThreshold = 5f;
    // Start is called before the first frame update
    void Start()
    {
        // Configure Movement
        arrive = new Arrive();
        arrive.ai = this;
        arrive.target = target;

        // Set Up Tasks
        ITask behavior = ConfigureBehavior();
        behavior.execute();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!forceUser)
        {
            mySteering = new SteeringOutput();
            mySteering.linear = arrive.GetSteering().linear;
            base.Update();
        }
        
    }

    public ITask ConfigureBehavior()
    {
;       List<ITask> TryToUseForce = new List<ITask>();
        ITask hasTheForce = new doesUseTheForce(this);
        ITask CheckDoor = new isDoorOpen(door);
        ITask ForceLift = new ForceMoveDoor(door);
        List<ITask> handleDoor = new List<ITask>();
        handleDoor.Add(CheckDoor);
        handleDoor.Add(ForceLift);
        ITask handleDoorSelect = new Selector(handleDoor);
        ITask forcePull = new ForcePullItem(target.GetComponent<Rigidbody>());
        TryToUseForce.Add(hasTheForce);
        TryToUseForce.Add(handleDoorSelect);
        TryToUseForce.Add(forcePull);
        ITask TryToUseForceSeq = new Sequence(TryToUseForce);


        List<ITask> moveToOpenRoom = new List<ITask>();
        ITask CheckOpenDoor = new isDoorOpen(door);
        ITask MoveIntoRoom = new MoveToTarget();
        moveToOpenRoom.Add(CheckOpenDoor);
        moveToOpenRoom.Add(MoveIntoRoom);
        Sequence moveToOpenRoomSeq = new Sequence(moveToOpenRoom);

        List<ITask> tryToOpenDoor = new List<ITask>();
        ITask isUnlocked = new isDoorUnlocked(door);
        ITask openDoor = new OpenDoor(door);
        tryToOpenDoor.Add(isUnlocked);
        tryToOpenDoor.Add(openDoor);
        ITask tryToOpenSeq = new Sequence(tryToOpenDoor);

        List<ITask> breakLockedDoor = new List<ITask>();
        ITask doorClosed = new isDoorClosed(door);
        ITask bargeDoor = new BargeDoor(door);
        breakLockedDoor.Add(doorClosed);
        breakLockedDoor.Add(bargeDoor);
        ITask breakDoor = new Sequence(breakLockedDoor);

        List<ITask> interactWithDoor = new List<ITask>();
        interactWithDoor.Add(tryToOpenSeq);
        interactWithDoor.Add(breakDoor);
        ITask interact = new Selector(interactWithDoor);

        List<ITask> moveToDoor = new List<ITask>();
        ITask goToDoor = new MoveToDoor();
        ITask goToRoom = new MoveToTarget();
        moveToDoor.Add(goToDoor);
        moveToDoor.Add(interact);
        moveToDoor.Add(goToRoom);
        ITask moveToClosedRoomSeq = new Sequence(moveToDoor);

        List<ITask> taskList = new List<ITask>();
        taskList.Add(TryToUseForceSeq);
        taskList.Add(moveToOpenRoomSeq);
        taskList.Add(moveToClosedRoomSeq);

        ITask root = new Selector(taskList);
        return root;
      
    }
}
