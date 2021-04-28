using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SceneLoads{
    class SceneLoader: MonoBehaviour{
        [SerializeField]
        GameObject ui;
        
        [SerializeField]
        Slider slider;
        string targetName = "";
        public void LoadScene(string name){
            targetName = name;
            StartCoroutine("LoadData");
        }

        IEnumerator LoadData(){
            //シーンの読み込み
            var async = SceneManager.LoadSceneAsync("Assets/Scenes/"+targetName+".unity");

            //UIを有効にする
            ui.SetActive(true);
            while(!async.isDone){
                //UIをいじる
                slider.value = async.progress;

                yield return null;
            }
        }
    }
}