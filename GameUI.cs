using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    internal class GameUI
    {
        private const string k_WinningResult = "VVVV";
        private const string k_UserNoNewGame = "N";
        private const string k_UserNewGame = "Y";
        private const int k_ValidGuessLength = 4;
        private const int k_PaddingAmountResultCol = 7;
        private const int k_PaddingAmountGuessCol = 6;
        private const int k_MinNumOfGuess = 4;
        private const int k_MaxNumOfGuess = 10;
        private const char k_FirstLetterOfRange = 'A';
        private const char k_LastLetterOfRange = 'H';
        private const char k_RightLetterInRightPlace = 'V';
        private const char k_RightLetterInWrongPlace = 'X';

        public static int GetNumberOfGuesses()
        {
            int numOfGuesses;
            bool isValidNumber;

            do
            {
                Console.Write("Please enter the number of guesses (4–10): ");
                string input = Console.ReadLine();

                isValidNumber = int.TryParse(input, out numOfGuesses) && checkValidGuesses(numOfGuesses);

                if (!isValidNumber)
                {
                    Console.WriteLine("Invalid input. Please enter a number between 4 and 10.");
                }

            } while (!isValidNumber);

            return numOfGuesses;
        }
        private static bool checkValidGuesses(int i_NumOfGuesses)
        {
            bool isNumberValid = true;

            if (i_NumOfGuesses < k_MinNumOfGuess || i_NumOfGuesses > k_MaxNumOfGuess)
            {
                isNumberValid = false;
                Console.WriteLine("Invalid number of guesses");
            }

            return isNumberValid;
        }

        public static string GetUserGuess()
        {
            string guess = "";
            bool isValid = false;
            string userInput = "";

            do
            {
                Console.WriteLine("Enter your guess (4 unique letters from A–H, or Q to quit): ");
                guess = Console.ReadLine()?.ToUpper().Trim();

                if (guess == GameLogic.k_UserEndsGame)
                {
                    userInput = GameLogic.k_UserEndsGame;
                    break;
                }
                isValid = isValidGuess(guess);
                if (!isValid)
                {
                    Console.WriteLine("Invalid guess. Make sure it's 4 unique letters between A and H.");
                }
            }
            while (!isValid);

            userInput = guess;
            return userInput;
        }

        private static bool isValidGuess(string i_guess)
        {
            if (i_guess == null || i_guess.Length != k_ValidGuessLength)
                return false;

            foreach (char letter in i_guess)
            {
                if (letter < k_FirstLetterOfRange || letter > k_LastLetterOfRange)
                    return false;
            }

            return i_guess.Distinct().Count() == k_ValidGuessLength;
        }

        public static void DisplayBoard(List<GameLogic.UserGuess> historyOfGuesses, int i_maxGuesses)
        {
            bool isWin = false;

            Ex02.ConsoleUtils.Screen.Clear();
            Console.WriteLine("Current board status:");
            Console.WriteLine("|Pins: |Result:|");
            Console.WriteLine("================");
            Console.WriteLine("|####  |       |");
            for (int i = 0; i < i_maxGuesses; i++)
            {
                if (i < historyOfGuesses.Count)
                {
                    printFormattedGuessAndResult(historyOfGuesses[i], ref isWin);
                }
                else
                {
                    Console.WriteLine("|      |       |");
                }
            }
            Console.WriteLine("================");
            if (!GameLogic.isOutOfTurns(historyOfGuesses.Count, i_maxGuesses))
            {
                if(!isWin)
                {
                    Console.WriteLine("Please type your next guess or 'Q' to quit");
                }
            }
        }

        private static void printFormattedGuessAndResult(GameLogic.UserGuess i_currentGuess, ref bool io_isWin)
        {
            string feedbackOnGuess = new string(k_RightLetterInRightPlace, i_currentGuess.RightLettersInRightPosCount) + new string(k_RightLetterInWrongPlace, i_currentGuess.RightLettersInWrongPosCount);
            string paddedGuess = i_currentGuess.GuessWord.PadRight(k_PaddingAmountGuessCol);     
            string paddedFeedbackOnGuess = feedbackOnGuess.PadRight(k_PaddingAmountResultCol);        

            Console.WriteLine($"|{paddedGuess}|{paddedFeedbackOnGuess}|");
            if(feedbackOnGuess == k_WinningResult)
            {
                io_isWin = true;
            }
        }

        public static void printReasonForEndOfRound(bool i_isWin, string i_secretWord, int i_currentNumOfGuesses)
        {
            if (!i_isWin)
            {
                Console.WriteLine($"You have run out of guesses, the secret word was: {i_secretWord}");
            }
            else //User won
            {
                Console.WriteLine($"Congratulations, you have won the game with: {i_currentNumOfGuesses} {(i_currentNumOfGuesses == 1 ? "guess" : "guesses")}!");
            }
        }

        public static bool AskUserForAnotherRound()
        {
            string userAnswer = "";
            bool isContinuing = false;

            Console.WriteLine("Would you like to start another game <Y/N> ?");
            userAnswer = Console.ReadLine()?.Trim().ToUpper();
            while (!isUserAnswerValid(userAnswer))
            {
                Console.WriteLine("Invalid input, try again <Y/N> : ");
                userAnswer = Console.ReadLine()?.Trim().ToUpper();
            }
            if(userAnswer == k_UserNoNewGame)
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

        private static bool isUserAnswerValid(string i_userAnswer)
        {
          return i_userAnswer == k_UserNewGame || i_userAnswer == k_UserNoNewGame;
        }

        public static void PrintUserExitMessage()
        {
            Console.WriteLine("Goodbye!");
        }
    }
}
