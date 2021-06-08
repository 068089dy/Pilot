using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanelManager : MonoBehaviour
{
    //public float width = 250;
    //public float height = 50;
    public WeaponMsg weaponMsg;
    public float posY = -10;
    //public 
    public Text magazineTxt;
    public Image weaponIconImg;
    public Image PanelBG;
    // Start is called before the first frame update
    void Start()
    {
        weaponIconImg.sprite = weaponMsg.icon;
        PanelBG.rectTransform.position = new Vector3(
            PanelBG.rectTransform.position.x,
            posY,
            PanelBG.rectTransform.position.z
            );
    }

    public void setPos()
    {
        PanelBG.rectTransform.position = new Vector3(
            PanelBG.rectTransform.position.x,
            posY,
            PanelBG.rectTransform.position.z
            );
    }

    // Update is called once per frame
    void Update()
    {
        int curMagazineNum = weaponMsg.weaponController.curMagazineNum;
        int ammoNum = weaponMsg.weaponController.ammoNum;
        string curMagazineNumS = curMagazineNum + "";
        string ammoNumS = ammoNum + "";
        if (curMagazineNum == -1)
        {
            curMagazineNumS = "¡Þ";
        }
        if (ammoNum == -1)
        {
            ammoNumS = "¡Þ";
        }
        magazineTxt.text = curMagazineNumS + "|" + ammoNumS;
        if (weaponMsg.weaponController.active)
        {
            PanelBG.color = Color.green;
        } else
        {
            PanelBG.color = Color.gray;
        }
    }
}
