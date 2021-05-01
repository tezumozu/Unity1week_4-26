using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Player;
using GameManagers;

namespace Traps{
    public class Hole : Trap{
        void Start(){
            //初期位置を乱数で決定
            Vector3 initPos = this.transform.position;
            initPos.y = 6.5f;
            initPos.z = 6.0f;
            if(UnityEngine.Random.Range(0f,1.0f) < 0.5){
                initPos.x = -4.25f;
            }else{
                initPos.x = 4.25f;
            }
            this.transform.position = initPos;

            this.kind = TrapKind.HOLE;
            this.UpdateAsObservable()
                .Where(_ => StatusManager.status.Value == GameStatus.GAME)
                .Subscribe(_ => {
                    Vector3 pos = this.transform.position;
                    pos.y -= velocity;

                    this.transform.position = pos;
                    if(this.transform.position.y < destoryPoint - this.transform.localScale.y/2){
                        Destroy(gameObject);
                    }
                });
            
            //ステージリセット時の処理
            StatusManager.status
                .Where(status => status == GameStatus.RESET)
                .Subscribe(_ => {
                    Destroy(gameObject);
                })
                .AddTo(gameObject);
        }

        void OnCollisionEnter2D(Collision2D collision){

            IBeCaught kogamo = collision.gameObject.GetComponent<IBeCaught>();
            if(kogamo == null){
                return;
            }
            kogamo.caught(this.kind,this);
        }
    }
}

