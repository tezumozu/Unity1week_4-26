using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using GameManagers;
using Player;

namespace Traps{
    public class Cat :Trap{
        bool direction = true;//true 右 , false 左
        Kamo kamo;
        bool cacthingFlag = false;
        GameObject hand;
        Vector3 handVelocity = Vector3.zero;
        void Start(){
            //初期値を乱数で決定
                //kamoの取得
            kamo = GameObject.Find("Kamo").GetComponent<Kamo>();
                //手を取得
            hand = transform.Find("Cat_hand").gameObject;

            Vector3 initPos = this.transform.position;
            initPos.y = 6.5f;

            if(UnityEngine.Random.Range(0f,1.0f) < 0.5){
                initPos.x = -5f;
                direction = false;
                hand.transform.Rotate(new Vector3(0,0,180f));
            }else{
                initPos.x = 5f;
                direction = true;
                hand.transform.position = new Vector3(hand.transform.position.x + 3.5f, hand.transform.position.y, 5.0f);
            }
            this.transform.position = initPos;
            

            this.kind = TrapKind.CAT;
            this.UpdateAsObservable()//移動
                .Where(_ => StatusManager.status.Value == GameStatus.GAME)
                .Subscribe(_ => {
                    Vector3 pos = this.transform.position;
                    pos.y -= velocity;

                    this.transform.position = pos;
                    if(this.transform.position.y < destoryPoint - this.transform.localScale.y/2){
                        Destroy(gameObject);
                    }

                    if(Vector3.Distance(pos,kamo.transform.position) < 5.5f){
                        cacthingFlag = true;
                    }
                });
            
            this.UpdateAsObservable()//手を伸ばす
                .Where(_ => StatusManager.status.Value == GameStatus.GAME)
                .Where(_ => {return cacthingFlag;})
                .Subscribe(_ => {
                    //手を取得
                    Vector3 targetPos = hand.transform.position;

                    if(direction){//右
                        targetPos.x = 3.5f;
                    }else{//左
                        targetPos.x = -3.5f;
                    }

                    hand.transform.position = Vector3.SmoothDamp(hand.transform.position,targetPos,ref handVelocity,0.5f);;
                });
            
            //ステージリセット時の処理
            StatusManager.status
                .Where(status => status == GameStatus.RESET)
                .Subscribe(_ => {
                    Destroy(gameObject);
                })
                .AddTo(gameObject);
        }
    }
}