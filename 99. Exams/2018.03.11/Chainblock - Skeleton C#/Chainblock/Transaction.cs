using System;

public class Transaction : IComparable<Transaction>
{
    public int Id { get; set; }
    public TransactionStatus Status { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public double Amount { get; set; }

    public Transaction(int id, TransactionStatus st, string from, string to, double amount)
    {
        this.Id = id;
        this.Status = st;
        this.From = from;
        this.To = to;
        this.Amount = amount;
    }

    public int CompareTo(Transaction other)
    {
        int result = this.Id.CompareTo(other.Id);
        if (result == 0)
        {
            result = this.Status.CompareTo(other.Status);
            if (result == 0)
            {
                result = this.From.CompareTo(other.From);
                if (result == 0)
                {
                    result = this.To.CompareTo(other.To);
                    if (result == 0)
                    {
                        result = this.Amount.CompareTo(other.Amount);
                    }
                }
            }
        }
        return result;
    }
}