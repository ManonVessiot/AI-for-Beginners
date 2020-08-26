using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPatient : MonoBehaviour
{

    [Header("Spawn Settings")]
    [Range(0, 1)]
    public float frequencyOfFastSpawn = 0.25f;

    public Limit2D timeRangeFastSpawn;
    public Limit2D timeRangeSpawn;

    [Header("Patients Settings")]

    public GameObject patientPrefab;
    public int numPatients;

    private void Start()
    {
        for (int i = 0; i < numPatients; i++)
        {
            Instantiate(patientPrefab, transform.position, Quaternion.identity);
        }
        Spawn();
    }

    void Spawn()
    {
        Instantiate(patientPrefab, transform.position, Quaternion.identity);
        if (Random.Range(0, 100) < 100 * frequencyOfFastSpawn)
        {
            Debug.Log("FastSpawn");
            Instantiate(patientPrefab, transform.position, Quaternion.identity);
            Invoke("Spawn", Random.Range(timeRangeFastSpawn.minimum, timeRangeFastSpawn.maximum));
        }
        else
        {
            Debug.Log("NOT FastSpawn");
            Invoke("Spawn", Random.Range(timeRangeSpawn.minimum, timeRangeSpawn.maximum));
        }
    }

}
