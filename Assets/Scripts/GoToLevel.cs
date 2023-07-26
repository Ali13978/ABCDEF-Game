using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToLevel : MonoBehaviour
{
    public void GoToScene(string sceneToGoTo)
    {
        SceneManager.LoadScene(sceneToGoTo);
    } 
}
