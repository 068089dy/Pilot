using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(WeaponController))]
public class WeaponSniper : MonoBehaviour
{
    WeaponController m_WeaponController;
    public float loadingSpeed = 10;
    public Image loadingBar;
    public Image loadingBar2;
    public AudioSource m_AudioSource;
    public AudioClip shootSFX;

    [System.NonSerialized]
    public float loadingProgress = 1f;

    public PlayerWeaponManager playerWeaponManager;
    string materialVarBlur = "Vector1_4D0B16C4";
    public MeshRenderer screenRenderer;
    Material screenMaterial;
    // Start is called before the first frame update
    void Start()
    {
        m_WeaponController = GetComponent<WeaponController>();
        m_WeaponController.ShootAction = Shoot;
        screenMaterial = screenRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (loadingProgress < 1)
        {
            loadingProgress += Time.deltaTime * loadingSpeed;
            loadingBar2.color = Color.black;
        } else
        {
            loadingBar2.color = Color.red;
        }
        loadingBar.fillAmount = loadingProgress;
        loadingBar2.fillAmount = loadingProgress;
        if (playerWeaponManager.isAiming)
        {
            screenMaterial.SetFloat(materialVarBlur, Mathf.Lerp(screenMaterial.GetFloat(materialVarBlur), 0f, Time.deltaTime * 1f));
        } else
        {
            //screenMaterial.SetFloat(materialVarBlur, )
            screenMaterial.SetFloat(materialVarBlur, Mathf.Lerp(screenMaterial.GetFloat(materialVarBlur), 1f, Time.deltaTime * 10));
        }
    }

    void Shoot()
    {
        if (loadingProgress >= 1f)
        {
            loadingProgress = 0;
            m_AudioSource.PlayOneShot(shootSFX);
            m_WeaponController.Fire();
        }
    }
}
