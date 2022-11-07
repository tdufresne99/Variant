using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private AudioSource _asPorte;
    void Start()
    {
        _asPorte = GetComponent<AudioSource>();
    }
    public void CloseDoor()
    {
        gameObject.SetActive(false);
    }
}
