using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameJam;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

namespace GameJam
{
    public class CombatScene : MonoBehaviour
    {

        public Text text;

        // Use this for initialization

        public Camera global;
        public Camera parent;
        public EnemySpawner spawner;
        public Canvas canva;
        public GameObject ship;
        private MicroShipBehaviour shipb;
        void Start()
        {
            shipb = ship.GetComponent<MicroShipBehaviour>();
            shipb.enabled = false;
        }

        public void StartCombat(PlayerPlataformerEntity player, EnemyPlataformMovement enemy)
        {
            var r = UnityEngine.Random.RandomRange(0, 5);
            if (r == 3 || r == 2)
            {
                parent.enabled = true;
                global.enabled = false;
                canva.enabled = true;
                shipb.enabled = true;
                player.CanMove = false;
                StartCoroutine(Combat(player, enemy));
            } else
            {
                enemy.Kill();
            }
        }

        private int ct;
        private IEnumerator Combat(PlayerPlataformerEntity _player, EnemyPlataformMovement _enemy)
        {
            
            ct = _enemy.timeToFight;
            spawner.active = true;
            for (int i = _enemy.timeToFight; i >= 0; i--)
            {
                text.text = "0:" + i.ToString();
                ct--;
                yield return new WaitForSeconds(1);
                
            }

            if (ct <= 0)
            {
                _enemy.Kill();
            }
            else
            {
                EndGame();
            }

            spawner.active = false;
            parent.enabled = false;
            global.enabled = true;
            canva.enabled = false;
            _player.CanMove = true;

            for (int i = 0; i < spawner.hull.childCount; i++)
            {
                Destroy(spawner.hull.GetChild(i));
            }
            
        }

        public void EndGame()
        {
            Debug.Log("Endgame!!!!");
            SceneManager.LoadScene("gameover");
        }
    }

}