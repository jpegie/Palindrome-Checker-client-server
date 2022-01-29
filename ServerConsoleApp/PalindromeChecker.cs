using System;
using System.Threading;

namespace ServerConsoleApp
{
    internal class PalindromeChecker
    {
        IRequest request;
        public PalindromeChecker(IRequest request)
        {
            this.request = request;
        }
        public ResultState GetPalindromeState()
        {
            Thread.Sleep(2000); //имитируем долгую обработку 
            ResultState result = ResultState.Palindrome;

            for (int i = 0; i < request.Data.Length / 2; ++i)
            {
                if (request.Data[i] != request.Data[request.Data.Length - 1 - i] && Char.ToLower(request.Data[i]) != Char.ToLower(request.Data[request.Data.Length - 1 - i]))
                {
                    result = ResultState.NotPalindrome;
                    return result;
                }
            }
            return result;
        }
    }
}
