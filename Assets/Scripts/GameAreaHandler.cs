using UnityEngine;
public class GameAreaHandler : MonoBehaviour
{
    public GameObject coin;
    public int numCoins;
    public float range;
    public Material scalableMaterial;
    public int maxObstacles;
    public GameObject obstacle;

    void GenerateCoins(int num, GameObject type)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject f = Instantiate(type, new Vector3(Random.Range(-range, range), 1f,
                Random.Range(-range, range)) + transform.position,
                Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f))));
            f.GetComponent<CoinsHandler>().gameArea = this;
        }
    }

    public void ResetArea(GameObject[] agents)
    {
        placeObstacles();    
        GenerateCoins(numCoins, coin);
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
    }

        
    public void placeObstacles(){
        for (int i = 0; i < maxObstacles; i++)
        {
            generateNewObstacle();          
        }
    }

    void generateNewObstacle(){
        GameObject newObstacle = Instantiate(obstacle, new Vector3(Random.Range(-range, range), 1f,
                Random.Range(-range, range)) + transform.position,
                Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f))));
        bool isScalable = 10 > Random.Range(0.0f,14.0f);        
        bool isThin = 10 > Random.Range(0.0f,20.0f);       
        if(isScalable && !isThin){
            newObstacle.GetComponentInChildren<Renderer>().material = scalableMaterial;
        }
        newObstacle.transform.localScale = new Vector3( isThin && !isScalable ? 1.0f : isScalable ? Random.Range(20f,50f) :  Random.Range(10.0f,40.0f),
            isScalable && !isThin ? 2f : Random.Range(10.0f,30.0f) ,isScalable ? Random.Range(20.0f,50.0f) : Random.Range(10f,40f));
        if(!isObstacleValid(newObstacle)){
            newObstacle.SetActive(false);
            Destroy(newObstacle);
        }
    }

    bool isObstacleValid(GameObject newObstacle){
        return newObstacle.GetComponent<ObstacleValidator>().isWallValid();
    }
}
