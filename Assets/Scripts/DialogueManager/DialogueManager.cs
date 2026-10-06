using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public CharacterDatabase characterDB;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public Image portraitImage;
    public TextMeshProUGUI tapToContinueText;

    public bool isDialogueActive;
    private bool isTyping;
    private int currentLineIndex;

    private GameDialogueData currentData;
    private PlayerMovement playerMovement;

    private void Start()
    {
        Instance = this;
    }

    public void StartDialogue(GameDialogueData data)
    {
        currentData = data;
        currentLineIndex = 0;
        isDialogueActive = true;
        dialoguePanel.SetActive(true);

        UIManager.Instance.HideGameplayUI();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();

            if (playerMovement != null)
            {
                playerMovement.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                playerMovement.GetComponent<Animator>().SetBool("isWalking", false);

                playerMovement.enabled = false;
            }
        }

        StartCoroutine(TypeLine());
    }

    public void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();

            AudioManager.Instance.StopTypingSFX();

            string parsedLine = ParseLine(currentData.dialogueLines[currentLineIndex]);
            dialogueText.text = parsedLine;

            isTyping = false;
            UpdateTapIndicator();
        }
        else if (++currentLineIndex < currentData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";
        tapToContinueText.gameObject.SetActive(false);

        string parsedLine = ParseLine(currentData.dialogueLines[currentLineIndex]);

        AudioManager.Instance.PlayTypingSFX("Typing");

        foreach (char letter in parsedLine.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(currentData.typingSpeed);
        }

        AudioManager.Instance.StopTypingSFX();
        isTyping = false;
        UpdateTapIndicator();

        if (currentData.autoProgressLines.Length > currentLineIndex &&
            currentData.autoProgressLines[currentLineIndex])
        {
            yield return new WaitForSeconds(currentData.autoProgressDelay);

            NextLine();
        }
    }

    private string ParseLine(string rawLine)
    {
        int colonIndex = rawLine.IndexOf(':');

        if (colonIndex != -1)
        {
            string speakerName = rawLine.Substring(0, colonIndex).Trim();
            string spokenText = rawLine.Substring(colonIndex + 1).Trim();

            CharacterProfile profile = characterDB.GetProfile(speakerName);

            nameText.text = profile.characterName != "" ? profile.characterName : speakerName;

            portraitImage.sprite = profile.portrait;
            portraitImage.gameObject.SetActive(true);

            return spokenText;
        }

        nameText.text = "";
        portraitImage.gameObject.SetActive(false);
        return rawLine;
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        if(GameManager.Instance.isTutorialComplete) UIManager.Instance.ShowGameplayUI();
        else UIManager.Instance.HideGameplayUI();
        if (playerMovement != null) playerMovement.enabled = true;
    }

    private void UpdateTapIndicator()
    {
        tapToContinueText.gameObject.SetActive(true);
        tapToContinueText.text = (currentLineIndex >= currentData.dialogueLines.Length - 1) ? "Tap to close..." : "Tap to continue...";
    }
}