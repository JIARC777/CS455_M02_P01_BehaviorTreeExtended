using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringOutput
{
    public Vector3 linear;
    public float angular;
}

public class AbstractKinematic : MonoBehaviour
{
    public Vector3 linearVelocity;
    public float angularVelocity; // in degrees
    public GameObject target;
    public float maxSpeed = 40f;
    public float maxAcceleration = 100f;
    protected SteeringOutput mySteering;

    public virtual void Update()
    {
        transform.position += linearVelocity * Time.deltaTime;
        // adding angular velocity to current transform rotation y component
        if (float.IsNaN(angularVelocity))
            angularVelocity = 0;
        transform.eulerAngles += new Vector3(0, angularVelocity * Time.deltaTime, 0);
        if (mySteering != null)
        {
            linearVelocity += mySteering.linear * Time.deltaTime;
            if (linearVelocity.magnitude > maxSpeed)
            {
                linearVelocity.Normalize();
                linearVelocity *= maxSpeed;
            }
            angularVelocity += mySteering.angular * Time.deltaTime;

        }

    }
}
public class Arrive
{
    public AbstractKinematic ai;
    public GameObject target;
    public float maxAcceleration = 25f;
    float targetRadius = 3f;
    float slowRadius = 10f;
    float timeToTarget = 0.1f;
    public float maxSpeed = 50f;

    public virtual SteeringOutput GetSteering()
    {
        SteeringOutput result = new SteeringOutput();
        result.linear = target.transform.position - ai.transform.position;
        float distance = result.linear.magnitude;
        float targetSpeed;
        Vector3 targetVelocity;
        // if (distance < targetRadius)
        //     return null;
        if (distance > slowRadius)
        {
            targetSpeed = maxSpeed;
        }
        else
        {
            targetSpeed = maxSpeed * (distance - targetRadius) / targetRadius;
        }
        targetVelocity = result.linear;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;


        result.linear = targetVelocity - ai.linearVelocity;
        result.linear /= timeToTarget;
        result.linear.y = 0;
        if (result.linear.magnitude > maxAcceleration)
        {
            result.linear.Normalize();
            result.linear *= maxAcceleration;
        }
        //Debug.Log(target);
        result.angular = 0;
        return result;

    }
    public float getTargetDist()
    {
        return (target.transform.position - ai.transform.position).magnitude;
    }
}
