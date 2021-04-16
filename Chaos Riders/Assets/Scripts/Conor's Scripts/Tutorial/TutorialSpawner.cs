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
        tps = FindObjectOfType<TutorialPlayerSettings>();
        StartCoroutine(SpawnPlayerCorotine(spawnDelay));
        StartCoroutine(SpawnAICorotine(spawnDelay));
    }


    void Update()
    {
        //testing
        if(Input.GetKeyDown(KeyCode.Space))
        {
            int randomNum = Random.Range(0, aiCars.Length);
            Debug.Log("Random number = " + randomNum);
        }
    }

    //Used for spawning your own player and AI companion
    private IEnumerator SpawnPlayerCorotine(float time)
    {
        yield return new WaitForSeconds(time);

        //Braker
        if(tps.playerType == PlayerType.Braker)
        {
            //spawn player
            Instantiate(Braker, playerSpawnPoint.position, playerSpawnPoint.rotation);

            //spawn ai
            Instantiate(aiGun, playerSpawnPoint.position, playerSpawnPoint.rotation);
        }

        //Shredder
        if(tps.playerType == PlayerType.Shredder)
        {
            //spawn player
            Instantiate(Shredder, playerSpawnPoint.position, playerSpawnPoint.rotation);

            //spawn ai
            Instantiate(aiGun, playerSpawnPoint.position, playerSpawnPoint.rotation);
        }

        //Standard Gun
        if (tps.playerType == PlayerType.StandardShooter)
        {
            //spawn player
            Instantiate(StandardGun, playerSpawnPoint.position, playerSpawnPoint.rotation);

            //spawn ai
            int randomNum = Random.Range(0, aiCars.Length);
            Instantiate(aiCars[randomNum], playerSpawnPoint.position, playerSpawnPoint.rotation);
        }

        //Golden Gun
        if (tps.playerType == PlayerType.GoldenGun)
        {
            //spawn player
            Instantiate(GoldenGun, playerSpawnPoint.position, playerSpawnPoint.rotation);

            //spawn ai
            int randomNum = Random.Range(0, aiCars.Length);
            Instantiate(aiCars[randomNum], playerSpawnPoint.position, playerSpawnPoint.rotation);
        }
    }

    //used for spawning the addition ai cars selected in the tutorial menu screen
    private IEnumerator SpawnAICorotine(float time)
    {
        yield return new WaitForSeconds(time);

        for (int i = 0; i < tps.amountOfAI; i++)
        {
            int randomNum = Random.Range(0, aiCars.Length);
            Instantiate(aiCars[randomNum], aiSpawnPoints[i].position, aiSpawnPoints[i].rotation); //spawn ai car

            Instantiate(aiGun, aiSpawnPoints[i].position, aiSpawnPoints[i].rotation); //spawn ai gun
        }
    }
}
