using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodePadBtn : MonoBehaviour
{
    [SerializeField] private CodepadHandler _codePadHandeler;
    private bool _canInteract = true; 
    private int _digit; // int qui contient le numéro qui a été assigné au bouton
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision other)  // Est appelé automatiquement quand le un autre GameObject entre en collision avec le bouton
    {
        if((_canInteract && other.gameObject.tag == "Hand")){ // - Si la porte n'est pas déverouillé et si le tag de l'autre objet impliqué dans l'impact == "Player" (joueur)
            Debug.Log(_digit);
            _canInteract = false;
            Invoke("ReActivateBtn", 0.6f);
            _codePadHandeler.AddAnswer(_digit); // Ajoute le chiffre lui correspondant à la liste _essai du PuzzleManager
        }
    }
    // private void OnCollisionExit(Collision other)
    // {
    //     _canInteract = true;
    // }
    private void ReActivateBtn()
    {
        _canInteract = true;
    }
    public void AssignDigit(int digit) // Est appeler au début du jeu par le CodepadHandler
    {
        _digit = digit; // Affecte la valeur passé en paramètre à la variable _digit
    }
}
