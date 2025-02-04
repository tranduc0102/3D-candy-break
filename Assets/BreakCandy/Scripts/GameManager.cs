using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Touch;
using UnityEngine;
using UnityEngine.Events;

namespace BreakCandy.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                return;
            }
            Destroy(this);
        } 
        [SerializeField] private List<CandyManager> dataCandy = new List<CandyManager>();
        [SerializeField] private List<ChoiceCandy> dataChoiceCandy = new List<ChoiceCandy>();
        private bool canChoice = false;
        public bool CanChoice
        {
            get => canChoice;
            set => canChoice = value;
            
        }
        private Transform currentCandy;
        private Transform nextCandy;
        
        public GameObject effectBreak;
        public Camera Cam;
        public int indexCurrentCandy = 0;
        public Transform parent;
        public int AmountChoiceCandy = 0;
       private void Start()
       {
           ChoiceCandy(indexCurrentCandy,true);
           AmountChoiceCandy = 0;
       }
      
       public void NextCandy()
       {
           dataChoiceCandy[indexCurrentCandy].DeactiveOutlineCurrentCandy();
           do
           {
               indexCurrentCandy += 1;
               if (indexCurrentCandy >= dataCandy.Count)
               {
                   indexCurrentCandy = 0;
               }
           } while (dataChoiceCandy[indexCurrentCandy].CanReward);
           canChoice = false;
           dataChoiceCandy[indexCurrentCandy].ActiveOutlineCurrentCandy();
           DOVirtual.DelayedCall(1.2f, delegate
           {
               if (indexCurrentCandy > 0)
               {
                   currentCandy.DOMoveX(-5f, 1f);
               }
               else
               {
                   currentCandy.DOMoveX(5f, 1f);
               }
               DOVirtual.DelayedCall(1.1f, delegate
               {
                   if (currentCandy != null)
                   {
                       DOTween.Kill(currentCandy);
                       Destroy(currentCandy.gameObject);
                   };
                   if(indexCurrentCandy >= dataCandy.Count) indexCurrentCandy = 0;
                   if (indexCurrentCandy > 0)
                   {
                       currentCandy = Instantiate(dataCandy[indexCurrentCandy], Vector3.right * 5f, Quaternion.identity, parent).transform;
                   }
                   else
                   {
                       currentCandy = Instantiate(dataCandy[indexCurrentCandy], Vector3.left * 5f, Quaternion.identity, parent).transform;
                   }
                   currentCandy.DOMoveX(0f, 1f).OnComplete(() =>
                   {
                       canChoice = true;
                   });
               });
           });
       }
       public void ChoiceCandy(int choice, bool isLeft)
       {
           if (currentCandy != null)
           {
               if (isLeft)
               {
                   currentCandy.DOMoveX(5f, 1f);   
               }
               else
               {
                   currentCandy.DOMoveX(-5f, 1f);   
               }
               dataChoiceCandy[indexCurrentCandy].DeactiveOutlineCurrentCandy();
               indexCurrentCandy = choice;
               dataChoiceCandy[indexCurrentCandy].ActiveOutlineCurrentCandy();
               DOVirtual.DelayedCall(1.1f, delegate
               {
                   if (currentCandy != null)
                   {
                       DOTween.Kill(currentCandy);
                       Destroy(currentCandy.gameObject);
                   }
                   if (isLeft)
                   {
                       currentCandy = Instantiate(dataCandy[choice], Vector3.left * 5f, Quaternion.identity, parent).transform;
                       currentCandy.DOMoveX(0f, 1f).OnComplete(() =>
                       {
                           canChoice = true;
                       });
                   }
                   else
                   {
                       currentCandy = Instantiate(dataCandy[choice], Vector3.right * 5f, Quaternion.identity, parent).transform;
                       currentCandy.DOMoveX(0f, 1f).OnComplete(() =>
                       {
                           canChoice = true;
                       });
                   }
               });
           }
           else
           {
               indexCurrentCandy = choice;
               dataChoiceCandy[indexCurrentCandy].ActiveOutlineCurrentCandy();
               currentCandy = Instantiate(dataCandy[choice], Vector3.left * 5f, Quaternion.identity, parent).transform;
               currentCandy.DOMoveX(0f, 1f).OnComplete(() =>
               {
                   canChoice = true;
               });
           }
       }
    }
}
