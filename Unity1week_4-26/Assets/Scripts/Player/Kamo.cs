using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
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

        [SerializeField]
        GameObject preKogamo;

        [SerializeField]
        Text kamoNumText;
        
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
                }).AddTo(gameObject);

            con.releasedIn
                .Where(_ => StatusManager.status.Value == GameStatus.GAME)
                .Subscribe(button => {
                    buttonStatus[button] = false;
                }).AddTo(gameObject);

            //ゲーム開始時の初期化
            StatusManager.status
                .Where(status => status == GameStatus.INIT)
                .Subscribe(_ => {
                    childNum.Value = 9;
                }).AddTo(gameObject);

            //ゲーム開始の準備
            StatusManager.status
                .Where(status => status == GameStatus.READY)
                .Subscribe(_ => {
                    //keyの初期化
                    buttonStatus[ButtonKind.UP] = false;
                    buttonStatus[ButtonKind.DOWN] = false;
                    buttonStatus[ButtonKind.RIGHT] = false;
                    buttonStatus[ButtonKind.LEFT] = false;
                    buttonStatus[ButtonKind.START] = false;
                    //初期値へ移動
                    this.transform.position = new Vector3(0.0f,-3.0f,4.0f);

                    //子がもの生成
                    for(int i = 0; i < childNum.Value; i++){
                        Vector3 pos = this.transform.position;
                        float rand_x = UnityEngine.Random.Range(-1.0f,1.0f);
                        float rand_y = UnityEngine.Random.Range(-0.2f,0.5f);
                        pos.y -= 1.0f + rand_y;
                        pos.x += rand_x;


                        ChildKamo kogamo = Instantiate(preKogamo,pos,Quaternion.identity).GetComponent<ChildKamo>();
                        kogamo.setParent(this,rand_x,rand_y);
                        kogamo.beCaught.Subscribe(caught => {
                            childNum.Value --;
                            kamoNumText.text = childNum.Value.ToString();
                        });
                    }
                }).AddTo(gameObject);

            this.UpdateAsObservable()
            .Where(_ => StatusManager.status.Value == GameStatus.GAME)
            .Subscribe(_ => {
                move();
            });
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
            float velocity = 0.1f;

            if(buttonStatus[ButtonKind.UP]){
                if(pos.y < limitY){
                    pos.y += velocity;
                }else{
                    pos.y = limitY;
                }
            }else if(buttonStatus[ButtonKind.DOWN]){
                if(pos.y > -limitY+1.0f){
                    pos.y -= velocity;
                }else{
                    pos.y = -limitY+1.0f;
                }
            }

            if(buttonStatus[ButtonKind.RIGHT]){
                if(pos.x < limitX){
                    pos.x += velocity;
                }else{
                    pos.x = limitX;
                }
            }else if(buttonStatus[ButtonKind.LEFT]){
                if(pos.x > -limitX){
                    pos.x -= velocity;
                }else{
                    pos.x = -limitX;
                }
            }

            this.transform.position = pos;
        }

    }
}

