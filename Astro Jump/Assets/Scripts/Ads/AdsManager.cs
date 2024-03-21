using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour
{
    private void Awake()
    {
        MobileAds.Initialize(initStatus => {});
    }
}
