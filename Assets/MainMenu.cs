using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    public GameObject levelSelectMenu;

    // Start is called before the first frame update
    void Start()
    {
        levelSelectMenu.SetActive(false);
        
    }

    public void Quit() {
        Application.Quit();
    }

}
