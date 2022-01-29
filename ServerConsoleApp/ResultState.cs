using System;
using System.Runtime.Serialization;

[Serializable]
public enum States
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