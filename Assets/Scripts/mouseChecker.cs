using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseChecker : MonoBehaviour
{
    [SerializeField, Tooltip("the goal object to be searching for")]private string goalObject = "";
    private bool overGoalObject = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        if(goalObject == null || goalObject == ""){
            return;
        }
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero, 0);
        overGoalObject = false;
        foreach( RaycastHit2D hit in hits) {
            selectableObject obj;
            obj = hit.collider.gameObject.GetComponent<selectableObject>();
            if (obj && obj.checkIdentifier(goalObject))
            {
                overGoalObject = true;
            }
        }
    }

    public void OnClick()
    {
        if (overGoalObject)
        {
            SuccessfulClick();
        }
        else
        {
            InvalidClick();
        }
    }
    private void SuccessfulClick()
    {
        Debug.Log("succesful click");
    }

    private void InvalidClick()
    {
        Debug.Log("invalid click");
    }
}
