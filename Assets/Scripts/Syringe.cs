using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syringe : MonoBehaviour
{
    [SerializeField] AudioSource _asSyringe;
    [SerializeField] private Animator _monstreAnim;
    [SerializeField] private GameObject _monstre;
    [SerializeField] private PathManager _pathScript;
    [SerializeField] private Monstre _monstreScript;
    private Vector3 _startPosition = new Vector3(2.2f,0f,28.5f);
    void Start()
    {
        _asSyringe = GetComponent<AudioSource>();
        _monstreAnim = _monstre.GetComponent<Animator>();
    }
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Monstre")){
            _asSyringe.Play();
            _pathScript.stab = true;
            _monstreScript.canKill = false;
            _monstreAnim.SetTrigger("stab");
            Debug.Log("monstre hit");
            Invoke("ResetMonstre", 10f);
            gameObject.SetActive(false);
        }
    }
    private void ResetMonstre()
    {
        _monstre.SetActive(true);
        _pathScript.chase = false;
        _pathScript.stab = false;
        _monstre.transform.position = _startPosition;
    }
}
