using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointTest : MonoBehaviour {

    float actualSpeed = 2.0f;
    public GameObject[] checkpoints;
    int counter = 0;

    

    private void Start()
    {
       
    }

    void Update()
    {
        
        //check our distance to the current waypoint, Are we near enough?
        if (gameObject.transform.position == checkpoints[counter].transform.position)
        {
            if (counter < checkpoints.Length - 1) //switch to the nex waypoint if exists
            {
                counter++;
            }
            else //ship is off screen, destroy it
            {
                counter = 0;
                //Destroy(gameObject);
                //// destroy waypoints
                //for (int i = 0; i < checkpoints.Length; i++)
                //{
                //    Destroy(checkpoints[i]);
                //}
                // need to destroy parent also
            }
        }
        // move towards next waypoint
        float step = actualSpeed * Time.deltaTime;
        gameObject.transform.position = Vector3.MoveTowards(transform.position, checkpoints[counter].transform.position, step);

        // rotate facing to direction of next way point
        Vector3 vectorToTarget = checkpoints[counter].transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        angle += 90f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.rotation = q;
        transform.rotation = Quaternion.Lerp(transform.rotation, q, 5f * Time.deltaTime);


    }

    
}
