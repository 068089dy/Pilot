using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponControl : MonoBehaviour
{
    // Ç¹¿Ú
    public Transform weaponMuzzle;
    public LayerMask shotLayermask = -1;
    public float maxShotDistance = 500;

    public ProjectileBase projectilePrefab;
    public AudioClip shootSFX;
    public AudioSource audioSource;
    public Actor owner;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire(Vector3 curAimPoint)
    {
        if (projectilePrefab)
        {
            Vector3 shotDir = curAimPoint - weaponMuzzle.position;
            
            ProjectileBase projectileBase = Instantiate(projectilePrefab, weaponMuzzle.position, Quaternion.LookRotation(shotDir));
            //projectileBase.parentWeaponController = this;
            projectileBase.owner = owner;
            projectileBase.bulletLayerMask = shotLayermask;
            audioSource.PlayOneShot(shootSFX);
        }
    }
}
