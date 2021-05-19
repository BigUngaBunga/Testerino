using System;
using UnityEngine;
using Mirror;
using Random = System.Random;

namespace BelowUs
{
    public class GoldHandler : NetworkBehaviour
    {
        [SerializeField] private int minimumGoldValue;
        [SerializeField] private int maximumGoldValue;
        [SerializeField] private int value;
        private ShipResource gold;

        [Server]
        private void Start()
        {
            gold = GameObject.Find("CurrentGold").GetComponent<ShipResource>();
            minimumGoldValue += (int)(-transform.position.y / 100);
            maximumGoldValue = (int)(-transform.position.y / 40) + minimumGoldValue;
            Random random = new Random(transform.position.y.ToString().GetHashCode() + Environment.TickCount.ToString().GetHashCode());
            value = random.Next(minimumGoldValue, maximumGoldValue);
        }

        [Server]
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(ReferenceManager.Singleton.SubmarineTag))
            {
                gold.ApplyChange(value);
                RemoveGold();
                Destroy(gameObject);
            }
        }

        [ClientRpc]
        private void RemoveGold() => Destroy(gameObject);
    }
}

