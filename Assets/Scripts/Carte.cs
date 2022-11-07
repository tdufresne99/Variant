using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carte : MonoBehaviour
{
    [SerializeField] private TimerManager _timerManager;
    [SerializeField] private Door Door;
    [SerializeField] private GameObject _monstre;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Door")){
            Door.CloseDoor();
            _monstre.SetActive(true);
            _timerManager.StartTimer();
        }
    }
}
