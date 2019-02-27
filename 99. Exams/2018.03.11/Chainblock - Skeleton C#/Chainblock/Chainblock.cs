using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

public class Chainblock : IChainblock
{
    private Dictionary<int, Transaction> transactions;
    private OrderedDictionary<TransactionStatus, OrderedDictionary<double, Bag<Transaction>>> byStatus;
    private OrderedDictionary<double, OrderedDictionary<int, Transaction>> byAmountAndId;
    private OrderedDictionary<string, OrderedDictionary<double, Bag<Transaction>>> bySenderAndAmount;
    private OrderedDictionary<string, OrderedDictionary<double,OrderedDictionary<int, Transaction>>> byReceiverAndAmountAndId;
    private OrderedDictionary<TransactionStatus, OrderedDictionary<double,OrderedBag<Transaction>>> byStatusAndAmount;
    private OrderedDictionary<double, OrderedBag<Transaction>> byAmount;
    public Chainblock()
    {
        this.transactions = new Dictionary<int, Transaction>();
        this.byStatus = new OrderedDictionary<TransactionStatus, OrderedDictionary<double, Bag<Transaction>>>();
        this.byAmountAndId = new OrderedDictionary<double, OrderedDictionary<int, Transaction>>();
        this.bySenderAndAmount = new OrderedDictionary<string, OrderedDictionary<double, Bag<Transaction>>>();
        this.byReceiverAndAmountAndId = new OrderedDictionary<string, OrderedDictionary<double, OrderedDictionary<int, Transaction>>>();
        this.byStatusAndAmount = new OrderedDictionary<TransactionStatus, OrderedDictionary<double, OrderedBag<Transaction>>>();
        this.byAmount = new OrderedDictionary<double, OrderedBag<Transaction>>();
    }
    public int Count => this.transactions.Count;

    public void Add(Transaction tx)
    {
        this.transactions.Add(tx.Id, tx);

        if (!this.byStatus.ContainsKey(tx.Status))
        {
            this.byStatus.Add(tx.Status, new OrderedDictionary<double, Bag<Transaction>>());
        }
        if (!this.byStatus[tx.Status].ContainsKey(tx.Amount))
        {
            this.byStatus[tx.Status].Add(tx.Amount,new Bag<Transaction>());
        }
        this.byStatus[tx.Status][tx.Amount].Add(tx);

        if (!this.byAmountAndId.ContainsKey(tx.Amount))
        {
            this.byAmountAndId.Add(tx.Amount, new OrderedDictionary<int, Transaction>());
        }
        if (!this.byAmountAndId[tx.Amount].ContainsKey(tx.Id))
        {
            this.byAmountAndId[tx.Amount].Add(tx.Id, tx);
        }
        else
        {
            this.byAmountAndId[tx.Amount][tx.Id] = tx;
        }

        if (!this.bySenderAndAmount.ContainsKey(tx.From))
        {
            this.bySenderAndAmount.Add(tx.From, new OrderedDictionary<double, Bag<Transaction>>());
        }
        if (!this.bySenderAndAmount[tx.From].ContainsKey(tx.Amount))
        {
            this.bySenderAndAmount[tx.From].Add(tx.Amount, new Bag<Transaction>());
        }
        this.bySenderAndAmount[tx.From][tx.Amount].Add(tx);

        if (!this.byReceiverAndAmountAndId.ContainsKey(tx.To))
        {
            this.byReceiverAndAmountAndId.Add(tx.To, new OrderedDictionary<double, OrderedDictionary<int, Transaction>>());
        }
        if (!this.byReceiverAndAmountAndId[tx.To].ContainsKey(tx.Amount))
        {
            this.byReceiverAndAmountAndId[tx.To].Add(tx.Amount, new OrderedDictionary<int, Transaction>());
        }
        if (!this.byReceiverAndAmountAndId[tx.To][tx.Amount].ContainsKey(tx.Id))
        {
            this.byReceiverAndAmountAndId[tx.To][tx.Amount].Add(tx.Id,tx);
        }
        else
        {
            this.byReceiverAndAmountAndId[tx.To][tx.Amount][tx.Id] = tx;
        }
        if (!this.byStatusAndAmount.ContainsKey(tx.Status))
        {
            this.byStatusAndAmount.Add(tx.Status, new OrderedDictionary<double, OrderedBag<Transaction>>());
        }
        if (!this.byStatusAndAmount[tx.Status].ContainsKey(tx.Amount))
        {
            this.byStatusAndAmount[tx.Status].Add(tx.Amount, new OrderedBag<Transaction>());
        }
        this.byStatusAndAmount[tx.Status][tx.Amount].Add(tx);
        if (!this.byAmount.ContainsKey(tx.Amount))
        {
            this.byAmount.Add(tx.Amount, new OrderedBag<Transaction>());
        }
        this.byAmount[tx.Amount].Add(tx);

    }

    public void ChangeTransactionStatus(int id, TransactionStatus newStatus)
    {
        if (!this.transactions.ContainsKey(id))
        {
            throw new ArgumentException();
        }
        Transaction transaction = this.transactions[id];
        this.transactions[id].Status = newStatus;
        Transaction newTransaction = this.transactions[id];
        this.byStatus[transaction.Status][transaction.Amount].Remove(transaction);
        if (!this.byStatus.ContainsKey(newTransaction.Status))
        {
            this.byStatus.Add(newTransaction.Status, new OrderedDictionary<double, Bag<Transaction>>());
        }
        if (!this.byStatus[newTransaction.Status].ContainsKey(newTransaction.Amount))
        {
            this.byStatus[newTransaction.Status].Add(newTransaction.Amount, new Bag<Transaction>());
        }
        this.byStatus[newTransaction.Status][newTransaction.Amount].Add(newTransaction);

        this.byAmountAndId[transaction.Amount][transaction.Id].Status = newStatus;

        this.bySenderAndAmount[transaction.From][transaction.Amount].First().Status = newStatus;
        this.byReceiverAndAmountAndId[transaction.To][transaction.Amount][transaction.Id].Status = newStatus;
        this.byStatusAndAmount[transaction.Status][transaction.Amount].First().Status = newStatus;
        this.byAmount[transaction.Amount].Where(t => t.CompareTo(transaction) == 0).First().Status = newStatus;
    }

    public bool Contains(Transaction tx)
    {
        if (!this.transactions.ContainsKey(tx.Id))
        {
            return false;
        }
        return true;
    }

    public bool Contains(int id)
    {
        if (!this.transactions.ContainsKey(id))
        {
            return false;
        }
        return true;
    }

    public IEnumerable<Transaction> GetAllInAmountRange(double lo, double hi)
    {
        var collection = this.byAmount.Range(lo, true, hi, true);
        List<Transaction> result = new List<Transaction>();
        if (collection.Count == 0)
        {
            return result;
        }
        foreach (var item in collection)
        {
            foreach (var subItem in item.Value)
            {
                result.Add(subItem);
            }
        }
        return result;
    }

    public IEnumerable<Transaction> GetAllOrderedByAmountDescendingThenById()
    {
        foreach (var amount in this.byAmountAndId.Reverse())
        {
            foreach (var id in amount.Value)
            {
                yield return id.Value;
            }
        }
    }

    public IEnumerable<string> GetAllReceiversWithTransactionStatus(TransactionStatus status)
    {
        if (!this.byStatus.ContainsKey(status))
        {
            throw new InvalidOperationException();
        }
        List<string> result = new List<string>();
        foreach (var price in this.byStatus[status])
        {
            foreach (var item in price.Value)
            {
                result.Add(item.From);
            }
        }
        return result;
    }

    public IEnumerable<string> GetAllSendersWithTransactionStatus(TransactionStatus status)
    {
        if (!this.byStatus.ContainsKey(status))
        {
            throw new InvalidOperationException();
        }
        List<string> result = new List<string>();
        foreach (var price in this.byStatus[status])
        {
            foreach (var item in price.Value)
            {
                result.Add(item.To);
            }
        }
        return result;
    }

    public Transaction GetById(int id)
    {
        if (!this.transactions.ContainsKey(id))
        {
            throw new InvalidOperationException();
        }
        return this.transactions[id];
    }

    public IEnumerable<Transaction> GetByReceiverAndAmountRange(string receiver, double lo, double hi)
    {
        if (!this.byReceiverAndAmountAndId.ContainsKey(receiver))
        {
            throw new InvalidOperationException();
        }
        List<Transaction> result = new List<Transaction>();
        foreach (var currentReceiver in this.byReceiverAndAmountAndId[receiver].Range(lo,true,hi,false).Reverse())
        {
            foreach (var price in currentReceiver.Value)
            {
                result.Add(price.Value);
            }
        }
        return result;
    }

    public IEnumerable<Transaction> GetByReceiverOrderedByAmountThenById(string receiver)
    {
        
        if (!this.byReceiverAndAmountAndId.ContainsKey(receiver))
        {
            throw new InvalidOperationException();
        }
        List<Transaction> result = new List<Transaction>();
        foreach (var currentReceiver in this.byReceiverAndAmountAndId[receiver].Reverse())
        {
            foreach (var price in currentReceiver.Value)
            {
                result.Add(price.Value);
            }
        }
        return result;
    }

    public IEnumerable<Transaction> GetBySenderAndMinimumAmountDescending(string sender, double amount)
    {
        if (!this.bySenderAndAmount.ContainsKey(sender))
        {
            throw new InvalidOperationException();
        }
        List<Transaction> result = new List<Transaction>();
        foreach (var currentSender in this.bySenderAndAmount[sender].RangeFrom(amount,false))
        {
            foreach (var transaction in currentSender.Value.Reverse())
            {
                result.Add(transaction);
            }
        }
        return result;
    }

    public IEnumerable<Transaction> GetBySenderOrderedByAmountDescending(string sender)
    {
        if (!this.bySenderAndAmount.ContainsKey(sender))
        {
            throw new InvalidOperationException();
        }
        List<Transaction> result = new List<Transaction>();
        foreach (var currentSender in this.bySenderAndAmount[sender])
        {
            foreach (var transaction in currentSender.Value.Reverse())
            {
                result.Add(transaction);
            }
        }
        return result;
    }

    public IEnumerable<Transaction> GetByTransactionStatus(TransactionStatus status)
    {
        if (!this.byStatus.ContainsKey(status))
        {
            throw new InvalidOperationException();
        }
        List<Transaction> result = new List<Transaction>();
        foreach (var price in this.byStatus[status].Reverse())
        {
            foreach (var item in price.Value)
            {
                result.Add(item);
            }
        }
        return result;
    }

    public IEnumerable<Transaction> GetByTransactionStatusAndMaximumAmount(TransactionStatus status, double amount)
    {
        List<Transaction> result = new List<Transaction>();
        if (!this.byStatusAndAmount.ContainsKey(status))
        {
            return result;
        }
        foreach (var item in this.byStatusAndAmount[status].RangeTo(amount, true))
        {
            foreach (var subItem in item.Value)
            {
                result.Add(subItem);
            }
            
        }
        return result;
    }

    public IEnumerator<Transaction> GetEnumerator()
    {
        foreach (var transaction in this.transactions)
        {
            yield return transaction.Value;
        }
    }

    public void RemoveTransactionById(int id)
    {
        if (!this.transactions.ContainsKey(id))
        {
            throw new InvalidOperationException();
        }
        Transaction transaction = this.transactions[id];
        this.transactions.Remove(id);
        this.byStatus[transaction.Status][transaction.Amount].Remove(transaction);
        this.byAmountAndId[transaction.Amount].Remove(transaction.Id);
        this.bySenderAndAmount[transaction.From][transaction.Amount].Remove(transaction);
        this.byReceiverAndAmountAndId[transaction.To][transaction.Amount].Remove(transaction.Id);
        this.byStatusAndAmount[transaction.Status][transaction.Amount].Remove(transaction);
        this.byAmount[transaction.Amount].Remove(transaction);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}

