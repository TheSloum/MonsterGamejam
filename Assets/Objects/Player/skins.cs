using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skins : MonoBehaviour
{
public int skinValue = 0;
public int price = 0;
            public AudioSource buySound;
void Update() { if (Input.GetMouseButtonDown(0)) // Left click 
   { 
    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero); 
    if (hit.collider != null && hit.collider.gameObject == gameObject) {
    if(GameScore.Instance.score >= price){
        GameScore.Instance.skin = skinValue;
        
                buySound.Play();  Debug.Log($"Skin set to {skinValue}");

    }
        
}
}}}