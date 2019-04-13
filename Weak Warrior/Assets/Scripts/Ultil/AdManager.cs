using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoSingleton<AdManager>
{

    [System.Serializable]
    public class AdmobID
    {
        public string banner = "YOUR_PLACEMENT_ID";
        public string instertitial = "YOUR_PLACEMENT_ID";
        public string rewardVideo = "YOUR_PLACEMENT_ID";
    }

    public AdmobID admobAndroid;
    public AdmobID admobIOS;
    public AdmobID admmobTest;
    private AdmobID admob;
    private bool isBannerAdmobLoaded;
    private GoogleMobileAds.Api.BannerView bannerView;
    private GoogleMobileAds.Api.InterstitialAd interstitial;
    private GoogleMobileAds.Api.RewardBasedVideoAd rewardVideo;
    private bool admobReward = false;
    public delegate void CallbackWatchVideo(bool success);
    public CallbackWatchVideo callback;

    private void LoadBannerAdmob()
    {
        this.bannerView = new GoogleMobileAds.Api.BannerView(admob.banner, GoogleMobileAds.Api.AdSize.SmartBanner, AdPosition.Top);
        if (this.bannerView != null)
        {
            bn = "Banner inited";
        }
        this.bannerView.LoadAd(new AdRequest.Builder().Build());
        this.bannerView.OnAdLoaded += (delegate (System.Object sender, EventArgs args) {
            this.alreadyShowBanner = false;
            this.isBannerAdmobLoaded = true;
            this.bannerView.Hide();
            this.ShowBanner();
            bn = "Banner Loaded";
        });
        this.bannerView.OnAdFailedToLoad += (delegate (System.Object sender, AdFailedToLoadEventArgs args) {
            bn = "Banner fail to load";
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                this.bannerView.LoadAd(new AdRequest.Builder().Build());
            }
        });
    }
    private void LoadInterstitialAdmob()
    {
        this.interstitial = new GoogleMobileAds.Api.InterstitialAd(admob.instertitial);
        if (this.interstitial != null)
        {
            vi = "interstitial inited";
        }
        this.interstitial.OnAdClosed += (delegate (System.Object sender, EventArgs args) {
            vi = "interstitial video new load";
            this.interstitial.LoadAd(new AdRequest.Builder().Build());
        });
        this.interstitial.OnAdLoaded += (delegate (System.Object sender, EventArgs args) {
            vi = "Video Loaded";
        });
        this.interstitial.OnAdFailedToLoad += (delegate (System.Object sender, AdFailedToLoadEventArgs args) {
            vi = "Video fail to load";
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                this.interstitial.LoadAd(new AdRequest.Builder().Build());
            }
        });
        this.interstitial.LoadAd(new AdRequest.Builder().Build());
    }
    private void LoadRewardVideoAdmob()
    {
        if(this.rewardVideo != null)
        {
            this.rewardVideo = null;
        }
        this.rewardVideo = RewardBasedVideoAd.Instance;
        if (this.rewardVideo != null)
        {
            rv = "rewardVideo inited";
        }
        this.rewardVideo.OnAdLoaded += (delegate (System.Object sender, EventArgs args) {
            rv = "Reward video Loaded";
        });
        this.rewardVideo.OnAdFailedToLoad += (delegate (System.Object sender, AdFailedToLoadEventArgs args) {
            rv = "Reward video fail to load";
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                this.rewardVideo.LoadAd(new AdRequest.Builder().Build(), admob.rewardVideo);
            }
        });
        this.rewardVideo.OnAdClosed += (delegate (System.Object sender, EventArgs args) {
            rv = "Reward video closed. Shame";
            //Time.timeScale = 1f;
            this.rewardVideo.LoadAd(new AdRequest.Builder().Build(), admob.rewardVideo);
            if (!admobReward)
            {
                //GameManager.Instance.EndGame();
                if (this.callback != null)
                {
                    this.callback.Invoke(false);
                }
            }
            else
            {
                admobReward = false;
            }
            if(this.rewardVideo != null)
            {
                if(this.rewardVideo.IsLoaded() == false)
                {
                    this.ReloadRewardVideo();
                }                
            }
        });
        this.rewardVideo.OnAdRewarded += (delegate (System.Object sender, Reward reward)
        {
            rv = "Reward video get rewared";
            admobReward = true;
            //Time.timeScale = 1f;
            /* GET REWARD */
            //GameManager.Instance.ContinueOnLose();
            if(this.callback != null)
            {
                this.callback.Invoke(true);
            }


        });
        this.rewardVideo.OnAdLeavingApplication += (delegate (System.Object sender, EventArgs args) {
            rv = "Reward video leaving";
            //Time.timeScale = 0f;
        });

        this.rewardVideo.LoadAd(new AdRequest.Builder().Build(), admob.rewardVideo);
    }
    private void ReloadRewardVideo()
    {
        this.rewardVideo.LoadAd(new AdRequest.Builder().Build(), admob.rewardVideo);
    }
    private void ShowBannerAdmob()
    {
        if (isBannerAdmobLoaded)
        {
            this.bannerView.Show();
        }
    }
    private void ShowInterstitialAdmob()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }
        else
        {
            if (this.interstitial != null)
            {
                this.interstitial.Destroy();
                this.LoadInterstitialAdmob();
            }
        }
    }
    private void ShowRewardVideoAdmob(CallbackWatchVideo callback)
    {
        this.callback = callback;
        if (this.rewardVideo.IsLoaded())
        {
            this.rewardVideo.Show();
        }
    }
    private void InitAdmob()
    {
        if (this.testMode)
        {
            this.admob = this.admmobTest;
        }
        else
        {


#if UNITY_ANDROID
            this.admob = this.admobAndroid;
#elif UNITY_IOS
		this.admob = this.admobIOS;
#else
        this.admob = this.admobAndroid;
#endif
        }
        //LoadBannerAdmob();
        LoadInterstitialAdmob();
        LoadRewardVideoAdmob();
    }
    private void RenewAdmob()
    {
        this.bannerView.LoadAd(new AdRequest.Builder().Build());
        this.interstitial.LoadAd(new AdRequest.Builder().Build());
        this.rewardVideo.LoadAd(new AdRequest.Builder().Build(), admob.rewardVideo);
    }
    public bool banner = true;
    public bool video = true;
    private bool alreadyShowBanner = false;
    private NetworkReachability network;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //this.InitFacebook();
        this.InitAdmob();
        //this.InitChartboost();
        network = Application.internetReachability;
    }
    private void FixedUpdate()
    {
        if (network == NetworkReachability.NotReachable)
        {
            network = Application.internetReachability;
            if (network != NetworkReachability.NotReachable)
            {
                RenewAdmob();
            }
        }
    }

    public void ShowBanner()
    {
        if ( !banner || alreadyShowBanner || Application.internetReachability == NetworkReachability.NotReachable)
        {
            return;
        }
        
        if (isBannerAdmobLoaded)
        {
            ShowBannerAdmob();
            alreadyShowBanner = true;
        }
    }
    public void ShowInterstitial()
    {
        if (!video || Application.internetReachability == NetworkReachability.NotReachable)
        {
            return;
        }
        if (interstitial.IsLoaded())
        {
            ShowInterstitialAdmob();
        }
    }
    public void ShowRewardVideo(CallbackWatchVideo callback)
    {
#if UNITY_EDITOR
        callback.Invoke(true);
#endif
        if (rewardVideo.IsLoaded())
        {
            ShowRewardVideoAdmob(callback);
        }
        else
        {
            if(rewardVideo != null)
            {
                if(this.rewardVideo.IsLoaded() == false)
                {
                    this.ReloadRewardVideo();
                }
                
            }
        }
    }
    public bool IsLoadedVideo()
    {
        return this.rewardVideo.IsLoaded();
    }
    public void RemoveBanner()
    {
        if (this.bannerView != null)
        {
            this.bannerView.Hide();
            this.bannerView.Destroy();
        }
    }

    public bool IsRewardVideoLoaded()
    {
        return rewardVideo.IsLoaded();
    }

    string bn = "";
    string vi = "";
    string rv = "";
    string bnFB = "";
    string viFB = "";
    public bool testMode = false;
    public void OnGUI()
    {
        if (!testMode)
        {
            return;
        }

        int w = Screen.width, h = Screen.height ;
        int rate = 50;
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / rate;
        
        style.normal.textColor = new Color(1.0f, 1.0f, 1f, 1.0f);
        GUI.Label(new Rect(0, h - 1.5f * h * 2 / rate, w, h * 2 / rate), rv, style);
        GUI.Label(new Rect(0, h - 2.5f * h * 2 / rate, w, h * 2 / rate), vi, style);
        GUI.Label(new Rect(0, h - 3.5f * h * 2 / rate, w, h * 2 / rate), bn, style);
        GUI.Label(new Rect(0, h - 4.5f * h * 2 / rate, w, h * 2 / rate), viFB, style);
        GUI.Label(new Rect(0, h - 5.5f * h * 2 / rate, w, h * 2 / rate), bnFB, style);
    }
}
