using DG.Tweening;
using UnityEngine;
namespace BreakCandy.Scripts
{
    public enum DicrectionType
    {
        Up, Down, Left, Right
    }
    
    public class LineCandy : MonoBehaviour
    {
        [SerializeField] private Transform neo1;
        [SerializeField] private Transform neo2;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private CandyManager candyManager;
        [SerializeField] private DicrectionType dicrectionType;
        private bool activeAnim;
        private void Reset()
        {
            sprite = GetComponent<SpriteRenderer>();
            if (transform.childCount <= 0) return;
            neo1 = transform.GetChild(0);
            neo2 = transform.GetChild(1);
        }

        private int dem1 = 0;
        private int dem2 = 0;
        public void CheckPosNeo(Vector3 pos, bool playSound)
        {
            if (!neo1 && !neo2) return;
            if (Vector3.Distance(pos, neo1.position) <= candyManager.doLech)
            {
                dem1++;
            }

            if (Vector3.Distance(pos, neo2.position) <= candyManager.doLech)
            {
                dem2++;
            }

            if (dem1 >= 5 && dem2 >= 5 && !activeAnim)
            {
                if (playSound)
                {
                    /*AudioManager.instance.PlaySoundBreakCandy();*/
                }
                ActiveAnimationBreak(0.5f);
                
                activeAnim = true;
            }
        }

        public void ActiveAnimationBreak(float time)
        {
            Vector3 targetPosition = transform.position;

            if (dicrectionType == DicrectionType.Right)
            {
                targetPosition.x += 2f;
            }
            else if (dicrectionType == DicrectionType.Left)
            {
                targetPosition.x += -2f;
            }
            else if (dicrectionType == DicrectionType.Down)
            {
                targetPosition.y += -2f;
            }
            else if (dicrectionType == DicrectionType.Up)
            {
                targetPosition.y += 2f;
            }
            else
            {
                targetPosition.x += 1f;
                targetPosition.y -= 1f;
            }

            transform.DOMove(targetPosition, time).SetEase(Ease.Linear)
                     .OnStart(() =>
                     {
                         sprite.DOFade(0, time + 0.2f).OnComplete(()=>gameObject.SetActive(false));
                     });
        }
    }
}