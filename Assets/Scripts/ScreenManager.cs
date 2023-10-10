using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> screens;

    [SerializeField]
    private int screenIndex = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            TurnLeft();
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            TurnRight();
    }
    
    void TurnLeft()
    {
        if (screenIndex - 1 < 0)
            screenIndex = screens.Count - 1;
        else
            screenIndex--;

        UpdateActiveScreen();
    }

    void TurnRight()
    {
        if (screenIndex + 1 >= screens.Count)
            screenIndex = 0;
        else
            screenIndex++;

        UpdateActiveScreen();
    }

    void UpdateActiveScreen()
    {
        int idx = 0;
        foreach (GameObject screen in screens)
        {
            if (idx != screenIndex)
                screen.SetActive(false);
            else
                screen.SetActive(true);

            idx++;
        }
    }
}
