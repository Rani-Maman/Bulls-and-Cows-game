using System;
using System.Collections.Generic;

namespace Ex02
{
    public class GameUI
    {
        private const string k_WinningResult = "VVVV";
        private const string k_UserNoNewGame = "N";
        private const string k_UserNewGame = "Y";
        private const int k_ValidGuessLength = 4;
        private const int k_PaddingAmountResultCol = 7;
        private const int k_PaddingAmountGuessCol = 6;
        private const char k_RightLetterInRightPlace = 'V';
        private const char k_RightLetterInWrongPlace = 'X';
        private const string k_UserEndsGame = "Q";

        public void Run()
        {
            bool playAgain = true;

            while (playAgain)
            {
                int numGuesses = GetNumberOfGuesses();
                GameLogic gameLogic = new GameLogic(numGuesses);

                while (!gameLogic.IsGameOver)
                {
                    DisplayBoard(gameLogic.GuessesHistory, numGuesses);

                    string userGuess = GetUserGuess();
                    if (userGuess == k_UserEndsGame)
                    {
                        PrintUserExitMessage();
                        return;
                    }

                    if (!gameLogic.IsValidGuess(userGuess))
                    {
                        Console.WriteLine("Invalid guess. Make sure it's 4 unique letters between A and H.");
                        continue;
                    }

                    gameLogic.AddGuess(userGuess);
                }

                DisplayBoard(gameLogic.GuessesHistory, numGuesses);
                printReasonForEndOfRound(gameLogic.IsWin, gameLogic.SecretWordValue, gameLogic.NumGuessesMade);

                playAgain = AskUserForAnotherRound();
            }
        }

        public int GetNumberOfGuesses()
        {
            int numOfGuesses = 0;
            bool isValidNumber = false;

            do
            {
                Console.Write("Please enter the number of guesses (4–10): ");
                string input = Console.ReadLine();
                int parsed = 0;

                isValidNumber = int.TryParse(input, out parsed) && (parsed >= 4 && parsed <= 10);
                if (isValidNumber)
                {
                    numOfGuesses = parsed;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number between 4 and 10.");
                }
            } while (!isValidNumber);

            return numOfGuesses;
        }

        public string GetUserGuess()
        {
            string guess = string.Empty;
            bool isValid = false;

            do
            {
                Console.WriteLine("Enter your guess (4 unique letters from A–H, or Q to quit): ");
                guess = Console.ReadLine();
                if (guess != null)
                {
                    guess = guess.Trim();
                }
                else
                {
                    guess = string.Empty;
                }
                if (guess == k_UserEndsGame)
                {
                    isValid = true;
                }
                else
                {
                    isValid = guess.Length > 0;
                    if (!isValid)
                    {
                        Console.WriteLine("Guess cannot be empty.");
                    }
                }
            }
            while (!isValid);

            return guess;
        }


        public void DisplayBoard(List<GameLogic.UserGuess> i_HistoryOfGuesses, int i_MaxGuesses)
        {
            bool isWin = false;

            Console.Clear();
            Console.WriteLine("Current board status:");
            Console.WriteLine("|Pins: |Result:|");
            Console.WriteLine("================");
            Console.WriteLine("|####  |       |");
            for (int i = 0; i < i_MaxGuesses; i++)
            {
                if (i < i_HistoryOfGuesses.Count)
                {
                    printFormattedGuessAndResult(i_HistoryOfGuesses[i], ref isWin);
                }
                else
                {
                    Console.WriteLine("|      |       |");
                }
            }
            Console.WriteLine("================");
            if (i_HistoryOfGuesses.Count < i_MaxGuesses && !isWin)
            {
                Console.WriteLine("Please type your next guess or 'Q' to quit");
            }
        }

        private static void printFormattedGuessAndResult(GameLogic.UserGuess i_CurrentGuess, ref bool io_IsWin)
        {
            string feedbackOnGuess = new string(k_RightLetterInRightPlace, i_CurrentGuess.RightLettersInRightPosCount)
                                   + new string(k_RightLetterInWrongPlace, i_CurrentGuess.RightLettersInWrongPosCount);
            string paddedGuess = i_CurrentGuess.GuessWord.PadRight(k_PaddingAmountGuessCol);
            string paddedFeedbackOnGuess = feedbackOnGuess.PadRight(k_PaddingAmountResultCol);

            Console.WriteLine("|" + paddedGuess + "|" + paddedFeedbackOnGuess + "|");
            if (feedbackOnGuess == k_WinningResult)
            {
                io_IsWin = true;
            }
        }

        public void printReasonForEndOfRound(bool i_IsWin, string i_SecretWord, int i_CurrentNumOfGuesses)
        {
            if (!i_IsWin)
            {
                Console.WriteLine("You have run out of guesses, the secret word was: " + i_SecretWord);
            }
            else
            {
                Console.WriteLine("Congratulations, you have won the game with: " + i_CurrentNumOfGuesses
                    + (i_CurrentNumOfGuesses == 1 ? " guess!" : " guesses!"));
            }
        }

        public bool AskUserForAnotherRound()
        {
            string userAnswer = string.Empty; 
            bool isContinuing = false;

            Console.WriteLine("Would you like to start another game <Y/N> ?");
            userAnswer = Console.ReadLine();
            if (userAnswer != null)
            {
                userAnswer = userAnswer.Trim().ToUpper();
            }
            else
            {
                userAnswer = string.Empty;
            }

            while (!isUserAnswerValid(userAnswer))
            {
                Console.WriteLine("Invalid input, try again <Y/N> : ");
                userAnswer = Console.ReadLine();
                if (userAnswer != null)
                {
                    userAnswer = userAnswer.Trim().ToUpper();
                }
                else
                {
                    userAnswer = string.Empty;
                }
            }
            if (userAnswer == k_UserNoNewGame)
            {
                isContinuing = false;
                PrintUserExitMessage();
            }
            else
            {
                isContinuing = true;
            }

            return isContinuing;
        }

        private static bool isUserAnswerValid(string i_UserAnswer)
        {
            return i_UserAnswer == k_UserNewGame || i_UserAnswer == k_UserNoNewGame;
        }

        public static void PrintUserExitMessage()
        {
            Console.WriteLine("Goodbye!");
        }
    }
}
