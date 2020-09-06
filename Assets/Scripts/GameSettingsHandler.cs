using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;

public class GameSettingsHandler : MonoBehaviour
{
    [HideInInspector]
    public GameObject[] agents;
    [HideInInspector]
    public GameAreaHandler[] listArea;


    public int totalScore;
    public Text scoreText;

    StatsRecorder m_Recorder;

    public void Awake()
    {
        Academy.Instance.OnEnvironmentReset += EnvironmentReset;
        m_Recorder = Academy.Instance.StatsRecorder;
    }

    void EnvironmentReset()
    {
        ClearObjects(GameObject.FindGameObjectsWithTag("food"));

        agents = GameObject.FindGameObjectsWithTag("agent");
        listArea = FindObjectsOfType<GameAreaHandler>();
        foreach (var area in listArea)
        {
            area.ResetArea(agents);
        }

        totalScore = 0;
    }



    void ClearObjects(GameObject[] objects)
    {
        foreach (var food in objects)
        {
            Destroy(food);
        }
    }


    public void Update()
    {
        scoreText.text = $"Score: {totalScore}";

        // Send stats via SideChannel so that they'll appear in TensorBoard.
        // These values get averaged every summary_frequency steps, so we don't
        // need to send every Update() call.
        if ((Time.frameCount % 100)== 0)
        {
            m_Recorder.Add("TotalScore", totalScore);
        }
    }


}