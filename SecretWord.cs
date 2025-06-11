using System;

namespace Ex02
{
    public class SecretWord
    {
        private string m_SecretWord;

        public string SecretWordValue
        {
            get { return m_SecretWord; }
        }

        public SecretWord()
        {
            m_SecretWord = generateSecretWord();
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
                char temp = shuffledLetters[i];
                shuffledLetters[i] = shuffledLetters[randomIndex];
                shuffledLetters[randomIndex] = temp;
            }

            return new string(shuffledLetters, 0, 4);
        }

        public void CheckUserGuess(ref GameLogic.UserGuess io_CurrentGuess)
        {
            for (int i = 0; i < io_CurrentGuess.GuessWord.Length; i++)
            {
                char currentCharInGuess = io_CurrentGuess.GuessWord[i];

                int posInSecret = m_SecretWord.IndexOf(currentCharInGuess);
                if (posInSecret != -1)
                {
                    if (m_SecretWord[i] == currentCharInGuess)
                    {
                        io_CurrentGuess.RightLettersInRightPosCount++;
                    }
                    else
                    {
                        io_CurrentGuess.RightLettersInWrongPosCount++;
                    }
                }
            }
        }
    }
}
