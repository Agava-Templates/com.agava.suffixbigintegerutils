using UnityEngine;
using System;
using System.Numerics;

namespace Agava.SuffixBigIntegerUtils
{
    [Serializable]
    public struct SerializableBigInteger
    {
        [SerializeField] private string _value;

        public void Validate()
        {
            try
            {
                _value.ToBigIntegerFromSuffixFormat();
            }
            catch (Exception e)
            {
                Debug.LogError(_value + ": " + e.Message);
                _value = "0";
            }
        }

        public BigInteger BigInteger => _value.ToBigIntegerFromSuffixFormat();
    }
}