using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tuto2 : MonoBehaviour
{

    public GameObject textToAppear;
    public GameObject textToDisappear;
    public GameObject textToAppear2;
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
            textToAppear2.SetActive(true);
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {     
            textToAppear.SetActive(true);
            textToDisappear.SetActive(false);
            StartCoroutine(Wait());
            
        }

    }
}
