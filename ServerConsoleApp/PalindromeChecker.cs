using System;
using System.Threading;

namespace ServerConsoleApp
{
    internal static class PalindromeChecker
    {
        public static States GetPalindromeState(IRequest request)
        {
            Thread.Sleep(2000); //имитируем долгую обработку 
            States result = States.Palindrome;

            if(request.Data == "")
            {
                result = States.NotPalindrome;
                return result;
            }

            for (int i = 0; i < request.Data.Length / 2; ++i)
            {
                if (request.Data[i] != request.Data[request.Data.Length - 1 - i] && Char.ToLower(request.Data[i]) != Char.ToLower(request.Data[request.Data.Length - 1 - i]))
                {
                    result = States.NotPalindrome;
                    return result;
                }
            }
            return result;
        }
    }
}
