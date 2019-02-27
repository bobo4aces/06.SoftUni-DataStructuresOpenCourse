using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Program
{
    static void Main(string[] args)
    {
        IChainblock cb = new Chainblock();
        int count = 100000;
        List<Transaction> txs = new List<Transaction>();
        TransactionStatus[] statuses = new TransactionStatus[]
        {
            TransactionStatus.Aborted,
            TransactionStatus.Failed,
            TransactionStatus.Successfull,
            TransactionStatus.Unauthorized
        };
        Random rand = new Random();
        for (int i = 0; i < count; i++)
        {
            int status = rand.Next(0, 4);
            Transaction tx = new Transaction(i, statuses[status],
                i.ToString(), i.ToString(), i);
            cb.Add(tx);
            txs.Add(tx);
        }

        Stopwatch watch = new Stopwatch();
        watch.Start();

        foreach (Transaction tx in txs)
        {
            cb.Contains(tx);
        }

        watch.Stop();
        long l1 = watch.ElapsedMilliseconds;

        Console.WriteLine(l1);
    }
}
