using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleValidator : MonoBehaviour
{
    int numOtherCollisions;
    int numWallCollisions;
    
   void CheckCollisions()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale, Quaternion.identity);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if(hitColliders[i].gameObject.tag == "wall")
                numWallCollisions++;
            else if(hitColliders[i].gameObject.tag == "obstacle" || hitColliders[i].gameObject.tag == "scalableObstacle"){
                numOtherCollisions++;
            }
        }

    }
    
    public bool isWallValid(){
        CheckCollisions();
        return numWallCollisions < 2 && numOtherCollisions < 2;
    }

}
