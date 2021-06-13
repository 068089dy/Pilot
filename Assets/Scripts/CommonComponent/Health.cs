using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    public float maxHp = 100;
    public float hp = 100;
    public float regHp = 0;

    private void Update()
    {
        if (hp > 0 && regHp > 0) {
            hp += regHp * Time.deltaTime;
            hp = Mathf.Clamp(hp, 0, maxHp);
        }
    }
}
