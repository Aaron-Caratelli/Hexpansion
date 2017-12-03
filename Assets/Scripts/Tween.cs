using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexpansion
{
    public class Tween : MonoBehaviour
    {
        private bool _isTweening;
        private Vector3 _destination;
        public float speed;

        public void TweenTo(Vector3 pos, float speed)
        {
            _isTweening = true;
            _destination = pos;
            this.speed = speed;
        }

        public void TweenBy(Vector3 delta, float speed)
        {
            _isTweening = true;
            _destination = transform.position + delta;
            this.speed = speed;
        }

        // Update is called once per frame
        void Update()
        {
            if (_isTweening)
            {
                var newPos = Vector3.MoveTowards(this.transform.position, _destination, speed);
                transform.position = newPos;
            }                
        }
    }
}

