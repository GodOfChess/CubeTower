using UnityEngine.Advertisements;
using UnityEngine;
using System.Collections;

public class Ads : MonoBehaviour
{
    private Coroutine showAd;
    private string gameId = "3975371", type = "video";
    private bool testMode = true, needToStop;

    private static int countLoses;

    private void Start()
    {
        Advertisement.Initialize(gameId, testMode);
        countLoses += 1;

        if (countLoses % 3 == 0)
            showAd = StartCoroutine(ShowAd());
    }

    private void Update()
    {
        if (needToStop)
        {
            needToStop = false;
            StopCoroutine(showAd);
        }
    }

    IEnumerator ShowAd()
    {
        while (true)
        {
            if (Advertisement.IsReady(type))
            {
                Advertisement.Show(type);
                needToStop = true;
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
