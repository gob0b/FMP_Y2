using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class Rihanna : MonoBehaviour
{
    [Header("Video")]
    public GameObject videoPanel;
    public VideoPlayer videoPlayer;

    [Header("Main Panel")]
    public GameObject mainPanel;
    public Button[] mainButtons;
    public Image[] arrowIndicators;

    [Header("Sub Panels")]
    public GameObject subPanel1;
    public GameObject subPanel2;

    [Header("SubPanel1 Pages")]
    public GameObject[] subPanel1Pages;
    public RectTransform subPanel1Container;

    [Header("SubPanel1 Page1 Buttons")]
    public Button[] subPanel1Page1Buttons;
    public Button gameOneButton;
    public AudioSource gameOneAudioSource;
    public AudioClip gameOneClip; // ADDED

    [Header("SubPanel1 Page2 Button")]
    public Button gameTwoButton;
    public AudioSource gameTwoAudioSource;
    public AudioClip gameTwoClip; // ADDED

    [Header("Audio")]
    public AudioSource navigationAudioSource;
    public AudioClip navigationSound;

    private bool isGameOneAudioPlaying = false;
    private bool isGameTwoAudioPlaying = false;

    [Header("Shake Settings")]
    public float shakeIntensity = 5f;
    public float shakeSpeed = 10f;

    [Header("Page Transition Settings")]
    public float shrinkScale = 0.8f;
    public float scaleDuration = 0.15f;
    public float slideDuration = 0.4f;
    public float pageSpacing = 800f;

    private int selectedMainIndex = 0;
    private int selectedSub1Index = 0;
    private int subPanel1PageIndex = 0;

    private bool isMainPanelActive = false;
    private bool inSubPanel1 = false;
    private bool inSubPanel2 = false;
    private bool isTransitioning = false;

    private bool firstLeftPressOnPage1 = true;

    void Start()
    {
        videoPanel.SetActive(true);
        mainPanel.SetActive(false);
        subPanel1.SetActive(false);
        subPanel2.SetActive(false);
        Cursor.visible = false;

        videoPlayer.loopPointReached += OnVideoFinished;

        subPanel1Container.anchoredPosition = Vector2.zero;
        subPanel1Pages[0].transform.localScale = Vector3.one;
        subPanel1Pages[1].transform.localScale = Vector3.one * shrinkScale;

        HighlightMainButton();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        videoPanel.SetActive(false);
        mainPanel.SetActive(true);
        isMainPanelActive = true;
        HighlightMainButton();
    }

    void Update()
    {
        if (isMainPanelActive)
            HandleMainPanel();
        else if (inSubPanel1)
            HandleSubPanel1();
        else if (inSubPanel2)
            HandleSubPanel2();
    }

    void HandleMainPanel()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedMainIndex = (selectedMainIndex - 1 + mainButtons.Length) % mainButtons.Length;
            PlayNavigationSound();
            HighlightMainButton();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedMainIndex = (selectedMainIndex + 1) % mainButtons.Length;
            PlayNavigationSound();
            HighlightMainButton();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (selectedMainIndex == 0)
                EnterSubPanel1();
            else if (selectedMainIndex == 1)
                EnterSubPanel2();
            else if (selectedMainIndex == 2)
                Debug.Log("Button 3 pressed.");
        }
    }

    void EnterSubPanel1()
    {
        mainPanel.SetActive(false);
        subPanel1.SetActive(true);
        isMainPanelActive = false;
        inSubPanel1 = true;
        inSubPanel2 = false;

        subPanel1Container.anchoredPosition = Vector2.zero;
        subPanel1Pages[0].transform.localScale = Vector3.one;
        subPanel1Pages[1].transform.localScale = Vector3.one * shrinkScale;

        subPanel1PageIndex = 0;

        selectedSub1Index = 0;
        HighlightSubPanel1Buttons();
        HighlightGameTwoButton(false);
        firstLeftPressOnPage1 = true;
    }

    void EnterSubPanel2()
    {
        mainPanel.SetActive(false);
        subPanel2.SetActive(true);
        isMainPanelActive = false;
        inSubPanel1 = false;
        inSubPanel2 = true;
    }

    void HandleSubPanel1()
    {
        subPanel1Pages[0].SetActive(true);
        subPanel1Pages[1].SetActive(true);

        if (isTransitioning) return;

        if (subPanel1PageIndex == 0)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                selectedSub1Index = (selectedSub1Index - 1 + subPanel1Page1Buttons.Length) % subPanel1Page1Buttons.Length;
                PlayNavigationSound();
                HighlightSubPanel1Buttons();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                selectedSub1Index = (selectedSub1Index + 1) % subPanel1Page1Buttons.Length;
                PlayNavigationSound();
                HighlightSubPanel1Buttons();
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                if (selectedSub1Index == 0)
                    ReturnToMainPanel();
                else if (selectedSub1Index == 1)
                    StartCoroutine(ShakeButton(gameOneButton.transform, gameOneAudioSource));
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PlayNavigationSound();
                if (firstLeftPressOnPage1)
                {
                    firstLeftPressOnPage1 = false;
                    StartCoroutine(SwitchPage(1));
                }
            }
        }
        else if (subPanel1PageIndex == 1)
        {
            HighlightGameTwoButton(true);
            HighlightSubPanel1Buttons(false);

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                PlayNavigationSound();
                StartCoroutine(SwitchPage(0));
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                gameTwoButton.onClick.Invoke();
                PlayNavigationSound();
                if (gameTwoAudioSource != null && gameTwoClip != null)
                    gameTwoAudioSource.PlayOneShot(gameTwoClip);
            }
        }
    }

    void HandleSubPanel2() { }

    void HighlightMainButton()
    {
        for (int i = 0; i < mainButtons.Length; i++)
        {
            ColorBlock colors = mainButtons[i].colors;
            colors.normalColor = (i == selectedMainIndex) ? new Color(0.7f, 0.7f, 0.7f) : Color.white;
            mainButtons[i].colors = colors;

            if (arrowIndicators != null && i < arrowIndicators.Length)
                arrowIndicators[i].enabled = (i == selectedMainIndex);
        }
    }

    void HighlightSubPanel1Buttons(bool highlight = true)
    {
        for (int i = 0; i < subPanel1Page1Buttons.Length; i++)
        {
            ColorBlock colors = subPanel1Page1Buttons[i].colors;
            bool isSelected = highlight && (i == selectedSub1Index);
            colors.normalColor = isSelected ? new Color(0.7f, 0.7f, 0.7f) : Color.white;
            subPanel1Page1Buttons[i].colors = colors;
        }

        if (highlight && selectedSub1Index == 1)
        {
            if (gameOneAudioSource != null && !gameOneAudioSource.isPlaying && gameOneClip != null)
            {
                gameOneAudioSource.clip = gameOneClip;
                gameOneAudioSource.Play();
                isGameOneAudioPlaying = true;
            }
        }
        else
        {
            if (gameOneAudioSource != null && gameOneAudioSource.isPlaying)
            {
                gameOneAudioSource.Pause();
                isGameOneAudioPlaying = false;
            }
        }
    }

    void HighlightGameTwoButton(bool highlight)
    {
        if (gameTwoButton == null) return;

        ColorBlock colors = gameTwoButton.colors;
        colors.normalColor = highlight ? new Color(0.7f, 0.7f, 0.7f) : Color.white;
        gameTwoButton.colors = colors;

        if (highlight)
        {
            if (gameTwoAudioSource != null && !gameTwoAudioSource.isPlaying && gameTwoClip != null)
            {
                gameTwoAudioSource.clip = gameTwoClip;
                gameTwoAudioSource.Play();
                isGameTwoAudioPlaying = true;
            }
        }
        else
        {
            if (gameTwoAudioSource != null && gameTwoAudioSource.isPlaying)
            {
                gameTwoAudioSource.Pause();
                isGameTwoAudioPlaying = false;
            }
        }
    }

    void ReturnToMainPanel()
    {
        mainPanel.SetActive(true);
        subPanel1.SetActive(false);
        subPanel2.SetActive(false);

        isMainPanelActive = true;
        inSubPanel1 = false;
        inSubPanel2 = false;
        selectedMainIndex = 0;
        HighlightMainButton();

        Cursor.visible = false;
    }

    IEnumerator ShakeButton(Transform buttonTransform, AudioSource audioSource)
    {
        Vector3 originalPos = buttonTransform.localPosition;
        float elapsed = 0f;

        if (audioSource != null && gameOneClip != null)
        {
            audioSource.PlayOneShot(gameOneClip);
        }

        while (elapsed < 0.3f)
        {
            float x = Random.Range(-shakeIntensity, shakeIntensity);
            float y = Random.Range(-shakeIntensity, shakeIntensity);
            buttonTransform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime * shakeSpeed;
            yield return null;
        }

        buttonTransform.localPosition = originalPos;
    }

    IEnumerator SwitchPage(int targetPageIndex)
    {
        isTransitioning = true;

        GameObject currentPage = subPanel1Pages[subPanel1PageIndex];
        GameObject targetPage = subPanel1Pages[targetPageIndex];

        Vector3 startScale = currentPage.transform.localScale;
        Vector3 endScale = Vector3.one * shrinkScale;

        float timer = 0f;
        while (timer < scaleDuration)
        {
            timer += Time.deltaTime;
            currentPage.transform.localScale = Vector3.Lerp(startScale, endScale, timer / scaleDuration);
            yield return null;
        }
        currentPage.transform.localScale = endScale;

        Vector2 startPos = subPanel1Container.anchoredPosition;
        Vector2 endPos = new Vector2(-pageSpacing * targetPageIndex, 0);
        timer = 0f;
        while (timer < slideDuration)
        {
            timer += Time.deltaTime;
            subPanel1Container.anchoredPosition = Vector2.Lerp(startPos, endPos, timer / slideDuration);
            yield return null;
        }
        subPanel1Container.anchoredPosition = endPos;

        targetPage.transform.localScale = Vector3.one * shrinkScale;

        startScale = targetPage.transform.localScale;
        endScale = Vector3.one;
        timer = 0f;
        while (timer < scaleDuration)
        {
            timer += Time.deltaTime;
            targetPage.transform.localScale = Vector3.Lerp(startScale, endScale, timer / scaleDuration);
            yield return null;
        }
        targetPage.transform.localScale = endScale;

        subPanel1PageIndex = targetPageIndex;

        if (subPanel1PageIndex == 0)
            firstLeftPressOnPage1 = true;

        isTransitioning = false;

        if (subPanel1PageIndex == 0)
        {
            HighlightSubPanel1Buttons(true);
            HighlightGameTwoButton(false);
            selectedSub1Index = 0;
            HighlightSubPanel1Buttons();
        }
        else
        {
            HighlightSubPanel1Buttons(false);
            HighlightGameTwoButton(true);
        }
    }

    void PlayNavigationSound()
    {
        if (navigationAudioSource != null && navigationSound != null)
        {
            navigationAudioSource.PlayOneShot(navigationSound);
        }
    }
}
