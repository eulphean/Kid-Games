
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Callback for agents to request new positions once they reach their target. 
public delegate Vector3 Callback(string name);

public class AgentManager : MonoBehaviour
{
    public List<GameObject> agentPrefabs;
    public List<GameObject> spawnPoints;
    public List<GameObject> targetPoints;
    public List<GameObject> targetCubes;
    private Dictionary<string, int> _agentTargetIdx; 

    public int maxAgents;
    private Callback m_calcPosition; 

    // Start is called before the first frame update
    void Start()
    {
        m_calcPosition = calcTargetPosition;
        _agentTargetIdx = new Dictionary<string, int>(); 

        for (int i = 0; i < maxAgents; i++)
        {
            // Get a random index for the agent.
            int idx = Random.Range(0, agentPrefabs.Count);

            // Instantiate a new agent on the NavMesh.
            string name = "Kid: " + i;
            Vector3 curPos = calcSpawnPosition(name);
            GameObject a = Instantiate(agentPrefabs[idx], transform);
            a.name = name;

            // Default target location for the agent. 
            _agentTargetIdx.Add(name, -1);

            // Calculate a target. 
            Vector3 targetPos = calcTargetPosition(name); 
            Kid k = a.GetComponent<Kid>();
            k.gameObject.AddComponent<NavMeshAgent>();

            var nm = k.GetComponent<NavMeshAgent>();
            // nm.transform.localPosition = curPos;
            nm.Warp(curPos);
            nm.radius = 0.03f;
            nm.height = 0.2f;
            nm.autoBraking = false;

            k.setCallback(m_calcPosition); 
            k.setTarget(targetPos);
            k.setTargetObject(targetCubes[i]); 

            print (a.name + " spawned."); 
        }
    }

    Vector3 calcTargetPosition(string agentName) {

        // Find a new target zone. Don't find a target in the same zone.
        // We need to have atleast 2 target zones moving forward.
        int idx = Random.Range(0, targetPoints.Count);

        // Check the dictionary for last target zone. 
        int lastTargetIdx = _agentTargetIdx[agentName]; 
        while (idx == lastTargetIdx)
        {
            idx = Random.Range(0, targetPoints.Count); 
        }

        // Store the last target index in the dictionary.
        _agentTargetIdx[agentName] = idx; 

        float targetRadius = targetPoints[idx].GetComponent<TargetPoint>().radius; 
        Vector3 curPosition = targetPoints[idx].transform.localPosition; 
        Vector2 targetPos = Random.insideUnitCircle * targetRadius;
        //Vector3 p = new Vector3(curPosition.x + targetPos.x, 0, curPosition.z + targetPos.y);
        //return p;

        // Query NavMesh for a corresponding target position. 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(curPosition, out hit, 3.0f, NavMesh.AllAreas))
        {
            print(agentName + " Found target position");
            Vector3 result = hit.position;
            return result;
        }
        else
        {
            print("Not found");
            return Vector3.zero;
        }
    }

    Vector3 calcSpawnPosition(string agentName) {
        int idx = Random.Range(0, spawnPoints.Count); 
        float spawnRadius = spawnPoints[idx].GetComponent<SpawnPoint>().radius; 
        Vector3 curPosition = spawnPoints[idx].transform.localPosition; 
        Vector2 spawnPos = Random.insideUnitCircle * spawnRadius;
        // Vector3 p = new Vector3(curPosition.x + spawnPos.x, 0, curPosition.z + spawnPos.y);
        return curPosition; 

        ////// Query NavMesh for a corresponding spawn position. 
        //NavMeshHit hit;
        //if (NavMesh.SamplePosition(p, out hit, 500.0f, NavMesh.AllAreas))
        //{
        //    print(agentName + " Found spawn position");
        //    Vector3 result = hit.position;
        //    return result;
        //}
        //else
        //{
        //    print("Not found");
        //    return Vector3.zero;
        //}
    }
}
