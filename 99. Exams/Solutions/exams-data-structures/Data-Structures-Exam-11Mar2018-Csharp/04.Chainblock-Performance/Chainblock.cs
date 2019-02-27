using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

public class Chainblock : IChainblock
{
    Dictionary<int, LinkedListNode<Transaction>> byId = new Dictionary<int, LinkedListNode<Transaction>>();
    Dictionary<TransactionStatus, OrderedDictionary<double, LinkedList<Transaction>>> byTxStatus
        = new Dictionary<TransactionStatus, OrderedDictionary<double, LinkedList<Transaction>>>()
        {
            {TransactionStatus.Aborted, new OrderedDictionary<double, LinkedList<Transaction>>((x,y) => y.CompareTo(x)) },
            {TransactionStatus.Failed,  new OrderedDictionary<double, LinkedList<Transaction>>((x,y) => y.CompareTo(x)) },
            {TransactionStatus.Successfull, new OrderedDictionary<double, LinkedList<Transaction>>((x,y) => y.CompareTo(x))},
            {TransactionStatus.Unauthorized, new OrderedDictionary<double, LinkedList<Transaction>>((x,y) => y.CompareTo(x)) }
        };


    public int Count => this.byId.Count;

    public void Add(Transaction tx)
    {
        LinkedListNode<Transaction> node = new LinkedListNode<Transaction>(tx);
        //LinkedListNode<Transaction> senderNode = new LinkedListNode<Transaction>(tx);
        this.byId.Add(tx.Id, node);
        if (!this.byTxStatus[tx.Status].ContainsKey(tx.Amount))
        {
            this.byTxStatus[tx.Status][tx.Amount] = new LinkedList<Transaction>();
        }
        this.byTxStatus[tx.Status][tx.Amount].AddLast(node);
        //if (!this.bySender.ContainsKey(tx.From))
        //{
        //    this.bySender[tx.From] = 
        //       new OrderedDictionary<double, LinkedList<Transaction>>(
        //            (x, y) => y.CompareTo(x)
        //        );
        //}
        //if (!this.bySender[tx.From].ContainsKey(tx.Amount))
        //{
        //    this.bySender[tx.From].Add(tx.Amount, new LinkedList<Transaction>());
        //}
        //if (!this.byReceiver.ContainsKey(tx.To))
        //{ 
         //   this.byReceiver[tx.To] = new OrderedBag<Transaction>();
        //}
        //this.bySender[tx.From][tx.Amount].AddLast(senderNode);
        //this.senderNodes.Add(tx.Id, senderNode);
        //this.byReceiver[tx.To].Add(tx);
    }

    public bool Contains(Transaction tx)
    {
        return this.byId.ContainsKey(tx.Id);
    }

    public bool Contains(int id)
    {
        return this.byId.ContainsKey(id);
    }

    //No such id -> InvalidOperationException
    public Transaction GetById(int id)
    {
        if (!this.byId.ContainsKey(id))
        {
            throw new InvalidOperationException();
        }
        return this.byId[id].Value;
    }

    //No such id -> ArgumentExecption();
    public void ChangeTransactionStatus(int id, TransactionStatus newStatus)
    {
        if (!this.byId.ContainsKey(id))
        {
            throw new ArgumentException();
        }

        LinkedListNode<Transaction> tx = this.byId[id];
        this.byTxStatus[tx.Value.Status][tx.Value.Amount].Remove(tx);
        tx.Value.Status = newStatus;
        if (!this.byTxStatus[newStatus].ContainsKey(tx.Value.Amount))
        {
            this.byTxStatus[newStatus][tx.Value.Amount] = new LinkedList<Transaction>();
        }
        this.byTxStatus[newStatus][tx.Value.Amount].AddLast(tx);
        
    }

    //By insertion order (inclusive)
    public IEnumerable<Transaction> GetAllInAmountRange(double lo, double hi)
    {
        return this.byId.Values
            .Select(x => x.Value)
            .Where(x => x.Amount >= lo && x.Amount <= hi);
    }

    public IEnumerable<Transaction> GetAllOrderedByAmountDescendingThenById()
    {
        return this.byId.Values.Select(x => x.Value)
                .OrderByDescending(x => x.Amount)
                .ThenBy(x => x.Id);
    }

    //Throws InvalidOperationException if no tx with such status
    public IEnumerable<string> GetAllSendersWithTransactionStatus(TransactionStatus status)
    {
        if (this.byTxStatus[status].Count == 0)
        {
            throw new InvalidOperationException();
        }

        return this.byTxStatus[status].SelectMany(x => x.Value).Select(x => x.From);
    }
    public IEnumerable<string> GetAllReceiversWithTransactionStatus(TransactionStatus status)
    {
        if (this.byTxStatus[status].Count == 0)
        {
            throw new InvalidOperationException();
        }

        return this.byTxStatus[status].SelectMany(x => x.Value).Select(x => x.To);
    }

    //Order by amount descending then by tx id (lower-end inclusive higher-end exclusive)
    //Throws InvalidOperationException if no such receiver exists
    public IEnumerable<Transaction> GetByReceiverAndAmountRange(string receiver, double lo, double hi)
    {
        List<Transaction> result = this.byId.Values
            .Where(x => x.Value.Amount >= lo && x.Value.Amount < hi && x.Value.To == receiver)
            .Select(x => x.Value)
            .OrderByDescending(x => x.Amount)
            .ThenBy(x => x.Id)
            .ToList();

        //if (!this.byReceiver.ContainsKey(receiver))
        if(result.Count == 0)
        {
            throw new InvalidOperationException();
        }
        //Transaction loT = 
        //    new Transaction(int.MaxValue, TransactionStatus.Aborted, "a", "a", lo);
        //Transaction hiT =
        //    new Transaction(int.MaxValue, TransactionStatus.Failed, "a", "a", hi);
        //return this.byReceiver[receiver].Range(hiT, false, loT, true);
        return result;
    }

    //If no such receiver -> throws InvalidOperationException()
    public IEnumerable<Transaction> GetByReceiverOrderedByAmountThenById(string receiver)
    {
        List<Transaction> result = this.byId.Values
            .Where(x => x.Value.To == receiver)
            .Select(x => x.Value)
            .OrderByDescending(x => x.Amount)
            .ThenBy(x => x.Id)
            .ToList();

        //if(!this.byReceiver.ContainsKey(receiver))
        if(result.Count == 0)
        {
            throw new InvalidOperationException();
        }
        //
        //return this.byReceiver[receiver].ToList();
        return result;
    }

    //If sender is missing throw InvalidOperationExeption
    //Min amount is exclusive
    //Ordered by amount rest is preserved by insertion
    public IEnumerable<Transaction> GetBySenderAndMinimumAmountDescending(string sender, double amount)
    {

        List<Transaction> result = this.byId.Values
            .Where(x => x.Value.From == sender && x.Value.Amount > amount)
            .Select(x => x.Value)
            .OrderByDescending(x => x.Amount)
            .ToList();

        if (result.Count == 0)
        {
            throw new InvalidOperationException();
        }

        return result;
    }

    //Throws InvalidOperationException in sender is missing
    //All by sender ordered by amount descending
    public IEnumerable<Transaction> GetBySenderOrderedByAmountDescending(string sender)
    {

        List<Transaction> result = this.byId.Values
            .Where(x => x.Value.From == sender)
            .Select(x => x.Value)
            .OrderByDescending(x => x.Amount)
            .ToList();

        if (result.Count == 0)
        {
            throw new InvalidOperationException();
        }

        return result;
    }

    //Throws InvalidOperationException if no tx with such status
    //Ordered by amount descending
    public IEnumerable<Transaction> GetByTransactionStatus(TransactionStatus status)
    {
        if (this.byTxStatus[status].Count == 0)
        {
            throw new InvalidOperationException();
        }
        foreach(var item in this.byTxStatus[status])
        {
            foreach(var subitem in item.Value)
            {
                yield return subitem;
            }
        }
    }

    //InclusiveOrderByDescending
    //Shoud return empty collection no matter what
    public IEnumerable<Transaction> GetByTransactionStatusAndMaximumAmount(TransactionStatus status, double amount)
    {
        return this.byTxStatus[status]
            .RangeFrom(amount, true)
            .SelectMany(x => x.Value);
    }


    public void RemoveTransactionById(int id)
    {
        if (!this.byId.ContainsKey(id))
        {
            throw new InvalidOperationException();
        }

        LinkedListNode<Transaction> tx = this.byId[id];
        this.byId.Remove(id);
        LinkedList<Transaction> list = this.byTxStatus[tx.Value.Status][tx.Value.Amount];
        list.Remove(tx);
        if(list.Count == 0)
        {
            this.byTxStatus[tx.Value.Status].Remove(tx.Value.Amount);
        }
    }


    public IEnumerator<Transaction> GetEnumerator()
    {
        foreach (var item in this.byId)
        {
            yield return item.Value.Value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

}

