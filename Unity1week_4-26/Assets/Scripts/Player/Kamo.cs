using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Inputs;
using GameManagers;

namespace Player{
    public class Kamo : MonoBehaviour{
        float limitX = 4.5f;
        float limitY = 4.5f;
        ReactiveProperty<int> childNum = new ReactiveProperty<int>(9);
        public IReadOnlyReactiveProperty<int> child{
            get{
                return childNum;
            }
        }
        Controller con;
        Dictionary<ButtonKind,bool> buttonStatus = new Dictionary<ButtonKind, bool>();
        void Start(){

            buttonStatus.Add(ButtonKind.UP,false);
            buttonStatus.Add(ButtonKind.DOWN,false);
            buttonStatus.Add(ButtonKind.RIGHT,false);
            buttonStatus.Add(ButtonKind.LEFT,false);
            buttonStatus.Add(ButtonKind.START,false);

            con = GameObject.Find("KeyController").GetComponent<Controller>();
            con.pressedIn
                .Where(_ => StatusManager.status.Value == GameStatus.GAME)
                .Subscribe(button => {
                    buttonDown(button);
                });

            con.releasedIn
                .Where(_ => StatusManager.status.Value == GameStatus.GAME)
                .Subscribe(button => {
                    buttonStatus[button] = false;
                });

            //ゲーム開始時の初期化
            StatusManager.status
                .Where(status => status == GameStatus.INIT)
                .Subscribe(_ => {
                    childNum.Value = 9;
                    //子がもの生成
                    for(int i = 0; i < childNum.Value; i++){

                    }
                });

            this.UpdateAsObservable()
            .Where(_ => StatusManager.status.Value == GameStatus.GAME)
            .Subscribe(_ => {
                move();
            });
        }

        public void setChilde(ChildKamo kogamo){
            
        }

        void buttonDown(ButtonKind button){
            buttonStatus[button] = true;
            if(button == ButtonKind.UP){
                buttonStatus[ButtonKind.DOWN] = false;

            }else if(button == ButtonKind.DOWN){
                buttonStatus[ButtonKind.UP] = false;

            }else if(button == ButtonKind.RIGHT){
                buttonStatus[ButtonKind.LEFT] = false;

            }else if(button == ButtonKind.LEFT){
                buttonStatus[ButtonKind.RIGHT] = false;
            }
        }

        void move(){
            Vector3 pos = this.transform.position;

            if(buttonStatus[ButtonKind.UP]){
                if(pos.y < limitY){
                    pos.y += 0.1f;
                }else{
                    pos.y = limitY;
                }
            }else if(buttonStatus[ButtonKind.DOWN]){
                if(pos.y > -limitY+1.0f){
                    pos.y -= 0.1f;
                }else{
                    pos.y = -limitY+1.0f;
                }
            }

            if(buttonStatus[ButtonKind.RIGHT]){
                if(pos.x < limitX){
                    pos.x += 0.1f;
                }else{
                    pos.x = limitX;
                }
            }else if(buttonStatus[ButtonKind.LEFT]){
                if(pos.x > -limitX){
                    pos.x -= 0.1f;
                }else{
                    pos.x = -limitX;
                }
            }

            this.transform.position = pos;
        }

    }
}

