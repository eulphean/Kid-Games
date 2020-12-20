using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Callback to let agents ask the AgentManager to request for a new target position. 
public delegate Vector2 Callback();

public class AgentManager : MonoBehaviour
{
    public List<GameObject> agentPrefabs;
    public int maxAgents;
    public float spawnRadius;
    private Callback m_calcPosition; 


    // Start is called before the first frame update
    void Start()
    {
        m_calcPosition = calcRandomPositionInCircle;

        for (int i = 0; i < maxAgents; i++)
        {
            // Get a random index for the agent.
            int idx = Random.Range(0, agentPrefabs.Count);

            // Instantiate a new agent.
            Vector2 curPos = m_calcPosition();
            GameObject a = Instantiate(agentPrefabs[idx], transform);
            a.transform.localPosition = new Vector3(curPos.x, 0, curPos.y);

            // Set callback. 
            var c = a.GetComponent<Agent>();
            c.setCallback(m_calcPosition); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector2 calcRandomPositionInCircle()
    {
        return Random.insideUnitCircle * spawnRadius; 
    }
}
