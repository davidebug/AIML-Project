using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;

public class GameSettingsHandler : MonoBehaviour
{
    [HideInInspector]
    public GameObject[] agents;
    [HideInInspector]
    public GameAreaHandler[] areaList;

    public void Awake(){
        Academy.Instance.OnEnvironmentReset += EnvironmentReset;
        Physics.gravity = Physics.gravity *= 8;
    }

    public void EnvironmentReset(){
        ClearObjects(GameObject.FindGameObjectsWithTag("obstacle"));
        ClearObjects(GameObject.FindGameObjectsWithTag("scalableObstacle"));
        ClearObjects(GameObject.FindGameObjectsWithTag("coin"));    
        agents = GameObject.FindGameObjectsWithTag("agent");
        areaList = FindObjectsOfType<GameAreaHandler>();
        foreach (var area in areaList)
            area.ResetArea(agents);
    }



    void ClearObjects(GameObject[] objects){
        foreach (var obj in objects){
            obj.SetActive(false);
            Destroy(obj);
        }    
    }


}
