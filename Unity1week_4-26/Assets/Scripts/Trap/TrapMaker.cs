using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using GameManagers;

namespace Traps{
    public class TrapMaker : MonoBehaviour{
        [SerializeField]
        GameObject[] trapList = new GameObject[3];
        float coolTime = 0;

        void Start(){
            this.UpdateAsObservable()
                .Where(_ => StatusManager.status.Value == GameStatus.GAME)
                .Subscribe(_ => {
                    if(coolTime <= 0){
                        int rand = UnityEngine.Random.Range(0,3);
                        Instantiate(trapList[rand],new Vector3(0.0f,7.0f,3.0f),Quaternion.identity);
                        coolTime = (rand + 1)*(0.8f - StageManager.stageLevel*0.1f);
                    }else{
                        coolTime -= Time.deltaTime;
                    }
                });
        }
    }
}
