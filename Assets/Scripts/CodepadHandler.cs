using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CodepadHandler : MonoBehaviour
{
    [SerializeField] private ChangeScene _changeScene;
    [SerializeField] private CodePadBtn[] _codePadBtn;
    [SerializeField] private GameObject[] _codePadVisual;
    [SerializeField] private TextMeshProUGUI[] _codePadVisualText;
    // [SerializeField] private TMP_Text _passwordText;
    private List<int> _answer; // Liste contenant la réponse généré
    private List<int> _wrongAnswer; // Liste contenant la réponse généré
    private List<int> _attempt; // Liste contenant la combinaison tenté par le joueur
    private int _ordre = 0;

    void Start()
    {
        // _passwordText = GetComponent<TMP_Text>();
        for (int i = 0; i < _codePadBtn.Length; i++) // Assigne un numéro à chaque gameObject représentant une partie de la réponse (0 à 3)
        {
            _codePadBtn[i].AssignDigit(i + 1);
        }
        // for (int i = 0; i < _codePadVisual.Length; i++) // Assigne un numéro à chaque gameObject représentant une partie de la réponse (0 à 3)
        // {
        //     _codePadVisualText[i] = _codePadVisual[i].GetComponent<Text>();
        // }
        GenerateAnswer();
    }
    private void GenerateAnswer()
    {
        _answer = new List<int>();
        _wrongAnswer = new List<int>();
        _attempt = new List<int>();
            _answer.Add(1);
            _answer.Add(9);
            _answer.Add(3);
            _answer.Add(8);

            _wrongAnswer.Add(2);
            _wrongAnswer.Add(6);
            _wrongAnswer.Add(4);
            _wrongAnswer.Add(3);
        // for (int i = 0; i < _codeLength; i++) // Génère un réponse aléatoire
        // {
        //     int possibilite = Random.Range(1, 10); // Choisi un nombre aléatoire dans _numerosPossible
        //     _answer.Add(possibilite); // Ajoute cette valeur dans la liste _reponse
        // }
        foreach (int rep in _answer) // Affiche la réponse dans la console
        {
            Debug.Log("Answer: " + rep);
        }
        foreach (int rep in _wrongAnswer) // Affiche la réponse dans la console
        {
            Debug.Log("WrongAnswer: " + rep);
        }
    }
    private void VerifierEssai() // Est appelé lorsque le joueur a appuyer sur tous les boutons ("AjouterEssai()")
    {
        bool rightAnswer = true; // Affecte la valeur true à bonneRep par défaut
        if(_attempt.Count != _answer.Count){ // - Si la longueur de la liste _essai n'est pas identique à celle de _reponse ------------->
            rightAnswer = false; // - > Affecte la valeur false à bonneRep
        } else { // Sinon -------------------------------------------------------------------------------------------------------------->
            for (int i = 0; i < _attempt.Count; i++) // - > Vérifie un par un chaque position des deux liste pour vérifier si elles sont identiques
            { 
                if(_attempt[i] != _answer[i]){ // - - Si la valeur à une position n'est pas identique
                    rightAnswer = false; // - - > Affecte la valeur false à bonneRep
                }
            }
        }
        if(rightAnswer) {
            Debug.Log("YOU WIN!!!");
            _changeScene.ChangerSceneAvecInvoke("Win", 1.5f);

        } else {
            Debug.Log("Try try again..");
            for (int i = 0; i < _codePadVisualText.Length; i++) // Assigne un numéro à chaque gameObject représentant une partie de la réponse (0 à 3)
            {
                _codePadVisualText[i].text = "0";
            }
        }

        bool wrongAnswer = false; // Affecte la valeur true à bonneRep par défaut
        for (int i = 0; i < _attempt.Count; i++) // - > Vérifie un par un chaque position des deux liste pour vérifier si elles sont identiques
        { 
            if(_attempt[i] == _wrongAnswer[i]){ // - - Si la valeur à une position n'est pas identique
                wrongAnswer = true; // - - > Affecte la valeur false à bonneRep
            } else {
                wrongAnswer = false;
                break;
            }
        }
        if(wrongAnswer) {
            Debug.Log("YOU JUST KILLED YOURSELF!!");
            _changeScene.ChangerSceneAvecInvoke("Lose", 1.5f);

        }
        _attempt = new List<int>();
    }
    public void AddAnswer(int attempt)
    {
        _attempt.Add(attempt); // Ajoute le int passé en paramètre à la liste d'essai
        _codePadVisualText[_ordre].text = "" + _attempt[_ordre];
        _ordre++; // Augmente la position à laquelle le joueur est rendu dans la réponse
        if(_ordre > 3){
            _ordre = 0;
            VerifierEssai();
        }
    }
}
