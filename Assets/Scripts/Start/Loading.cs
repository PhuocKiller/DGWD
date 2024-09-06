using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    [SerializeField]
    GameObject loadingObj;
    public void ShowLoading()
    {
        loadingObj.gameObject.SetActive(true);  
    }
    public void HideLoading()
    {
        loadingObj.gameObject.SetActive(false);
    }
}
