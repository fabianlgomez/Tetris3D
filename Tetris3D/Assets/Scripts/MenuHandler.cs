using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuHandler : MonoBehaviour
{
    // Start is called before the first frame update

    public void LoadScene(string sceneToLoad){
        SceneManager.LoadScene(sceneToLoad);
    }


}
