using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBtn : MonoBehaviour
{
    [SerializeField] private ChangeScene _changeScene;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Hand")){
            _changeScene.ChangerSceneAvecInvoke("Game", 1f);
        }
    }
}
