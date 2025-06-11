using System;
using System.Collections.Generic;

namespace Ex02
{
    public class GameLogic
    {
        private const int k_GameWonAmount = 4;
        private const int k_MinNumOfGuess = 4;
        private const int k_MaxNumOfGuess = 10;
        private const int k_ValidGuessLength = 4;
        private const char k_FirstLetterOfRange = 'A';
        private const char k_LastLetterOfRange = 'H';

        public struct UserGuess
        {
            private string m_GuessWord;
            private int m_RightLettersInRightPosCount;
            private int m_RightLettersInWrongPosCount;

            public UserGuess(string i_UserInput)
            {
                m_GuessWord = i_UserInput;
                m_RightLettersInRightPosCount = 0;
                m_RightLettersInWrongPosCount = 0;
            }

            public string GuessWord
            {
                get { return m_GuessWord; }
            }
            public int RightLettersInRightPosCount
            {
                get { return m_RightLettersInRightPosCount; }
                set { m_RightLettersInRightPosCount = value; }
            }
            public int RightLettersInWrongPosCount
            {
                get { return m_RightLettersInWrongPosCount; }
                set { m_RightLettersInWrongPosCount = value; }
            }
        }
        private SecretWord m_SecretWord;
        private List<UserGuess> m_GuessesHistory;
        private int m_MaxNumOfGuesses;

        public GameLogic(int i_MaxNumOfGuesses)
        {
            m_SecretWord = new SecretWord();
            m_GuessesHistory = new List<UserGuess>();
            m_MaxNumOfGuesses = i_MaxNumOfGuesses;
        }

        public List<UserGuess> GuessesHistory
        {
            get { return m_GuessesHistory; }
        }
        public string SecretWordValue
        {
            get { return m_SecretWord.SecretWordValue; }
        }
        public int MaxNumOfGuesses
        {
            get { return m_MaxNumOfGuesses; }
        }
        public int NumGuessesMade
        {
            get { return m_GuessesHistory.Count; }
        }
        public bool IsWin
        {
            get
            {
                bool isWin = false;
                foreach (UserGuess guess in m_GuessesHistory)
                {
                    if (guess.RightLettersInRightPosCount == k_GameWonAmount)
                    {
                        isWin = true;
                        break;
                    }
                }

                return isWin;
            }

        }
        public bool IsOutOfTurns
        {
            get { return m_GuessesHistory.Count >= m_MaxNumOfGuesses; }
        }
        public bool IsGameOver
        {
            get { return IsWin || IsOutOfTurns; }
        }

        public bool CheckValidGuesses(int i_NumOfGuesses)
        {
            return i_NumOfGuesses >= k_MinNumOfGuess && i_NumOfGuesses <= k_MaxNumOfGuess;
        }

        public bool IsValidGuess(string i_Guess)
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(i_Guess) || i_Guess.Length != k_ValidGuessLength)
            {
                isValid = false;
            }
            else
            {
                for (int i = 0; i < i_Guess.Length && isValid; i++)
                {
                    char letter = i_Guess[i];

                    if (letter < k_FirstLetterOfRange || letter > k_LastLetterOfRange || !char.IsUpper(letter))
                    {
                        isValid = false;
                    }
                    for (int j = 0; j < i && isValid; j++)
                    {
                        if (i_Guess[i] == i_Guess[j])
                        {
                            isValid = false;
                        }
                    }
                }
            }

            return isValid;
        }

        public UserGuess AddGuess(string i_Guess)
        {
            UserGuess currentUserGuess = new UserGuess(i_Guess);
            
            m_SecretWord.CheckUserGuess(ref currentUserGuess);
            m_GuessesHistory.Add(currentUserGuess);

            return currentUserGuess;
        }
    }
}
