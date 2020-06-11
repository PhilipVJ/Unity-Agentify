using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject foodPrefab;
    [SerializeField]
    private GameObject waterPrefab;

    // Update is called once per frame
    void Update()
    {       
        GameObject[] water = GameObject.FindGameObjectsWithTag("Water");
        GameObject[] food = GameObject.FindGameObjectsWithTag("Food");
        if (food.Length < 20)
        {
            GameObject.Instantiate(foodPrefab, GetRandomPoint(transform.position, 25), Quaternion.identity);
        }
        if (water.Length < 10)
        {
            GameObject.Instantiate(waterPrefab, GetRandomPoint(transform.position, 25), Quaternion.identity);
        }
    }

    private Vector3 GetRandomPoint(Vector3 center, float maxDistance)
    {
        Vector3 randomPos = Random.insideUnitSphere * maxDistance + center;
        randomPos.y = 0;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, 2, NavMesh.AllAreas);    
        return hit.position;
    }
}
