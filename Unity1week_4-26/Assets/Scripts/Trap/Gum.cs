using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Player;
using GameManagers;

namespace Traps{
    public class Gum : Trap{
        int count = 1;

        // Start is called before the first frame update
        void Start(){
            //初期位置を乱数で決定
            Vector3 initPos = this.transform.position;
            initPos.y = 6;
            initPos.x = UnityEngine.Random.Range(-2.0f,2.0f);
            initPos.z = 5.0f;
            this.transform.position = initPos;

            this.kind = TrapKind.GUM;
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
            if(count <= 0){
                return;
            }

            IBeCaught kogamo = collision.gameObject.GetComponent<IBeCaught>();
            if(kogamo == null){
                return;
            }

            count --;
            kogamo.caught(this.kind,this);

        }
    }
}

