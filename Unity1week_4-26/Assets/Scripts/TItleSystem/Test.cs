using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Inputs;

public class Test
{
    // Start is called before the first frame update
    public static void start()
    {
        Controller con =  GameObject.Find("KeyController").GetComponent<Controller>();

        con.pressedIn
            .Subscribe(button => {
                Debug.Log(button);
            });
    }
}
