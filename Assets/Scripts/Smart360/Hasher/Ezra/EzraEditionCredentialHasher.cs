﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;

public class EzraEditionCredentialHasher : ICredentialHasher<EditionCredential>
{
    string ICredentialHasher<EditionCredential>.Hash(EditionCredential credential)
    {
        var numList = new Queue<byte>();
        numList.Enqueue((byte)credential.id);
        var tmp = BitConverter.GetBytes(credential.deviceUniqueIdentifier.GetHashCode());
        foreach( var elm in tmp)
        {
            numList.Enqueue(elm);
        }
        if (credential.purchased)
        {
            EnqueneBySalt(numList, credential.deviceUniqueIdentifier);
        }
        EnqueueLong(numList,credential.expiredDate);
        string output = "";
        using (SHA1 sha1 = SHA1.Create())
        {
            byte[] hashValue = sha1.ComputeHash(numList.ToArray());
            output = BitConverter.ToString(hashValue).Replace("-", "");
        }
        return output;
    }

    private void EnqueueLong(Queue<byte> numList, long expiredDate)
    {
        byte[] ret = BitConverter.GetBytes(expiredDate);
        foreach( var elm in ret)
        {
            numList.Enqueue(elm);
        }
    }
    
    private void EnqueneBySalt(Queue<byte> queue, string value, string salt = "4940")
    {
        EnqueueInt(queue,(value+salt).GetHashCode());
    }
    void EnqueueInt(Queue<byte> numList,int input)
    {
        byte[] ret = BitConverter.GetBytes(input);
        foreach (var elm in ret) {
            numList.Enqueue(elm);
        }
    }
  }
