using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField]
    private Vector2Int foodSpawnArea = new Vector2Int(46, 21);
    
    [SerializeField]
    private GameObject foodPrefab;
    
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnFood();
        
    }

    public void SpawnFood()
    {

        Vector3 randomPosition = new Vector3(
            Random.Range(-foodSpawnArea.x / 2, foodSpawnArea.x / 2),
            Random.Range(-foodSpawnArea.y / 2, foodSpawnArea.y / 2),
            0
        );
        Instantiate(foodPrefab, randomPosition, Quaternion.identity);

    }
}
