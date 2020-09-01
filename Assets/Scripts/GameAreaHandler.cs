using UnityEngine;
    public class GameArea : MonoBehaviour
    {
        public virtual void ResetArea()
        {
        }
    }

public class GameAreaHandler : GameArea
{
    public GameObject food;
    public int numFood;
    public bool respawnFood;
    public float range;

    public int minObstacles;
    public int maxObstacles;
    public GameObject obstacle;

    void CreateFood(int num, GameObject type)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject f = Instantiate(type, new Vector3(Random.Range(-range, range), 1f,
                Random.Range(-range, range)) + transform.position,
                Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f))));
            f.GetComponent<CoinsHandler>().respawn = respawnFood;
            f.GetComponent<CoinsHandler>().myArea = this;
        }
    }

    public void ResetFoodArea(GameObject[] agents)
    {
        placeObstacles();
        foreach (GameObject agent in agents)
        {
            if (agent.transform.parent == gameObject.transform)
            {
                agent.transform.position = new Vector3(Random.Range(-range, range), 2f,
                    Random.Range(-range, range))
                    + transform.position;
                agent.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360f)));
            }
        }

        CreateFood(numFood, food);
    }

        
    public void placeObstacles(){
        int num = Random.Range(minObstacles,maxObstacles);
        for (int i = 0; i < num; i++)
        {
            getNewObstacle();
            
        }
    }

    GameObject getNewObstacle(){
        GameObject newObstacle = Instantiate(obstacle, new Vector3(Random.Range(-range, range), 1f,
                Random.Range(-range, range)) + transform.position,
                Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f))));
        bool isThin = 10 < Random.Range(2.0f,20.0f);
        bool isScalable = 10 > Random.Range(4.0f,20.0f);
        newObstacle.transform.localScale = new Vector3( isThin ? 1.0f : isScalable ? Random.Range(30f,50f) :  Random.Range(10.0f,40.0f),
            isScalable ? 2f : Random.Range(5.0f,50.0f) ,isScalable ? Random.Range(30.0f,50.0f) : Random.Range(10f,40f));
        if(!isObstacleValid(newObstacle)){
            Destroy(newObstacle);
            return getNewObstacle();
        }
        return newObstacle;
    }

    //TODO: obstacle validation
    bool isObstacleValid(GameObject newObstacle){
        GameObject wall = GameObject.FindGameObjectWithTag("wall");
        return true;
    }
    public override void ResetArea()
    {
    }
}
