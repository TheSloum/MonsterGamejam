using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tokinoto : MonoBehaviour
{

    public float speed = 10f;
    public GameObject score;
        public GameObject cine;

    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
        if(transform.position.x >= 80f){
             transform.Translate(Vector3.left* speed * Time.deltaTime);
            
        }else{
            Vector3 spawnPosition = new Vector3(-10427f,-54f,0f);
            player.position = spawnPosition;     
            score.SetActive(true);
            cine.SetActive(false);
        }
    }
}
