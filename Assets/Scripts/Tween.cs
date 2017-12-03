using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexpansion
{
    public class Tween : MonoBehaviour
    {
        private bool _isTweening;
        private bool _isFlipping;

        private float _totalFlip;
        private float _flipSpeed;

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

        public void Flip(float speed)
        {
            _isFlipping = true;
            _totalFlip = 0;
            _flipSpeed = speed;
        }

        public void StopFlip()
        {
            _isFlipping = false;
            _totalFlip = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (_isTweening)
            {
                var newPos = Vector3.MoveTowards(this.transform.position, _destination, speed);
                transform.position = newPos;
            }
            
            if (_isFlipping)
            {
                transform.Rotate(new Vector3(0, _flipSpeed, 0));
                _totalFlip += _flipSpeed;

                // If went further than Flip, clamp to flip.
                if (_totalFlip > 180)
                {
                    _isFlipping = false;
                    float error = _totalFlip - 180;
                    transform.Rotate(new Vector3(0, -_flipSpeed, 0));
                    _totalFlip = 0;
                }
            }
        }
    }
}

