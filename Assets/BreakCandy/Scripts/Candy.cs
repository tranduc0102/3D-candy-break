using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace BreakCandy.Scripts
{
    
    public class Candy : MonoBehaviour
    {
        [SerializeField] private List<Transform> childObjInChild0;
        [SerializeField] private List<Transform> childObjInChild1;
        [SerializeField] private int countCandyBreakWin;
        private void Reset()
        {
            childObjInChild0 = new List<Transform>();
            childObjInChild1 = new List<Transform>();

            Transform child0 = transform.GetChild(0);
            foreach (Transform child in child0)
            {
                childObjInChild0.Add(child);
            }

            Transform child1 = transform.GetChild(1);
            foreach (Transform child in child1)
            {
                childObjInChild1.Add(child);
            }
        }
        private void OnEnable()
        {
            countCandyBreakWin = childObjInChild1.Count;
            foreach (Transform child in childObjInChild0)
            {
                if (!child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(true);
                }
            }
            foreach (Transform child in childObjInChild1)
            {
                if (!child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
        public void CheckLosseOrWin()
        {
            if (childObjInChild0.Any(candy => !candy.gameObject.activeSelf))
            {
                GameManager.instance.IsLosse = true;
                //Losse
                return;
            }
            countCandyBreakWin--;
            if (countCandyBreakWin == 0)
            {
                GameManager.instance.IsWin = true;
            }
        }
    }
}
