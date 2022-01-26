using System;
using System.Runtime.Serialization;
[Serializable]
public enum ResultState
{
    [EnumMember(Value = "Palindrome")]
    Palindrome,
    [EnumMember(Value = "NotPalindrome")]
    NotPalindrome,
    [EnumMember(Value = "NotChecked")]
    NotChecked,
    [EnumMember(Value = "TryAgain")]
    TryAgain,
    [EnumMember(Value = "ServerOverloaded")]
    ServerOverloaded
}