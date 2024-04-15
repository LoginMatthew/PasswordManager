
namespace PasswordManager.Stores
{    
    public class RandomGenerator
    {
        private readonly Random random;

        public RandomGenerator()
        {
            random = new Random();
        }

        public string GetRandomlyGeneratedStringWithOutNumber(int generatedLengthSize)
        {
            int randomNumber;
            string letter = "";
            for (int i = 0; i < generatedLengthSize; i++)
            {
                //Generating random number and converting into character
                randomNumber = random.Next(0, 26);
                letter += Convert.ToChar(randomNumber + 65);
            }

            return letter;
        }
    }
}