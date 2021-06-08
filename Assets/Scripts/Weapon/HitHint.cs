using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HitHint : MonoBehaviour
{
    Image m_Image;
    public float disSovleSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        m_Image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Image.color = new Color(m_Image.color.r, m_Image.color.g, m_Image.color.b, Mathf.Lerp(m_Image.color.a, 0, Time.deltaTime * disSovleSpeed));
    }

    public void Show()
    {
        m_Image.color = new Color(m_Image.color.r, m_Image.color.g, m_Image.color.b, 255);
    }
}
