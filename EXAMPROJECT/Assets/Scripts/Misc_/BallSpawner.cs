using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.Random;
using Random = UnityEngine.Random;

public class BallSpawner : MonoBehaviour
{
    public static BallSpawner instance;
    public GameObject PrefabBall;

    private void Awake()
    {
        instance = this;
    }
    private string GenerateUniqueID()
    {
        Ball[] balls = FindObjectsByType<Ball>(FindObjectsSortMode.None);
        return CheckBallID(balls).ToString();
        
    }

    private int CheckBallID(Ball[] balls)
    {
        bool UniqueIdFound = false;
        int ID = 0;
        while (!UniqueIdFound)
        {
            if (balls.Length <=0)
            {
                UniqueIdFound = true;
            }
            else
            {
                bool IDExists = false;
                for (int i = 0; i < balls.Length; i++)
                {
                    if (ID.ToString() == balls[i].id)
                    {
                        IDExists = true;
                        break;
                    }
                    
                }

                if (!IDExists)
                {
                    UniqueIdFound = true;
                }
                else ID++;
            }
            
            
        }

        return ID;

    }

    [ContextMenu("Spawn random ball")]
    public void SpawnRandomBall()
    {
      SpawnBall(PrefabBall);
    }

    public GameObject SpawnBall(GameObject ball)
    {
       
        
        ball.GetComponent<Ball>().id = GenerateUniqueID();

        int randX = Random.Range(-13, 13);
        int randY = Random.Range(-5, 10);
        return Instantiate(ball, new Vector3(randX, randY), quaternion.identity);

    }
}
