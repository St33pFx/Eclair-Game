using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
   

    // Exp and level of the player
    [Header("Experience Levels")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    //Clase para definir el rango de nivel
    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    public List<LevelRange> levelRanges;

    [Header("UI Elements")]
    public Image xpFillImage;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;
    public int totalBloodPoints;
    public GameObject levelUp;

    private Drop drop;
    

    void Start()
    {
        experienceCap = levelRanges[0].experienceCapIncrease;
       UpdateExpBar();
        UpdateLevelText();
        UpdateScoreText();
    }

    public void AumentarExperiencia(int amount)
    {
        experience += amount;
        LevelUpChecker();
        UpdateExpBar();

        
    }

    void LevelUpChecker()
    {
        if(experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;

            int experienceCapIncrease = 0;
            foreach(LevelRange range in levelRanges)
            {
                if(level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;

            UpdateLevelText();
            Time.timeScale = 0;
            levelUp.SetActive(true);
        }
    }

    void UpdateExpBar()
    {
        xpFillImage.fillAmount = (float)experience / experienceCap;
    }

    void UpdateLevelText()
    {
        levelText.text = "LV - " + level.ToString();
    }

    public void AumentarBloodPoints(int amount)
    {
        totalBloodPoints += amount;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = totalBloodPoints.ToString("D4");
    }



}
