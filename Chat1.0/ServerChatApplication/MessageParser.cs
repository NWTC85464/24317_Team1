using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerChatApplication
{
    public static class MessageParser
    {
        // Holds the original message that's sent to the server before any tokenizing occurs
        private static string completeMessage;

        // Holds the tokenized message
        private static string[] tokenizedMessage;

        // Get and set for the completeMessage field. Once a new value is set, the tokenize method is called
        public static string CompleteMessage
        {
            get { return completeMessage; }
            set
            {
                completeMessage = value;
                TokenizeMessage();
            }
        }

        // Tokenize the original message and redirect the program flow
        private static void TokenizeMessage()
        {
            tokenizedMessage = completeMessage.Split('|');

            switch (tokenizedMessage[0])
            {
                case "<Message>":
                    // TODO setup appropriate methods
                    break;
                case "<Login>":
                    // TODO setup appropriate methods
                    break;
                case "<Signup>":
                    // TODO setup appropriate methods
                    break;
            }
        }
    }
}
