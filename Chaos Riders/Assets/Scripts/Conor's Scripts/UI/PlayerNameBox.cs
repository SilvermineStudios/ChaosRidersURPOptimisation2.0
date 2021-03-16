using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameBox : MonoBehaviour
{
    [SerializeField] private GameObject none, braker, shredder, standardGun, goldenGun;

    // Start is called before the first frame update
    void Awake()
    {
        NoneSelected();
    }

    
    public void NoneSelected()
    {
        none.SetActive(true);
        braker.SetActive(false);
        shredder.SetActive(false);
        standardGun.SetActive(false);
        goldenGun.SetActive(false);
    }

    #region Cars
    public void BrakerSelected()
    {
        none.SetActive(false);
        braker.SetActive(true);
        shredder.SetActive(false);
        standardGun.SetActive(false);
        goldenGun.SetActive(false);
    }

    public void ShredderSelected()
    {
        none.SetActive(false);
        braker.SetActive(false);
        shredder.SetActive(true);
        standardGun.SetActive(false);
        goldenGun.SetActive(false);
    }
    #endregion

    #region Guns
    public void StandardGunSelected()
    {
        none.SetActive(false);
        braker.SetActive(false);
        shredder.SetActive(false);
        standardGun.SetActive(true);
        goldenGun.SetActive(false);
    }

    public void GoldenGunSelected()
    {
        none.SetActive(false);
        braker.SetActive(false);
        shredder.SetActive(false);
        standardGun.SetActive(false);
        goldenGun.SetActive(true);
    }
    #endregion
}
