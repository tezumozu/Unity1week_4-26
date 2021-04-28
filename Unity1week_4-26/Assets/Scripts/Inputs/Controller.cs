using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Inputs{
    public class Controller : MonoBehaviour,IGetButton{
        Subject<ButtonKind> pressed = new Subject<ButtonKind>();
        Subject<ButtonKind> released = new Subject<ButtonKind>();
        public IObservable<ButtonKind> pressedIn{
            get{
                return pressed;
            }

        }
        public IObservable<ButtonKind> releasedIn{
            get{
                return released;
            }
        }

        void Update(){
            if(Input.GetKeyDown(KeyCode.Space)){
                pressed.OnNext(ButtonKind.START);
            }

            if(Input.GetKeyDown(KeyCode.UpArrow)){
                pressed.OnNext(ButtonKind.UP);
            }

            if(Input.GetKeyDown(KeyCode.DownArrow)){
                pressed.OnNext(ButtonKind.DOWN);
            }

            if(Input.GetKeyDown(KeyCode.RightArrow)){
                pressed.OnNext(ButtonKind.RIGHT);
            }

            if(Input.GetKeyDown(KeyCode.LeftArrow)){
                pressed.OnNext(ButtonKind.LEFT);
            }

            //UP
            if(Input.GetKeyUp(KeyCode.Space)){
                released.OnNext(ButtonKind.START);
            }

            if(Input.GetKeyUp(KeyCode.UpArrow)){
                released.OnNext(ButtonKind.UP);
            }

            if(Input.GetKeyUp(KeyCode.DownArrow)){
                released.OnNext(ButtonKind.DOWN);
            }

            if(Input.GetKeyUp(KeyCode.RightArrow)){
                released.OnNext(ButtonKind.RIGHT);
            }

            if(Input.GetKeyUp(KeyCode.LeftArrow)){
                released.OnNext(ButtonKind.LEFT);
            }
        }
    }
}

