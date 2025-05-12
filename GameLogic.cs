using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    public class GameLogic
    {
        public const string k_UserEndsGame = "Q";
        public const int k_GameWonAmount = 4;

        public struct UserGuess
        {
            public string GuessWord { get; }
            public int RightLettersInRightPosCount { get; set; } 
            public int RightLettersInWrongPosCount { get; set; }

            public UserGuess(string i_userInput)
            {
                GuessWord = i_userInput;
                RightLettersInRightPosCount = 0;
                RightLettersInWrongPosCount = 0;
            }
        }

        public static void StartGame()
        {
            SecretWord newSecretWord = new SecretWord();

            while (startRound(newSecretWord, GameUI.GetNumberOfGuesses()))
            {
                newSecretWord = new SecretWord();
            }
        }

        private static bool startRound(SecretWord i_secretWord, int i_maxNumOfGuesses)
        {
            bool isContinuing = true;
            string currentGuess = "";
            bool isWin = false;
            List<UserGuess> guessesHistory = new List<UserGuess>();
            int currentGuessesCount = 0;
            
            while(!isWin && !isOutOfTurns(currentGuessesCount, i_maxNumOfGuesses) && isContinuing)
            {
                GameUI.DisplayBoard(guessesHistory, i_maxNumOfGuesses);
                currentGuess = GameUI.GetUserGuess();
                if(currentGuess == k_UserEndsGame) //'Q'
                {
                    GameUI.PrintUserExitMessage();
                    isContinuing = false;
                }
                else
                {
                    UserGuess currentUserGuess = new UserGuess(currentGuess);

                    i_secretWord.CheckUserGuess(ref currentUserGuess);
                    guessesHistory.Add(currentUserGuess);
                    GameUI.DisplayBoard(guessesHistory, i_maxNumOfGuesses);

                    if (checkIfUserWon(currentUserGuess))
                    {
                        isWin = true;
                    }
                    else
                    {
                        currentGuessesCount++;
                    }
                }

            }
            if(isContinuing)
            {
                currentGuessesCount++;
                GameUI.printReasonForEndOfRound(isWin, i_secretWord.SecretWordValue, currentGuessesCount);
                isContinuing = GameUI.AskUserForAnotherRound();
            }

            return isContinuing;
        }

        public static bool isOutOfTurns(int i_currentNumOfGuesses, int i_maxNumOfGuesses)
        {
            return i_currentNumOfGuesses == i_maxNumOfGuesses;
        }

        private static bool checkIfUserWon(UserGuess i_currentGuess) 
        {
            return i_currentGuess.RightLettersInRightPosCount == k_GameWonAmount;
        }
        
    }
}
