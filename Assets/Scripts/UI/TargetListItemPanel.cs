using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetListItemPanel : MonoBehaviour
{
    public string title;
    public string note;
    public Actor targetActor;

    public Image hpImg;
    public Text titleTxt;
    public Text noteTxt;

    public bool active;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            GetComponent<Image>().enabled = true;
            hpImg.enabled = true;
            titleTxt.enabled = true;
            noteTxt.enabled = true;
            titleTxt.text = title;
            noteTxt.text = note;
        } else
        {
            GetComponent<Image>().enabled = false;
            hpImg.enabled = false;
            titleTxt.enabled = false;
            noteTxt.enabled = false;
        }
    }

}
