using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Callback for agents to request new positions once they reach their target. 
public delegate Vector3 Callback();

public class AgentManager : MonoBehaviour
{
    public List<GameObject> agentPrefabs;
    public List<GameObject> spawnPoints;
    public List<GameObject> targetPoints; 

    public int maxAgents;
    private Callback m_calcPosition; 

    // Start is called before the first frame update
    void Start()
    {
        m_calcPosition = calcTargetPosition; 

        for (int i = 0; i < maxAgents; i++)
        {
            // Get a random index for the agent.
            int idx = Random.Range(0, agentPrefabs.Count);

            // Instantiate a new agent.
            Vector3 curPos = calcSpawnPosition();
            GameObject a = Instantiate(agentPrefabs[idx], transform);
            a.name = "Kid: " + i; 
            a.transform.localPosition = curPos; 

            // Calculate a target. 
            Vector3 targetPos = calcTargetPosition(); 
            Kid k = a.GetComponent<Kid>();
            k.setCallback(m_calcPosition); 
            k.setTarget(targetPos);
        }
    }

    Vector3 calcTargetPosition() {
        int idx = Random.Range(0, targetPoints.Count); 
        float targetRadius = targetPoints[idx].GetComponent<TargetPoint>().radius; 
        Vector3 curPosition = targetPoints[idx].transform.localPosition; 
        Vector2 targetPos = Random.insideUnitCircle * targetRadius; 
        return new Vector3(curPosition.x + targetPos.x, 0, curPosition.z + targetPos.y); 
    }

    Vector3 calcSpawnPosition() {
        int idx = Random.Range(0, spawnPoints.Count); 
        float spawnRadius = spawnPoints[idx].GetComponent<SpawnPoint>().radius; 
        Vector3 curPosition = spawnPoints[idx].transform.localPosition; 
        Vector2 spawnPos = Random.insideUnitCircle * spawnRadius; 
        return new Vector3(curPosition.x + spawnPos.x, 0, curPosition.z + spawnPos.y); 
    }
}
