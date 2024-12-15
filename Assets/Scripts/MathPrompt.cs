using TMPro;
using UnityEngine;

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
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        time = timerDuration;
        generateRandomQuestion();
    }

    // Update is called once per frame
    void Update()
    {
        updateTimer();
        closeIfTimerHasExpired();
        playTickSound();
        
    }
    
    void closeIfTimerHasExpired()
    {
        if (time <= 0)
        {
            gameObject.SetActive(false);
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
        time = timerDuration;
        answerInput.text = "";
        audioManager.PlaySFX(audioManager.correctAnswer);
        generateRandomQuestion();
    }
    
    void handleWrongAnswer()
    {
        audioManager.PlaySFX(audioManager.wrongAnswer);
        gameObject.SetActive(false);
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
        // Probably should do some input validation here
        int userAnswer = int.Parse(answerInput.text);
        if (userAnswer == answer)
            handleCorrectAnswer();
        else
            handleWrongAnswer();
    }
}
