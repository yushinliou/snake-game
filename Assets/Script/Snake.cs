using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class Snake : MonoBehaviour
{
    private Vector2 direction = Vector2.up;
    
    [SerializeField]
    private float difficultyIncreaseTimeInterval = 5.0f;
    [SerializeField]
    private float minMoveTimeInterval = 0.01f;
    [SerializeField]
    private float difficultyIncreaseStep = 0.005f;

    [SerializeField]
    private float moveTimeInterval = 0.2f;
    private float moveTimer;

    [SerializeField]
    private GameObject tailSegmentPrefab;

    [SerializeField]
    private int score = 0;
    [SerializeField]
    private float elapsedTime = 0.0f;


    private List<Transform> tailSegmentTransforms = new List<Transform>();
    

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(IncreaseDifficulty());
    }

    private void Update()
    {
        bool hasTail = tailSegmentTransforms.Count > 0;
        if (Input.GetKeyDown(KeyCode.W) && (direction != Vector2.down || !hasTail))
        {
            direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) && (direction != Vector2.up || !hasTail))
        {
            direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.D) && (direction != Vector2.left || !hasTail))
        {
            direction = Vector2.right;
        }
        else if (Input.GetKeyDown(KeyCode.A) && (direction != Vector2.right || !hasTail))
        {
            direction = Vector2.left;
        }
    }

    private void Move()
    {

    Vector3 previousPosition = this.transform.position;
    this.transform.position += (Vector3)direction;
    for (int i = 0; i < tailSegmentTransforms.Count; i++)
    {
        Vector3 tempPosition = tailSegmentTransforms[i].position;
        tailSegmentTransforms[i].position = previousPosition;
        previousPosition = tempPosition;
    }

    }

    private void FixedUpdate()
    {
        moveTimer += Time.fixedDeltaTime;
        elapsedTime += Time.deltaTime;
        if (moveTimer >= moveTimeInterval)
        {
            Move();
            moveTimer = 0;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Collided with {other.gameObject.name}");
        if (other.CompareTag("Food"))
        {
            Extend();
            //destroy/remove food game object that's collided
            Destroy(other.gameObject);
            FindFirstObjectByType<FoodSpawner>().SpawnFood();
            score++;
        }
        else if (other.CompareTag("Barrier"))
        {
            //restart the game
            EndGame();
        }
    }

    private void Extend()
    {
        Vector3 tailSegmentPosition = new Vector3(0,0,0);

        if (tailSegmentTransforms.Count == 0)
        {
            tailSegmentPosition = this.transform.position - (Vector3)direction;

            GameObject newTailSegment = Instantiate(tailSegmentPrefab, tailSegmentPosition, Quaternion.identity);
            tailSegmentTransforms.Add(newTailSegment.transform);        

        }
        else
        {
            Transform lastTailSegmentPosition = tailSegmentTransforms[tailSegmentTransforms.Count - 1];
            tailSegmentPosition = lastTailSegmentPosition.position;
            GameObject newSegment = Instantiate(tailSegmentPrefab, tailSegmentPosition, Quaternion.identity);
            tailSegmentTransforms.Add(newSegment.transform);
        }
        
        

    }

    private void EndGame()
    {
        Debug.Log("Game is over");
        GameManager.Instance.ShowScore(score, elapsedTime);
        Time.timeScale = 0; //pauses the game
        
    }

    IEnumerator IncreaseDifficulty()
{
    while (moveTimeInterval > minMoveTimeInterval)
    {

        yield return new WaitForSeconds(difficultyIncreaseTimeInterval);
        moveTimeInterval = Mathf.Max(moveTimeInterval - difficultyIncreaseStep, minMoveTimeInterval);
        Debug.Log("<size=15><color=red><b>Warning:</b></color></size> Increased difficulty:" + moveTimeInterval);
    }
} 


}


