using System;
using System.Collections.Generic;
using System.Linq;

public class EasonModuleCredentialHasher : ICredentialHasher<ModuleCredential>
{
    string ICredentialHasher<ModuleCredential>.Hash(ModuleCredential credential)
    {

        var numList = new Queue<long>();
        EnqueneBySalt(numList, credential.deviceUniqueIdentifier.GetHashCode());
        if (credential.purchased)
        {
            EnqueneBySalt(numList, credential.deviceUniqueIdentifier.GetHashCode());
        }
        EnqueneBySalt(numList, credential.id);
        EnqueneBySalt(numList, credential.expiredDate);

        long num = numList.Dequeue();
        while (numList.Any())
        {
            var power = numList.Dequeue();
            if (power == 0) continue;
            num = Int64Pow(num, power);
        }
        return num.ToString();
    }
    private void EnqueneBySalt(Queue<long> queue, long value, long salt = 100)
    {
        queue.Enqueue(value + salt);
    }
    Int64 Int64Pow(Int64 x, Int64 pow)
    {
        Int64 ret = 1;
        while (pow != 0)
        {
            if ((pow & 1) == 1)
                ret *= x;
            x *= x;
            pow >>= 1;
        }
        return ret;
    }
}
