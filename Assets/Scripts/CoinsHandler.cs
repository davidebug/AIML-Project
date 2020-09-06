using UnityEngine;

public class CoinsHandler : MonoBehaviour
{
    public bool respawn;
    public GameAreaHandler myArea;

    public void OnPick()
    {
        if (respawn)
        {
            transform.position = new Vector3(Random.Range(-myArea.range, myArea.range),
                3f,
                Random.Range(-myArea.range, myArea.range)) + myArea.transform.position;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Update(){
        transform.Rotate(new Vector3(1f,0,0),Space.Self); 
    }
}
