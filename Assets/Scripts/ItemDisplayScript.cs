using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemDisplayScript : MonoBehaviour
{
    [SerializeField, Tooltip("the text to display item names in")]private TMP_Text itemDisplayText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Display(string text){
        itemDisplayText.text = text;
    }
}
