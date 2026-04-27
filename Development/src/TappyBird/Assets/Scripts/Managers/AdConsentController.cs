using GoogleMobileAds.Ump.Api;
using System;

public static class AdConsentController
{
    public static bool CanRequestAds => ConsentInformation.CanRequestAds();

    public static void ConsentData(Action<string> onComplete)
    {
        var requestParameters = new ConsentRequestParameters();
        ConsentInformation.Update(requestParameters, (FormError updateError) =>
        {

            if (updateError != null)
            {
                onComplete(updateError.Message);
                return;
            }
            if (CanRequestAds)
            {
                onComplete(null);
                return;
            }

            ConsentForm.LoadAndShowConsentFormIfRequired((FormError formLoadError) =>
             {
                 if (formLoadError != null)
                 {
                     Log.Error($"Ad consent form failed to load or show: {formLoadError.Message}");
                     onComplete?.Invoke(formLoadError.Message);
                 }
                 else
                 {
                     Log.Info("Ad consent form loaded and shown successfully.");
                     onComplete?.Invoke(null);
                 }
             });

        });
    }

    /// <summary>
    /// For testing purposes development only.
    /// </summary>
    public static void ResetConsent()
    {
        ConsentInformation.Reset();
    }
}
