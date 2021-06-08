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
                // ��panel����Ϊtransform�Ӽ���panel����ʾ��parent�·�������Ҫ����offset�ſ��ԡ�
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
