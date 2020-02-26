using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{

    public GameObject[] obstacles;
    public Vector2 numberOfObstacles;
    public GameObject coin;
    public Vector2 numberOfCoins;
    public GameObject dobulePoint;
    public Vector2 numberOfPoint;
    public GameObject[] moveObstacles;
    public Vector2 numberOfMoveObstacles;
 

    public List<GameObject> newObstacles;
    public List<GameObject> newCoins;
    public List<GameObject> newPoint;
    public List<GameObject> newMoveObstacles;

    void Start()
    {
        int newNumberOfObstabcles = (int)Random.Range(numberOfObstacles.x, numberOfObstacles.y);
        int newNumberOfCoins = (int)Random.Range(numberOfCoins.x, numberOfCoins.y);
        int newnumberOfPoint = (int)Random.Range(numberOfCoins.x, numberOfCoins.y);
        int newnumberOfMoveObstacles= (int)Random.Range(numberOfMoveObstacles.x, numberOfMoveObstacles.y);

        for (int i = 0; i < newNumberOfObstabcles; i++)
        {
            newObstacles.Add(Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform));
            newObstacles[i].SetActive(false);
        }

        for (int i = 0; i < newNumberOfCoins; i++)
        {
            newCoins.Add(Instantiate(coin,transform));
            newCoins[i].SetActive(false);
        }

        for (int i = 0; i < newnumberOfPoint; i++)
        {
            newPoint.Add(Instantiate(dobulePoint, transform));
            newPoint[i].SetActive(false);
        }

        for (int i = 0; i < newnumberOfPoint; i++)
        {
            newMoveObstacles.Add(Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform));
            newMoveObstacles[i].SetActive(false);
        }

        positionateObstacles();
        positionateCoins();
        positionatePoints();
    }


    void positionateObstacles()
    {
        for (int i = 0; i < newObstacles.Count; i++)
        {
            float posZmin = (282f / newObstacles.Count) + (282f / newObstacles.Count) * i;
            float posZmax= (282f / newObstacles.Count) + (282f / newObstacles.Count) * i+ 1;
            newObstacles[i].transform.localPosition = new Vector3(0, 0, Random.Range(posZmin, posZmax));
            newObstacles[i].SetActive(true);
            if(newObstacles[i].GetComponent<ChangeLane>()!= null)
            {
                newObstacles[i].GetComponent<ChangeLane>().PositionLane();
            }

        }
    }

    void positionateCoins()
    {
        float posZmin = 10f;
        for (int i = 0; i < newCoins.Count; i++)
        {
          
            float posZmax = posZmin + 5f;
            float randomZpos = Random.Range(posZmin, posZmax);
 
            newCoins[i].transform.localPosition = new Vector3(transform.position.x, transform.position.y, randomZpos);
            newCoins[i].SetActive(true);
            newCoins[i].GetComponent<ChangeLane>().PositionLane();
            posZmin = randomZpos + 1;
        }
    }


    void positionatePoints()
    {
        float posZmin = 40f;
        for (int i = 0; i < newPoint.Count; i++)
        {

            float posZmax = posZmin + 40f;
            float randomZpos = Random.Range(posZmin, posZmax);

            newPoint[i].transform.localPosition = new Vector3(transform.position.x, transform.position.y+0.7f, randomZpos);
            newPoint[i].SetActive(true);
            newPoint[i].GetComponent<ChangeLane>().PositionLane();
            posZmin = randomZpos + 100;
        }
    }


    void positionateMoveObstacles()
    {
        for (int i = 0; i < newObstacles.Count; i++)
        {
            float posZmin = (282f / newObstacles.Count) + (282f / newObstacles.Count) * i;
            float posZmax = (282f / newObstacles.Count) + (282f / newObstacles.Count) * i + 1;
            newObstacles[i].transform.localPosition = new Vector3(0, 0, Random.Range(posZmin, posZmax));
            newObstacles[i].SetActive(true);
            if (newObstacles[i].GetComponent<ChangeLane>() != null)
            {
                newObstacles[i].GetComponent<ChangeLane>().PositionLane();
            }

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Player>().IncreaseSpeed();

            if (numberOfObstacles.x<30 && numberOfObstacles.y<40)
            {
                numberOfObstacles = new Vector2(numberOfObstacles.x + 3, numberOfObstacles.y + 3);
            }
           
            transform.position = new Vector3(0, 0, transform.position.z + 282 * 2);
            positionateObstacles();
            positionateCoins();
        }
    }

}
