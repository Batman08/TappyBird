using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get; private set; }


    public event Action OnInitialized;
    public event Action<Reward> OnShowRewardedAdCompleted;

#if UNITY_EDITOR
    private readonly string TappyBird_KeepPlaying_Reward_Unit_Id = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_ANDROID
    private readonly string TappyBird_KeepPlaying_Reward_Unit_Id = "ca-app-pub-3940256099942544/5224354917";
#endif
    private RewardedAd _rewardedAd;
    private Reward _reward;
    private bool _isRewarded = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeAdsWithConsent();
    }


    #region InitMobileAds

    private void InitializeAdsWithConsent()
    {
        if (AdConsentController.CanRequestAds)
        {
            InitMobileAds();
        }
        else
        {
            InitMobileAdsConsent();
        }
    }

    private void InitMobileAdsConsent()
    {
        AdConsentController.ConsentData((string error) =>
        {
            if (string.IsNullOrEmpty(error))
            {
                Log.Info("Ad consent successful.");
            }
            else
            {
                Log.Error($"Ad consent error: {error}");
            }

            // Initialize Mobile Ads SDK after we have the consent status.
            if (AdConsentController.CanRequestAds)
            {
                MainThreadDispatcher.Enqueue(() => InitMobileAds());
            }
        });
    }

    private void InitMobileAds()
    {
        // Initialize Google Mobile Ads Unity Plugin.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            LoadRewardedAd();
            OnInitialized?.Invoke();
            Log.Info("Mobile Ads SDK initialized.");
        });
    }

    #endregion


    #region LoadRewardedAd

    private void LoadRewardedAd()
    {
        // If we already have a rewarded ad, destroy it and create a new one.
        DestroyAd();

        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // Send the request to load the ad.
        RewardedAd.Load(TappyBird_KeepPlaying_Reward_Unit_Id, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            Log.Info("Rewarded ad load callback received.");

            bool adLoadedSuccessfully = ad != null;
            if (adLoadedSuccessfully)
            {
                _rewardedAd = ad;
                EventListeners();

                Log.Info("Rewarded ad loaded successfully.");
            }
            else
            {
                Log.Error($"Rewarded ad failed to load with error: {error.GetMessage()}");
            }
        });
    }

    #endregion


    #region ShowRewardedAd

    public void ShowRewardedAd()
    {
        // Reset flag
        _isRewarded = false;

        Log.Info("Attempting to show rewarded ad.");
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            Log.Info("Rewarded ad is ready to be shown. Showing ad now.");
            _rewardedAd.Show((Reward reward) =>
            {
                Log.Info($"Reward received: {reward.Type}, amount: {reward.Amount}");

                // The ad was showen and the user earned a reward.
                _isRewarded = true;
                _reward = reward;
            });
        }
        else
        {
            Log.Warning("Rewarded ad is not ready yet. Please try again later.");
        }
    }

    #endregion


    #region EventListeners

    private void EventListeners()
    {
        // Raised when the ad closed full screen content.
        _rewardedAd.OnAdFullScreenContentClosed += OnAdFullScreenContentClosed_ReloadAd;
    }

    private void OnAdFullScreenContentClosed_ReloadAd()
    {
        Log.Info("Ad full screen content closed.");

        // Route the event through the Main Thread Dispatcher
        MainThreadDispatcher.Enqueue(() =>
        {
            // Everything in this block is guaranteed to run on the main thread
            if (_isRewarded && _reward != null) OnShowRewardedAdCompleted?.Invoke(_reward);
        });

        // Reload the ad so that we can show another as soon as possible.
        LoadRewardedAd();
    }

    #endregion


    #region DestroyAd

    private void DestroyAd()
    {
        _rewardedAd?.Destroy();
    }

    #endregion
}