using System;
using System.Linq;

namespace Ex02
{
    public class SecretWord
    {
        private readonly string m_secretWord;

        public string SecretWordValue { get { return m_secretWord; } }

        public SecretWord()
        {
            m_secretWord = generateSecretWord();
        }

        private static string generateSecretWord()
        {
            string allowedLetters = "ABCDEFGH";
            char[] shuffledLetters = allowedLetters.ToCharArray();
            Random randomGenerator = new Random();
            int randomIndex = 0;

            for (int i = shuffledLetters.Length - 1; i > 0; i--)
            {
                randomIndex = randomGenerator.Next(0, i + 1);
                (shuffledLetters[i], shuffledLetters[randomIndex]) = (shuffledLetters[randomIndex], shuffledLetters[i]);
            }

            return new string(shuffledLetters, 0, 4);
        }

        public void CheckUserGuess(ref GameLogic.UserGuess io_currentGuess)
        {
            for (int i = 0; i < io_currentGuess.GuessWord.Length; i++)
            {
                char currentCharInGuess = io_currentGuess.GuessWord[i];

                if (m_secretWord.Contains(currentCharInGuess))
                {
                    if (m_secretWord[i] == currentCharInGuess)
                    {
                        io_currentGuess.RightLettersInRightPosCount++;
                    }
                    else
                    {
                        io_currentGuess.RightLettersInWrongPosCount++;
                    }
                }
            }
        }
    }
}
