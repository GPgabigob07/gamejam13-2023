﻿using UnityEngine;
using UnityEngine.UI;

namespace GameJam
{
    public class Paralax : MonoBehaviour
    {
        [SerializeField] private Image fundo;
        [SerializeField] private float velocidade = 5;

        private void Update()
        {
            MoveFundo();
        }

        public void MoveFundo()
        {
            transform.position = new Vector3(transform.position.x - velocidade * Time.deltaTime * Input.GetAxisRaw("Horizontal"), 0, 0);

            if (transform.localPosition.x >= fundo.preferredWidth)
            {
                transform.localPosition = new Vector3(transform.localPosition.x - fundo.preferredWidth * 2, 0, 0);
            }
            else if (transform.localPosition.x <= -fundo.preferredWidth)
            {
                transform.localPosition = new Vector3(transform.localPosition.x + fundo.preferredWidth * 2, 0, 0);
            }
        }
    }
}
