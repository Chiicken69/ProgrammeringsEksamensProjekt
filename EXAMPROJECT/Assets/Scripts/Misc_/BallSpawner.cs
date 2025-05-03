using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public static BallSpawner instance;
    public GameObject PrefabBall;

    private void Awake()
    {
        //instance = this;
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
            foreach (var ball in balls)
            {
                if (ID.ToString() == ball.id)
                {
                    ID++;
                    
                }
                else
                {
                    UniqueIdFound = true;
                }
               
            }
        }

        return ID;

    }

    [ContextMenu("test")]
    public void testFunction()
    {
        SpawnBall(PrefabBall);
    }

    public GameObject SpawnBall(GameObject ball)
    {
       
        
        ball.GetComponent<Ball>().id = GenerateUniqueID();
        
        
        return Instantiate(ball);

    }
}
