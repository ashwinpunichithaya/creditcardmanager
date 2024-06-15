namespace CreditCardManager.Validators
{
    /// <summary>
    /// Custom validator for credit card number.
    /// </summary>
    public class CardNumberValidator
    {
        private readonly IEnumerable<string> ValidCardNumbers;

        /// <summary>
        /// Constructor
        /// </summary>
        public CardNumberValidator()
        {
            // sourced from https://www.paypalobjects.com/en_AU/vhelp/paypalmanager_help/credit_card_numbers.htm
            ValidCardNumbers =
            [
                "378282246310005",
                "371449635398431",
                "378734493671000",
                "5610591081018250",
                "30569309025904",
                "38520000023237",
                "6011111111111117",
                "6011000990139424",
                "3530111333300000",
                "3566002020360505",
                "5555555555554444",
                "5105105105105100",
                "4111111111111111",
                "4012888888881881",
                "4222222222222",
                "76009244561",
                "5019717010103742",
                "6331101999990016"
            ];
        }

        /// <summary>
        /// Checks whether given credit card number is in the valid cards list.
        /// </summary>
        /// <param name="cardNumber">Credit card number</param>
        /// <returns></returns>
        internal bool IsValid(string cardNumber)
        {
            return ValidCardNumbers.Contains(cardNumber);
        }
    }
}