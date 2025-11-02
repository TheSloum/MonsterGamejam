using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tuto : MonoBehaviour
{
    public GameObject textToDisppearMoove;
    public GameObject textToAppear;
    private PlayerMovement playerMovement;
    private bool amiPeutApparaitre = true;

    // Start is called before the first frame update
    void Start()
    {
    playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    IEnumerator Wait()
    {
        // Attend 2 secondes
        yield return new WaitForSeconds(2f);



            textToDisppearMoove.SetActive(false);
            if(amiPeutApparaitre == true){
                textToAppear.SetActive(true);
                amiPeutApparaitre = false;
            }
            
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerMovement.IsMoving())
            {
                    StartCoroutine(Wait());
                    
            }
    }

    
}
