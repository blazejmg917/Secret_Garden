using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClicksLeftScript : MonoBehaviour
{
    [SerializeField, Tooltip("the text to display clicks in")]private TMP_Text itemDisplayText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Display(int lives){
        itemDisplayText.text = lives.ToString();
    }
}
