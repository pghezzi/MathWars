using TMPro;
using UnityEngine;

public class MathPrompt : MonoBehaviour
{
    int number1;
    int number2;
    string op;
    int answer;
    string[] operators = {"+", "-", "x", "÷"};
    
    string questionString;
    public float timerDuration;
    float time;

    public TMP_Text questionText;
    public TMP_InputField answerInput;
    public TMP_Text timerText;
    
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

    int getAnswer(int num1, int num2, string op)
    {
        int result = 0;
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
                // Real division will lead to floats but integer division is also weird
                result = num1 / num2;
                break;
        }
        return result;
    }

    void generateRandomQuestion()
    {
        number1 = Random.Range(0, 13);
        number2 = Random.Range(0, 13);
        op = operators[Random.Range(0,operators.Length)];
        answer = getAnswer(number1, number2, op);
        questionString = $"{number1} {op} {number2}";
        questionText.text = questionString; 
    }
    
    void handleCorrectAnswer()
    {
        time = timerDuration;
        answerInput.text = "";
        generateRandomQuestion();
    }
    
    void handleWrongAnswer()
    {
        gameObject.SetActive(false);
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
