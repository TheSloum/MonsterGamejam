using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGen : MonoBehaviour
{
    [Header("Toutes les rooms")]
    public List<GameObject> allPrefabs;

    [Header("Sélection Random")]
    public int numberToSelect = 3;   
    private List<GameObject> selectedPrefabs;

public GameObject player; 
    private GameObject currentInstance;
    private int currentIndex = 0;

    void Start()

    {
        selectedPrefabs = GetRandomPrefabs(allPrefabs, numberToSelect);

        NextRoom();
    }

    public void NextRoom()
    {
        if (currentInstance != null)
            Destroy(currentInstance);

        if (currentIndex >= selectedPrefabs.Count)
        {
            return;
        }

        currentInstance = Instantiate(selectedPrefabs[currentIndex], transform.position, Quaternion.identity);
        currentIndex++;

         Transform spawnPoint = currentInstance.transform.Find("Spawn");
        {
            player.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        }
    }

    private List<GameObject> GetRandomPrefabs(List<GameObject> sourceList, int count)
    {
        List<GameObject> result = new List<GameObject>();
        List<GameObject> copy = new List<GameObject>(sourceList);

        count = Mathf.Min(count, copy.Count);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, copy.Count);
            result.Add(copy[index]);
            copy.RemoveAt(index);
        }

        return result;
    }
}
