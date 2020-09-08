using UnityEngine;

public class CoinsHandler : MonoBehaviour
{
    [HideInInspector]
    public GameAreaHandler gameArea;

    public void Update(){
        transform.Rotate(new Vector3(1f,0,0),Space.Self); 
    }
    public void OnPick(){
            transform.position = new Vector3(Random.Range(-gameArea.range, gameArea.range),3f,
                Random.Range(-gameArea.range, gameArea.range)) + gameArea.transform.position;
    }


}
