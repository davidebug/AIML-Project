using UnityEngine;
    public class GameArea : MonoBehaviour
    {

    }

public class GameAreaHandler : GameArea
{
    public GameObject food;
    public int numFood;
    public bool respawnFood;
    public float range;

    public Material scalableMaterial;
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

    public void ResetArea(GameObject[] agents)
    {
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

    public void Start(){
        placeObstacles();
    }

    GameObject getNewObstacle(){
        GameObject newObstacle = Instantiate(obstacle, new Vector3(Random.Range(-range, range), 1f,
                Random.Range(-range, range)) + transform.position,
                Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f))));
        bool isScalable = 10 > Random.Range(4.0f,20.0f);        
        bool isThin = 10 < Random.Range(2.0f,20.0f);       
        if(isScalable && !isThin){
            newObstacle.GetComponentInChildren<Renderer>().material = scalableMaterial;
        }
        newObstacle.transform.localScale = new Vector3( isThin && !isScalable ? 1.0f : isScalable ? Random.Range(30f,50f) :  Random.Range(10.0f,40.0f),
            isScalable && !isThin ? 2f : Random.Range(10.0f,30.0f) ,isScalable ? Random.Range(30.0f,50.0f) : Random.Range(10f,40f));
        if(!isObstacleValid(newObstacle)){
            Destroy(newObstacle);
            return getNewObstacle();
        }
        return newObstacle;
    }

    //TODO: obstacle validation
    bool isObstacleValid(GameObject newObstacle){
        return newObstacle.GetComponent<ObstacleValidator>().isWallValid();
    }
}
