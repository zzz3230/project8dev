using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Test_uicode : MonoBehaviour
{
    public UIDocument uiDoc;
    // Start is called before the first frame update
    void Start()
    {

        uiDoc.rootVisualElement.Q("start-btn").RegisterCallback<ClickEvent>( 
            (ev) => { 
                
                //print("Hello world!"); 
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
