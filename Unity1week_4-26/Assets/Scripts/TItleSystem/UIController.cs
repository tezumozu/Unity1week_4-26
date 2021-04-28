using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

using Inputs;
using SceneLoads;

namespace TitleSystem{
    enum UIStatus{
        RUN,
        WAIT
    }
    public class UIController : MonoBehaviour{
        UIStatus status;
        IDisposable pressed;
        void Start(){
            status = UIStatus.WAIT;
            Controller con =  GameObject.Find("KeyController").GetComponent<Controller>();
            //Subscribeの設定

            pressed = con.pressedIn
            .Where(_ => status == UIStatus.WAIT)
            .Subscribe(button => {
                if(button == ButtonKind.START){
                    status = UIStatus.RUN;
                    SceneLoader loader =  GameObject.Find("SceneLoader").GetComponent<SceneLoader>();

                    //購読の終了
                    pressed.Dispose();
                    loader.LoadScene("GameScene");
                }
            });
        }
    }
}

