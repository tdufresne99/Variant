using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    private string scene; // String contenant le nom d'une scène
    void Start()
    {

    }
    public void Bouton(string laScene) // Fonction qui 
    {
        scene = laScene; // Affecte le string correspondant au nom de la scène  à la variable scene
        Invoke("ChangerScene", 0.3f); // Appelle la fonction ChangerScene() après 0.3s
    }
    public void ChangerScene() // Fonction qui charge une nouvelle scene qu'elle reçoit en parametre sous forme de string
    {
        SceneManager.LoadScene(scene);
    }

    public void ChangerSceneAvecInvoke(string nomScene, float temps)
    {
        scene = nomScene;
        Invoke("ChangerScene", temps);
    }
}
