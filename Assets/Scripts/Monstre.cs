using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monstre : MonoBehaviour
{
    [SerializeField] AudioSource _asMonstre;
    [SerializeField] private Animator _anim;
    [SerializeField] private ChangeScene _changerScene;
    [SerializeField] private GameObject _monstre;
    [SerializeField] private GameObject _player;
    private float _distance;
    private bool _canHit = true;
    private bool _canKill = true;
    public bool canKill{
        get{ return _canKill; }
        set{ _canKill = value; }
    }
    void Start()
    {
        _anim = _monstre.GetComponent<Animator>();
    }
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _canHit)
        {
            Debug.Log("Monstre hit joueur");
            _anim.SetBool("chase", false);
            _anim.SetTrigger("attack");
            _canHit = false;
            Invoke("KillPlayer", 1f);
        }
    }
    private void KillPlayer()
    {
        if(_canKill){
            Debug.Log("Player dead");
            EndGame();
        }
        _canHit = true;
        _canKill = true;
    }
    private void EndGame()
    {
        _changerScene.ChangerSceneAvecInvoke("Lose", 0);
    }
}
