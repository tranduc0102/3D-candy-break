using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

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
        [Header("Requierce")]
        public GameObject effectBreak;
        public Camera Cam;
        [SerializeField] private ControlStick controlStick;
        [Space]
        [Header("Data Stick")]
        [SerializeField] private List<Transform> items = new List<Transform>();
        
        [Space]
        [Header("Data Candy In Level")]
        [SerializeField] private List<Transform> dataCandyInLevel = new List<Transform>();
        
       private void Start()
       {
           
       }
       private void Update()
       {
           if (Input.GetKeyDown(KeyCode.A))
           {
               controlStick.ChangeStick(items[Random.Range(0, items.Count)]);
           }
       }
    }
}
