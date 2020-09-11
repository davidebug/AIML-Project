using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;

public class GamePointsHandler : MonoBehaviour
{
    public int score1;
    public int score2;
    public int score3;
    public int score4;

    public Text scoreText1;
    public Text scoreText2;
    public Text scoreText3;
    public Text scoreText4;

    StatsRecorder m_Recorder;


    public void Awake(){
        m_Recorder = Academy.Instance.StatsRecorder;
        OnReset();
    }

    public void OnReset(){

        score1 = 0; score2 = 0; score3 = 0; score4 = 0;
    }



    public void Update(){
        scoreText1.text = $"Agent1 : {score1}";
        scoreText2.text = $"Agent2 : {score2}";
        scoreText3.text = $"Agent3 : {score3}";
        scoreText4.text = $"Agent4 : {score4}";

        if ((Time.frameCount % 100)== 0){
            m_Recorder.Add("Agent1 score", score1);
            m_Recorder.Add("Agent2 score", score2);
            m_Recorder.Add("Agent3 score", score3);
            m_Recorder.Add("Agent4 score", score4);
        }
    }


}
