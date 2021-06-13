using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HitHint : MonoBehaviour
{
    Image m_Image;
    public Text text;
    public float disSovleSpeed = 10;
    float lastShowTime;
    // Start is called before the first frame update
    void Start()
    {
        m_Image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Image.color = new Color(m_Image.color.r, m_Image.color.g, m_Image.color.b, Mathf.Lerp(m_Image.color.a, 0, Time.deltaTime * disSovleSpeed));
        if (text && Time.time > lastShowTime + 1f)
        {
            text.text = "";
        }
    }

    public void Show()
    {
        m_Image.color = new Color(m_Image.color.r, m_Image.color.g, m_Image.color.b, 255);
    }

    public void Show(DamageMsg damageMsg)
    {
        lastShowTime = Time.time;
        m_Image.color = new Color(m_Image.color.r, m_Image.color.g, m_Image.color.b, 255);
        Debug.Log("»÷É±" + damageMsg.target.characterType);
        if (damageMsg.target.characterType == CharacterType.ROBOT)
        {
            text.text = "²½±ø»÷É±";
            if (damageMsg.protectileType == ProtectileType.TREAD)
            {
                text.text = "(²ÈÌ¤)²½±ø»÷É±";
            }
        } else if (damageMsg.target.characterType == CharacterType.TITAN)
        {
            text.text = "Ì¹¿Ë»÷É±";
        } else if (damageMsg.target.characterType == CharacterType.BATTERY)
        {
            text.text = "´Ý»ÙÅÚÌ¨";
        }
    }
}
