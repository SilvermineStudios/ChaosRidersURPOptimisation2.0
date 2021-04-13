using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSpawner : MonoBehaviour
{
    [SerializeField] private TutorialPlayerSettings tps;
    [SerializeField] private float spawnDelay = 3;

    //players
    [SerializeField] private GameObject Braker;
    [SerializeField] private GameObject Shredder;
    [SerializeField] private GameObject StandardGun;
    [SerializeField] private GameObject GoldenGun;

    //ai
    [SerializeField] private GameObject[] aiCars;
    [SerializeField] private GameObject aiGun;

    public Transform playerSpawnPoint;
    public Transform aiSpawnPointHolder;
    public Transform[] aiSpawnPoints;


    void Start()
    {
        aiSpawnPoints = new Transform[aiSpawnPointHolder.childCount]; //make the array the same length as the amount of children waypoints
        for (int i = 0; i < aiSpawnPointHolder.childCount; i++) //put every waypoint(Child) in the array
        {
            aiSpawnPoints[i] = aiSpawnPointHolder.GetChild(i);
        }

        tps = FindObjectOfType<TutorialPlayerSettings>();
        StartCoroutine(SpawnCorotine(spawnDelay));

    }


    void Update()
    {
        /////////////////////////////////////////////////////////////////////////////////////REMOVE
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(Braker, playerSpawnPoint.position, playerSpawnPoint.rotation);
        }
    }

    private IEnumerator SpawnCorotine(float time)
    {
        yield return new WaitForSeconds(time);

        //Braker
        if(tps.playerType == PlayerType.Braker)
        {
            Instantiate(Braker, playerSpawnPoint.position, playerSpawnPoint.rotation);
            Instantiate(aiGun, playerSpawnPoint.position, playerSpawnPoint.rotation);
            //spawn AI gun
        }

        //Shredder
        if(tps.playerType == PlayerType.Shredder)
        {
            Instantiate(Shredder, playerSpawnPoint);
            Instantiate(aiGun, playerSpawnPoint);
            //spawn AI gun
        }

        //Standard Gun
        if (tps.playerType == PlayerType.StandardShooter)
        {
            Instantiate(StandardGun, playerSpawnPoint);
            //spawn AI car
        }

        //Golden Gun
        if (tps.playerType == PlayerType.GoldenGun)
        {
            Instantiate(GoldenGun, playerSpawnPoint);
            //spawn AI car
        }
    }
}
