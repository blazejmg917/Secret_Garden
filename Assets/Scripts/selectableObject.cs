using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectableObject : MonoBehaviour
{
    public enum ObjectType{
        TOOL,
        GNOME
    }
    [SerializeField, Tooltip("if this object is a tool or a gnome")]private ObjectType objType = ObjectType.TOOL;
    [SerializeField, Tooltip("this object's id")] private string id = "";
    [SerializeField, Tooltip("a list of all valid identifiers. Can be used if we want to search for a generic object type rather than a specific object")] private List<string> identifiers = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public bool checkIdentifier(string identifier)
    {
        return (identifier == id || identifiers.Contains(identifier));
    }

    public ObjectType GetObjectType(){
        return objType;
    }

    public string getId(){
        return id;
    }


}
