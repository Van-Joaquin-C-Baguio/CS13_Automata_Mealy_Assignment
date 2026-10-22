
using System;
using System.Collections.Generic;

public class MealyCommentValidator
{
    // States
    private enum State
    {
        q0, // Start
        q1, // First slash detected
        q2, // Inside comment
        q3, // Asterisk detected inside comment
        qA, // Comment successfully closed
        qD  // Dead state
    }

    // Mealy transition: current state + input -> next state + output
    private static (State NextState, char Output) Transition(
        State state, char input)
    {
        // Input alphabet: '/', '*', 'a' (any other character)
        char symbol = (input == '/' || input == '*') ? input : 'a';

        switch (state)
        {
            case State.q0:
                if (symbol == '/')
                    return (State.q1, '0');

                return (State.qD, '0');

            case State.q1:
                if (symbol == '*')
                    return (State.q2, '0');

                return (State.qD, '0');

            case State.q2:
                if (symbol == '*')
                    return (State.q3, '0');

                return (State.q2, '0');

            case State.q3:
                if (symbol == '/')
                    return (State.qA, '1');

                if (symbol == '*')
                    return (State.q3, '0');

                return (State.q2, '0');

            case State.qA:
                return (State.qD, '0');

            case State.qD:
                return (State.qD, '0');

            default:
                return (State.qD, '0');
        }
    }

    public static (string Outputs, bool IsAccepted) ProcessString(
        string input)
    {
        if (string.IsNullOrEmpty(input))
            return ("", false);

        State currentState = State.q0;
        List<char> outputs = new List<char>();

        foreach (char symbol in input)
        {
            // The transition produces BOTH the next state and output.
            var result = Transition(currentState, symbol);

            outputs.Add(result.Output);
            currentState = result.NextState;
        }

        string outputSequence = new string(outputs.ToArray());

        // Accept only if the final transition outputs 1.
        bool accepted =
            outputs.Count > 0 &&
            outputs[outputs.Count - 1] == '1';

        return (outputSequence, accepted);
    }



    public static void Main()
    {
        while (true)
        {
            Console.Write("\nEnter input string (or type 'exit' to quit): ");
            string input = Console.ReadLine() ?? "";

            if (input.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            var result = ProcessString(input);

            Console.WriteLine($"Input:  {input}");
            Console.WriteLine($"Output: {result.Outputs}");
        }
    }


}
