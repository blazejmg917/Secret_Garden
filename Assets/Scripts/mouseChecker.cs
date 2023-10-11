using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class mouseChecker : MonoBehaviour
{
    [SerializeField, Tooltip("the goal object to be searching for")]private string goalObject = "";
    private bool overGoalObject = false;
    private bool clicking = false;
    private bool pausing = false;
    [System.Serializable] private class CorrectClickEvent : UnityEvent { };
    [System.Serializable] private class InvalidClickEvent : UnityEvent { };
    [SerializeField,Tooltip("the event that is called when the correct object is clicked")]private CorrectClickEvent correctClickEvent;
    [SerializeField, Tooltip("the event that is called when something other than the correct object is clicked")] private InvalidClickEvent invalidClickEvent;
    public GameManager gm;


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

    public void OnClick(InputAction.CallbackContext ctx)
    {
        if(gm.IsPaused()){
            return;
        }
        //Debug.Log("value type: " + ctx.valueType);
        float currentClick = ctx.ReadValue<float>();
        if (clicking && currentClick < .5)
        {
            clicking = false;
        }
        else if (!clicking && currentClick > .5) { 
            clicking = true;
            if (overGoalObject)
            {
                SuccessfulClick();
            }
            else
            {
                InvalidClick();
            }
        }
    }
    private void SuccessfulClick()
    {
        correctClickEvent.Invoke();
    }

    private void InvalidClick()
    {
        invalidClickEvent.Invoke();
    }

    public void UpdateGoalObj(string newGoalObj){
        goalObject = newGoalObj;
    }

    public void Pause(InputAction.CallbackContext ctx){
        float currentPause = ctx.ReadValue<float>();
        if (pausing && currentPause < .5)
        {
            pausing = false;
        }
        else if (!pausing && currentPause > .5) { 
            pausing = true;
            gm.TogglePaused();
        }
    }
}
