
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Callback for agents to request new positions once they reach their target. 
public delegate Vector3 Callback(string name);

public class AgentManager : MonoBehaviour
{
    public List<GameObject> agentPrefabs;
    public List<GameObject> spawnPoints;
    public List<GameObject> targetCubes;
    private Dictionary<string, TargetPoint> _agentTargetIdx;

    public int maxAgents;
    private Callback m_calcPosition; 

    // Start is called before the first frame update
    void Start()
    {
        m_calcPosition = calcTargetPosition;
        _agentTargetIdx = new Dictionary<string, TargetPoint>(); 

        for (int i = 0; i < maxAgents; i++)
        {
            // Get a random index for the agent.
            int idx = Random.Range(0, agentPrefabs.Count);

            // Instantiate a new agent on the NavMesh.
            string name = "Kid: " + i;
            SpawnPoint spawnPoint; 
            Vector3 curPos = calcSpawnPosition(name, out spawnPoint);
            GameObject a = Instantiate(agentPrefabs[idx], curPos, Quaternion.identity, transform);
            a.name = name;

            // Calculate initial target. 
            List<TargetPoint> initTargets = spawnPoint.initialTargets;
            idx = Random.Range(0, initTargets.Count);
            TargetPoint curTarget = initTargets[idx];
            float targetRadius = curTarget.radius;
            Vector3 curPosition = curTarget.transform.localPosition;
            Vector2 targetPos = Random.insideUnitCircle * targetRadius;
            Vector3 t = new Vector3(curPosition.x + targetPos.x, 0, curPosition.z + targetPos.y);
            _agentTargetIdx.Add(name, curTarget); // Update dictionary. 

            Kid k = a.GetComponent<Kid>(); 
            k.setCallback(m_calcPosition);
            k.setTargetObject(targetCubes[i]);
            k.setTarget(t);

            print (a.name + " spawned."); 
        }
    }


    Vector3 calcSpawnPosition(string agentName, out SpawnPoint spawnPoint) {
        int idx = Random.Range(0, spawnPoints.Count);
        spawnPoint = spawnPoints[idx].GetComponent<SpawnPoint>(); 
        float spawnRadius = spawnPoint.radius; 
        Vector3 curPosition = spawnPoints[idx].transform.localPosition; 
        Vector2 spawnPos = Random.insideUnitCircle * spawnRadius;
        Vector3 p = new Vector3(curPosition.x + spawnPos.x, 0, curPosition.z + spawnPos.y);
        return p; 
    }

    Vector3 calcTargetPosition(string agentName)
    {
        // Get the current target point from dictionary.
        // Get a random target point from its neighbors. 
        TargetPoint targetPoint = _agentTargetIdx[agentName];
        List<TargetPoint> neighbors = targetPoint.neighbors;
        int idx = Random.Range(0, neighbors.Count);
        TargetPoint newTargetPoint = neighbors[idx];
        float targetRadius = newTargetPoint.radius;
        Vector3 curPosition = newTargetPoint.transform.localPosition;
        Vector2 targetPos = Random.insideUnitCircle * targetRadius;
        Vector3 t = new Vector3(curPosition.x + targetPos.x, 0, curPosition.z + targetPos.y);
        _agentTargetIdx[agentName] = newTargetPoint;

        return t; 
    }
}



