using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using DG.Tweening;
using pooling;
using UnityEngine;
using Random = UnityEngine.Random;

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
        [SerializeField] private Vector3 stickOriginTransform;
        [Space]
        [Header("Data Stick")]
        [SerializeField] private List<Transform> items = new List<Transform>();
        
        [Space]
        [Header("Data Candy In Level")]
        [SerializeField] private List<Candy> dataCandyInLevel = new List<Candy>();
        [SerializeField] private Candy currentCandy;
        [SerializeField] private Transform parentCandy;
        private int IndexCurrentCandy
        {
            get => PlayerPrefs.GetInt("IndexCurrentCandy", 0);
            set => PlayerPrefs.SetInt("IndexCurrentCandy", value);
        }
   
   
        public bool IsWin { get; set; }
        public bool IsLosse { get; set; }
        private void Start()
        {
            stickOriginTransform = controlStick.stick.position;
            Candy prefab = dataCandyInLevel[IndexCurrentCandy];

            Quaternion prefabRotation = prefab.transform.rotation;
            currentCandy = PoolingManager.Spawn(dataCandyInLevel[IndexCurrentCandy],new Vector3(0f, 0f, 0f), prefabRotation, parentCandy);
        }
        private void Update()
       {
           if (Input.GetKeyDown(KeyCode.A))
           {
               controlStick.ChangeStick(items[Random.Range(0, items.Count)]);
           }
       }
       public void WinOrLose()
       {
           currentCandy.CheckLosseOrWin();
           if (IsWin)
           {
               //Return Home
               // Update PlayerPrefabs index current Candy
               // Update Candy
               DOVirtual.DelayedCall(1f,() =>
               {
                   IsWin = false;
                   IsLosse = false;
                   NextCandy();

               });
               Debug.Log("Win");
               return;
           }
           if (IsLosse)
           {
               Debug.Log("!Win"); 
               //Show replay
           }
       }
       private void NextCandy()
       {
           currentCandy.transform.DOMoveX(-5f, 1f).OnComplete(() =>
           {
               controlStick.stick.position = stickOriginTransform;
               IndexCurrentCandy += 1;
               Candy prefab = dataCandyInLevel[IndexCurrentCandy];

               Quaternion prefabRotation = prefab.transform.rotation;
               currentCandy = PoolingManager.Spawn(dataCandyInLevel[IndexCurrentCandy],new Vector3(5f, 0f, 0f), prefabRotation, parentCandy);
               currentCandy.transform.DOMoveX(0f, 1f);
           });
       }
       
    }
}
