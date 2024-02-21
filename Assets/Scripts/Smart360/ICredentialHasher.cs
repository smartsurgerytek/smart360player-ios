using System.Collections.Generic;
using System;

internal interface ICredentialHasher<T>
{
    string Hash(T credential);
}