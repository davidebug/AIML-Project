using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class CollectorAgent : Agent
{
    GameSettingsHandler gameSettingsHandler;
    public GameObject area;
    GameAreaHandler gameAreaHandler;
    bool gotReward;
    bool m_Shoot;
    float chengeStateTime;
    Rigidbody m_AgentRb;
    float turnSpeed = 300;
    float speed = 2;
    public Material agentMaterial;
    public Material rewardMaterial;
    public GameObject myLaser;
    public bool contribute;
    public bool useVectorObs;

    EnvironmentParameters m_ResetParams;

    public override void Initialize()
    {
        m_AgentRb = GetComponent<Rigidbody>();
        gameAreaHandler = area.GetComponent<GameAreaHandler>();
        gameSettingsHandler = FindObjectOfType<GameSettingsHandler>();
        m_ResetParams = Academy.Instance.EnvironmentParameters;

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (useVectorObs)
        {
            var localVelocity = transform.InverseTransformDirection(m_AgentRb.velocity);
            sensor.AddObservation(localVelocity.x);
            sensor.AddObservation(localVelocity.z);
            sensor.AddObservation(false);
            sensor.AddObservation(m_Shoot);
        }
    }


    void setRewardState()
    {
        gotReward = true;
        chengeStateTime = Time.time;
        gameObject.GetComponentInChildren<Renderer>().material = rewardMaterial;
    }

    void setNormalState()
    {
        gotReward = false;
        gameObject.GetComponentInChildren<Renderer>().material = agentMaterial;
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        m_Shoot = false;

        if (Time.time > chengeStateTime + 0.5f)
        {
            if (gotReward)
            {
                setNormalState();
            }
        }

        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;


        var shootCommand = false;
        var forwardAxis = (int)vectorAction[0];
        var rightAxis = (int)vectorAction[1];
        var rotateAxis = (int)vectorAction[2];
        var shootAxis = (int)vectorAction[3];

        switch (forwardAxis)
        {
            case 1:
                dirToGo = transform.forward;
                break;
            case 2:
                dirToGo = -transform.forward;
                break;
        }

        switch (rightAxis)
        {
            case 1:
                dirToGo = transform.right;
                break;
            case 2:
                dirToGo = -transform.right;
                break;
        }

        switch (rotateAxis)
        {
            case 1:
                rotateDir = -transform.up;
                break;
            case 2:
                rotateDir = transform.up;
                break;
        }
        switch (shootAxis)
        {
            case 1:
                shootCommand = true;
                break;
        }
        if (shootCommand)
        {
            m_Shoot = true;
            dirToGo *= 0.5f;
            m_AgentRb.velocity *= 0.75f;
        }
        m_AgentRb.AddForce(dirToGo * speed, ForceMode.VelocityChange);
        transform.Rotate(rotateDir, Time.fixedDeltaTime * turnSpeed);
    

        if (m_AgentRb.velocity.sqrMagnitude > 25f) // slow it down
        {
            m_AgentRb.velocity *= 0.95f;
        }

        if (m_Shoot)
        {
            var myTransform = transform;
            myLaser.transform.localScale = new Vector3(1f, 1f, 1f);
            var rayDir = 25.0f * myTransform.forward;
            Debug.DrawRay(myTransform.position, rayDir, Color.red, 0f, true);
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, 2f, rayDir, out hit, 25f))
            {
                if (hit.collider.gameObject.CompareTag("agent"))
                {
                    
                }
            }
        }
        else
        {
            myLaser.transform.localScale = new Vector3(0f, 0f, 0f);
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0f;
        actionsOut[1] = 0f;
        actionsOut[2] = 0f;
        if (Input.GetKey(KeyCode.D))
        {
            actionsOut[2] = 2f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            actionsOut[0] = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            actionsOut[2] = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            actionsOut[0] = 2f;
        }
        actionsOut[3] = Input.GetKey(KeyCode.Space) ? 1.0f : 0.0f;
    }

    public override void OnEpisodeBegin()
    {
        setNormalState();
        m_Shoot = false;
        m_AgentRb.velocity = Vector3.zero;
        myLaser.transform.localScale = new Vector3(0f, 0f, 0f);
        transform.position = new Vector3(Random.Range(-gameAreaHandler.range, gameAreaHandler.range),
            2f, Random.Range(-gameAreaHandler.range, gameAreaHandler.range))
            + area.transform.position;
        transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("food"))
        {
            setRewardState();
            collision.gameObject.GetComponent<CoinsHandler>().OnPick();
            AddReward(1f);
            if (contribute)
            {
                gameSettingsHandler.totalScore += 1;
            }
        }
        if (collision.gameObject.CompareTag("obstacle"))
        {
            AddReward(-0.5f);
            
        }
    }
}
