using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class ExampleShooting : NetworkBehaviour
{
    [SerializeField] GameObject cameraRecoilGO;
    [SerializeField] TextMeshProUGUI ammoText;

    [Header("Shooting")]
    [SerializeField] int maxAmmo = 20;
    [SerializeField] [SyncVar(hook = nameof(AmmoChanged))] int currentAmmo = 20;
    [SerializeField] int maxToReload = 80;
    [SerializeField] [SyncVar(hook = nameof(MagazineAmmoChanged))] int currentToReload = 80;
    [SerializeField] float reloadTime = 1f;
    [SerializeField] float shootCooldown = 0.1f;

    [SerializeField] [SyncVar(hook = nameof(ShootingStateChanged))] bool isShooting = false;

    [Header("Recoil")]
    private Vector3 currentRotation;
    private Vector3 targetRotation;
    [SerializeField] private float recoilX = -2;
    [SerializeField] private float recoilY = 2;
    [SerializeField] private float recoilZ = 0.35f;
    [SerializeField] private float smooth = 6;
    [SerializeField] private float returnSpeed = 2;

    void Start()
    {
        if (!isLocalPlayer) return;
        ammoText = GameObject.Find("HUDAmmoText").GetComponent<TextMeshProUGUI>();
        ammoText.text = currentAmmo + " / " + currentToReload;
    }

    void Update()
    {
        if (!isLocalPlayer) return;
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, smooth * Time.fixedDeltaTime);
        cameraRecoilGO.transform.localRotation = Quaternion.Euler(currentRotation);
        
        if (Input.GetKey(KeyCode.Mouse0))
        {
            CmdFire();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            CmdReload();
        }
    }
    
    [Command]
    void CmdFire()
    {
        if (isShooting) return;
        if (currentAmmo > 0)
        {
            Recoil();
            currentAmmo--;
            isShooting = true;
            StartCoroutine(RunShootingCooldown());
            RaycastHit hit;
            if (Physics.Raycast(cameraRecoilGO.transform.position, cameraRecoilGO.transform.forward, out hit, 100))
            {
                if (hit.transform.tag == "Player")
                {
                    hit.transform.GetComponent<PlayerStats>().TakeDamage(10);
                }
                if (hit.transform.tag == "Terrain")
                {
                    RpcSpawnBulletHole(hit.point + hit.normal / 100, Quaternion.LookRotation(-hit.normal));
                }
            }
        }
    }
    
    [Command]
    void CmdReload()
    {
        if (currentToReload > 0 && currentAmmo < maxAmmo)
        {
            StartCoroutine(RunningReload());
        }
    }
    
    [Server]
    IEnumerator RunningReload()
    {
        yield return new WaitForSeconds(reloadTime);
        if (currentToReload >= maxAmmo - currentAmmo)
        {
            currentToReload -= maxAmmo - currentAmmo;
            currentAmmo = maxAmmo;
        }
        else
        {
            currentAmmo += currentToReload;
            currentToReload = 0;
        }
    }
    [Server]
    IEnumerator RunShootingCooldown()
    {
        yield return new WaitForSeconds(shootCooldown);
        isShooting = false;
    }

    [ClientRpc]
    void RpcSpawnBulletHole(Vector3 position, Quaternion rotation)
    {
        Object hole = Instantiate(Resources.Load("_Prefabs/BulletHole"), position, rotation, GameObject.Find("BulletsHolder").transform);
        hole.name = "BulletHole";
    }

    [TargetRpc]
    void Recoil()
    {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }

    //Syncing Vars

    void AmmoChanged(int _, int newAmmo)
    {
        OnAmmoChanged(newAmmo);
    }

    void OnAmmoChanged(int newAmmo)
    {
        currentAmmo = newAmmo;
        if (!isLocalPlayer) return;
        ammoText.text = currentAmmo + " / " + currentToReload;
    }

    void MagazineAmmoChanged(int _, int newMagazineAmmo)
    {
        OnMagazineAmmoChanged(newMagazineAmmo);
    }

    void OnMagazineAmmoChanged(int newMagazineAmmo)
    {
        currentToReload = newMagazineAmmo;
        if (!isLocalPlayer) return;
        ammoText.text = currentAmmo + " / " + currentToReload;
    }

    void ShootingStateChanged(bool _, bool newState)
    {
        OnShootingStateChanged(newState);
    }

    void OnShootingStateChanged(bool newState)
    {
        isShooting = newState;
    }
}
