﻿using Mirror;
using UnityEngine;

namespace BelowUs
{
    public class ShipResource : NetworkBehaviour
    {
        #if UNITY_EDITOR
        public string ResourceName = "";
        #endif

        [SyncVar] private float currentValue;
        public float CurrentValue => currentValue;

        [SerializeField] private bool resetValue;
        [SerializeField] private FloatReference startingValue;
        public FloatReference maximumValue;

        public delegate void ResourceChangedDelegate(float currentHealth, float maxHealth);
        public event ResourceChangedDelegate EventResourceChanged;

        #region Server
        [Server]
        public void ApplyChange(float value)
        {
            currentValue += value;
            EventResourceChanged?.Invoke(currentValue, maximumValue.Value);
        }

        [Server]
        public override void OnStartServer()
        {
            base.OnStartServer();
            if (resetValue)
                currentValue = maximumValue.Value;
        }


        [Command]
        public void CmdDecreaseBy5() => ApplyChange(-5);

        #endregion
    }
} 
