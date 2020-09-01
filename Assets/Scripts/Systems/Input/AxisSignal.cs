using UnityEngine;

namespace Roomba.Systems.Input
{
    public class AxisSignal
    {
        public int id;
        private float _value;

        public float Value
        {
            get
            {
                return _value;
            }
            set
            {
                _lastValue = _value;
                _value = value;
            }
        }

        private float _lastValue;
        public bool Dirty => Mathf.Abs(_value - _lastValue) > 0.01f;

        public AxisSignal(int id, float value)
        {
            this.id = id;
            this.Value = value;
        }

        public override string ToString()
        {
            return $"(id: {this.id}, value: {this.Value})";
        }
    }
}