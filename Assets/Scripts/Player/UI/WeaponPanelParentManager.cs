using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanelParentManager : MonoBehaviour
{
    public PlayerWeaponManager playerWeaponManager;
    public WeaponPanelManager WeaponPanelPrefab;
    public Image panelParentImg;
    WeaponPanelManager[] weaponPanelManagers;
    //public
    // Start is called before the first frame update
    void Start()
    {
        float startY = -10;
        float offset = playerWeaponManager.weaponList.Count * 50 + 35;
        if (playerWeaponManager)
        {
            foreach (WeaponMsg weaponMsg in playerWeaponManager.weaponList)
            {
                WeaponPanelManager wpm = Instantiate(WeaponPanelPrefab, transform);
                // 将panel设置为transform子级后，panel会显示到parent下方，所以要加上offset才可以。
                wpm.weaponMsg = weaponMsg;
                wpm.posY = startY + offset;
                //wpm.setPos();
                startY -= 50;
            }
        }
        panelParentImg.rectTransform.sizeDelta = new Vector2(
            panelParentImg.rectTransform.sizeDelta.x, 
            20 + playerWeaponManager.weaponList.Count * 50 - 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
