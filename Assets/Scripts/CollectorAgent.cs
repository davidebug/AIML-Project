using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class CollectorAgent : Agent
{
    public int agentTag;
    GameSettingsHandler gameSettingsHandler;
    public GameObject area;
    GameAreaHandler gameAreaHandler;
    bool environmentStarted;
    bool jump;
    bool isOnScalable;
    bool isOnGround;
    float changeStateTime;
    Rigidbody agentRigidBody;
    float turnSpeed = 300;
    float speed = 2.5f;
    bool gotReward;
    bool isFloating;
    public Material agentMaterial;
    public Material rewardMaterial;
    public bool useVectorObs;

    EnvironmentParameters ResetParams;

    public override void Initialize(){
        agentRigidBody = GetComponent<Rigidbody>();
        gameAreaHandler = area.GetComponent<GameAreaHandler>();
        gameSettingsHandler = FindObjectOfType<GameSettingsHandler>();
        ResetParams = Academy.Instance.EnvironmentParameters;

    }

    public override void CollectObservations(VectorSensor sensor){
        if (useVectorObs){
            var localVelocity = transform.InverseTransformDirection(agentRigidBody.velocity);
            sensor.AddObservation(localVelocity.x);
            sensor.AddObservation(localVelocity.z);
            sensor.AddObservation(isOnScalable);
            sensor.AddObservation(jump);          
        }
    }


    void setRewardState(){
        gotReward = true;
        changeStateTime = Time.time;
        gameObject.GetComponentInChildren<Renderer>().material = rewardMaterial;
    }

    void setNormalState(){
        gotReward = false;
        gameObject.GetComponentInChildren<Renderer>().material = agentMaterial;
    }

    void checkJumpReward(){
        isFloating = !isOnGround && !isOnScalable;
        if(!isFloating){
            if(isOnScalable){
                setRewardState();
                AddReward(0.30f);
            }
            else if(isOnGround){
                AddReward(-0.005f);
            }
        }
    }
    public override void OnActionReceived(float[] vectorAction){

         if (Time.time > changeStateTime + 0.5f){
            if (gotReward){
                setNormalState();
            }
        }
        isOnScalable =  CheckGround("scalableObstacle");
        isOnGround = CheckGround("ground");
        if(isFloating){
           checkJumpReward();
        }
        var direction = Vector3.zero;
        var rotation = Vector3.zero;
        var forwardInput = (int)vectorAction[0];
        var rightInput = (int)vectorAction[1];
        var rotateInput = (int)vectorAction[2];
        var jumpInput = (int)vectorAction[3];

        switch (forwardInput){
            case 1:
                direction = transform.forward;
                break;
            case 2:
                direction = -transform.forward;
                break;
        }
        switch (rightInput){
            case 1:
                direction = transform.right;
                break;
            case 2:
                direction = -transform.right;
                break;
        }
        switch (rotateInput){
            case 1:
                rotation = -transform.up;
                break;
            case 2:
                rotation = transform.up;
                break;
        }
        switch (jumpInput){
            case 1:
                direction *= 0.2f;
                agentRigidBody.velocity *= 0.75f;
                jump = true;
                break;
        }

        agentRigidBody.AddForce(direction * speed, ForceMode.VelocityChange);
        transform.Rotate(rotation, Time.fixedDeltaTime * turnSpeed);
        
        if (agentRigidBody.velocity.sqrMagnitude > 25f){
            agentRigidBody.velocity *= 0.95f;
        }
        if(jump && isOnGround && !isFloating && !isOnScalable){                 
                    agentRigidBody.AddForce(
                    Vector3.up * 700, ForceMode.Impulse);
                isFloating = true;                              
        }
    }

    public override void Heuristic(float[] actionsOut){
        actionsOut[0] = 0f;
        actionsOut[1] = 0f;
        actionsOut[2] = 0f;
        if (Input.GetKey(KeyCode.D))
            actionsOut[2] = 2f;
        if (Input.GetKey(KeyCode.W))
            actionsOut[0] = 1f;
        if (Input.GetKey(KeyCode.A))
            actionsOut[2] = 1f;
        if (Input.GetKey(KeyCode.S))
            actionsOut[0] = 2f;
        actionsOut[3] = Input.GetKey(KeyCode.Space) ? 1.0f : 0.0f;
    }

        bool CheckGround(string objTag){
        RaycastHit hit;
            Physics.Raycast(transform.position, Vector3.down, out hit,
                1f);

            if (hit.collider != null &&
                (hit.collider.CompareTag(objTag)
                && hit.normal.y > 0.95f)){
                // Debug.Log("Agent"+agentTag.ToString()+" -->"+objTag);
                return true;
            }

            return false;
    }
    public override void OnEpisodeBegin(){
        setNormalState();
        var rayDir = 0.5f * transform.forward;
        isOnScalable = CheckGround("scalableObstacle");
        jump = false;
        agentRigidBody.velocity = Vector3.zero;
        gameSettingsHandler.EnvironmentReset();
        transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
    }

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("coin")){
            setRewardState();
            collision.gameObject.GetComponent<CoinsHandler>().OnPick();
            AddReward(1.5f);
            switch(agentTag){
                case 2:
                    gameSettingsHandler.score2 += 1;
                    break;
                case 3:
                    gameSettingsHandler.score3 += 1;
                    break;
                case 4:
                    gameSettingsHandler.score4 += 1;
                    break;
                default:
                    gameSettingsHandler.score1 += 1;
                    break;
            }
        }
        if (collision.gameObject.CompareTag("obstacle")){
            AddReward(-0.40f);           
        }
        if (collision.gameObject.CompareTag("scalableObstacle") && !isOnScalable){
            AddReward(-0.005f);           
        }
        if (collision.gameObject.CompareTag("wall")){
            AddReward(-0.20f);           
        }
    }
}
