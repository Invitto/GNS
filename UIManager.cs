using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    private static UIManager instance;

    public static UIManager MyInstance
    {   get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }
    
        
      
    

    [SerializeField]
    private ActionButton[] actionButtons;

    

    

    private Stat healthStat;

    [SerializeField]
    private Image portraitFrame;



    private KeyCode action1, action2;

    [SerializeField]
    private GameObject targetFrame;

    [SerializeField]
    private CanvasGroup keybindMenu;

    [SerializeField]
    private CanvasGroup spellBook;



    private GameObject[] keybindButtons;
    private void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");

    }
    void Start()
    {
        
        healthStat = targetFrame.GetComponentInChildren<Stat>();

    }

    // Update is called once per frame
    void Update()
    {
     if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenClose(keybindMenu);
        }
     if (Input.GetKeyDown(KeyCode.P))
        {
            OpenClose(spellBook);
        }
     if (Input.GetKeyDown(KeyCode.B))
        {
            InventoryScript.MyInstance.OpenClose();
        }
    }

    public void ShowTargetFrame(NPC target)
    {
        targetFrame.SetActive(true);

        healthStat.Intitialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);

        portraitFrame.sprite = target.MyPortrait;

        target.healthChanged += new HealthChanged(UpdateTargetFrame);

        target.characterRemoved += new CharacterRemoved(HideTargetFrame);
    }

   public void HideTargetFrame()
    {
        targetFrame.SetActive(false);
    }

    public void UpdateTargetFrame(float health)
        {
        healthStat.MyCurrentValue = health;
        }



    public void UpdateKeyText(string key, KeyCode code)
    {
        Text tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = code.ToString();

    }

    public void ClickActionButton(string buttonName)
   {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }

    

    public void OpenClose (CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void UpdateStackSize(IClickable clickable)
    {
        if (clickable.MyCount > 1)
        {
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.color = Color.white;
            clickable.MyIcon.color = Color.white;
        }
        else
        {
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
        }
        if (clickable.MyCount == 0)
        {
            clickable.MyIcon.color = new Color(0, 0, 0, 0);
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
        }
    }
}
