using System.Collections.Generic;
using BreakCandy.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChoiceCandy : MonoBehaviour
{
    public int index;
    [SerializeField] private GameObject objectAds;
    [SerializeField] private List<Sprite> objectSprites;
    [SerializeField] private Image objectImage;
    public bool Reward;
    public Button btnChoice;
    public bool CanReward
    {
        get { return  PlayerPrefs.GetInt($"RewardCandy: {index}", 1) == 1; }
        set
        {
            PlayerPrefs.SetInt($"RewardCandy: {index}", value?1:0);
        }
    }
    private void Start()
    {
        if (!Reward)
        {
            PlayerPrefs.SetInt($"RewardCandy: {index}", 0);
        }
        if (!CanReward)
        {
            objectAds.SetActive(false);
        }
        btnChoice.onClick.AddListener(choiceCandy);
    }
    private void OnValidate()
    {
        objectImage = transform.GetChild(0).GetComponent<Image>();
        objectAds = transform.GetChild(1).gameObject;
    }
    public void choiceCandy()
    {
       if(!GameManager.instance.CanChoice) return;
        if (GameManager.instance.indexCurrentCandy != index)
        {
            bool isLeft;
            if (index < GameManager.instance.indexCurrentCandy)
            {
                isLeft = true;
            }
            else
            {
                isLeft = false;
            }
            GameManager.instance.AmountChoiceCandy += 1;
            GameManager.instance.ChoiceCandy(index, isLeft);  
            GameManager.instance.CanChoice = false;
        }
    }
    public void ActiveOutlineCurrentCandy()
    {
        objectImage.sprite = objectSprites[1];
    }
    public void DeactiveOutlineCurrentCandy()
    {
        objectImage.sprite = objectSprites[0];
    }
}
