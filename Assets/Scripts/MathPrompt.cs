using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class MathPrompt : MonoBehaviour
{
    int number1;
    int number2;
    private int prevNumber1; 
    private int prevNumber2; 
    string op;
    int answer;
    string[] operators = {"+", "-", "x", "÷"};
    
    string questionString;
    public float timerDuration;
    float time;

    public TMP_Text questionText;
    public TMP_InputField answerInput;
    public TMP_Text timerText;

    float tickInterval = 1f; // Interval for the ticking sound
    float tickTimer;         // Tracks time for the tick sound

    AudioManager audioManager;
    public InfoPanelManager infoPanelManager;
    public WaveManager waveManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        infoPanelManager = GameObject.Find("Info Panel").GetComponent<InfoPanelManager>();
        waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        time = timerDuration;
        generateRandomQuestion();
        StartCoroutine(checkEnterKeyDown());
    }

    // Update is called once per frame
    void Update()
    {
        updateTimer();
        closeIfTimerHasExpired();
        playTickSound();
    }
    
    IEnumerator checkEnterKeyDown()
    {
        while (true) 
        {
            if (Input.GetKey(KeyCode.Return))
            {
                checkAnswer();
                yield return new WaitForSeconds(0.5f);
            }
            yield return null;
        }
    }
    
    void closeIfTimerHasExpired()
    {
        if (time <= 0)
        {
            gameObject.SetActive(false);
            if (waveManager)
            {
                waveManager.startSpawning();
            }
        }
    }
    
    void updateTimer()
    {
        if (time >= 0)
        {
            time -= Time.deltaTime;
            timerText.text = $"Time Remaining: {Mathf.Ceil(time)}";
        }
    }

    float getAnswer(int num1, int num2, string op)
    {
        float result = 0;
        switch (op)
        {
            case "+":
                result = num1 + num2;
                break;
            case "-":
                result = num1 - num2;
                break;
            case "x":
                result = num1 * num2;
                break;
            case "÷":
                result = (float) num1 / (float) num2;
                break;
        }
        return result;
    }
    
    void generateRandomQuestion()
    {
        op = operators[Random.Range(0, operators.Length)];
        float possibleAnswer = -1f; 
        // ensures only non-negative answers and integers and not same question as last time
        while ((possibleAnswer < 0 || possibleAnswer % 1 != 0)
                || (number1 == prevNumber1 && number2 == prevNumber2))
        {
            number1 = Random.Range(0, 13);
            number2 = Random.Range(0, 13);
            possibleAnswer = getAnswer(number1, number2, op);
        }
        answer = (int) possibleAnswer;
        prevNumber1 = number1;
        prevNumber2 = number2;
        questionString = $"{number1} {op} {number2}";
        questionText.text = questionString; 
    }
    
    void handleCorrectAnswer()
    {
        answerInput.text = "";
        infoPanelManager.gainCoins(5);
        audioManager.PlaySFX(audioManager.correctAnswer);
        generateRandomQuestion();
    }
    
    void handleWrongAnswer()
    {
        answerInput.text = "";
        audioManager.PlaySFX(audioManager.wrongAnswer);
    }
    
    void playTickSound()
    {
        if (time > 0)
        {
            tickTimer -= Time.deltaTime;
            if (tickTimer <= 0f)
            {
                audioManager.PlaySFX(audioManager.timerTick);
                tickTimer = tickInterval;
            }
        }
    }
    
    public void checkAnswer()
    {
        int userAnswer;
        int.TryParse(answerInput.text, out userAnswer);
        if (userAnswer == answer)
        {
            handleCorrectAnswer();
        }
        else
        {
            handleWrongAnswer();
        }
        answerInput.ActivateInputField();
    }
}
