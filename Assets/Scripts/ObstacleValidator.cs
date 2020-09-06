using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleValidator : MonoBehaviour
{
    int numOtherCollisions;
    int numWallCollisions;
    
   void CheckCollisions()
    {
        //Use the OverlapBox to detect if there are any other colliders within this box area.
        //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale, Quaternion.identity);
        //Check when there is a new collider coming into contact with the box

        for (int i = 0; i < hitColliders.Length; i++)
        {
            //Output all of the collider names
            if(hitColliders[i].gameObject.tag == "wall")
                numWallCollisions++;
            else if(hitColliders[i].gameObject.tag == "obstacle"){
                numOtherCollisions++;
            }
            //Increase the number of Colliders in the array
        }

    }
    
    public bool isWallValid(){
        CheckCollisions();
        Debug.Log("NumWallCollisions -->> "+ numWallCollisions);
        Debug.Log("NumOtherCollisions -->> "+ numOtherCollisions);
        return numWallCollisions < 2 && numOtherCollisions < 2;
    }

}
