using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemsLeftScript : MonoBehaviour
{
    [SerializeField, Tooltip("the text")]private TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateObjects(int current, int max){
        text.text = current.ToString() + "/" + max;
    }
}
