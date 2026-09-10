using System;
using System.Collections.Generic;

namespace FinanceManagementSystem
{
    // Record Type
    public record Transaction(
        int Id,
        DateTime Date,
        decimal Amount,
        string Category
    );

    // Interface
    public interface ITransactionProcessor
    {
        void Process(Transaction transaction);
    }

    // Bank Transfer Processor
    public class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine(
                $"[Bank Transfer] Processed GHS {transaction.Amount} for {transaction.Category}");
        }
    }

    // Mobile Money Processor
    public class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine(
                $"[Mobile Money] Processed GHS {transaction.Amount} for {transaction.Category}");
        }
    }

    // Crypto Wallet Processor
    public class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine(
                $"[Crypto Wallet] Processed GHS {transaction.Amount} for {transaction.Category}");
        }
    }

    // Base Account Class
    public class Account
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; protected set; }

        public Account(string accountNumber, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public virtual void ApplyTransaction(Transaction transaction)
        {
            Balance -= transaction.Amount;
        }
    }

    // Sealed Savings Account
    public sealed class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, decimal initialBalance)
            : base(accountNumber, initialBalance)
        {
        }

        public override void ApplyTransaction(Transaction transaction)
        {
            if (transaction.Amount > Balance)
            {
                Console.WriteLine(
                    $"Transaction {transaction.Id}: Insufficient funds.");
            }
            else
            {
                Balance -= transaction.Amount;

                Console.WriteLine(
                    $"Transaction {transaction.Id} successful." +
                    $" New Balance = GHS {Balance}");
            }
        }
    }

    // Base Application Class
    public class FinanceAppBase
    {
        protected readonly List<Transaction> _transactions;

        public FinanceAppBase()
        {
            _transactions = new List<Transaction>();
        }
    }

    public class FinanceApp : FinanceAppBase
    {
        public void Run()
        {
            SavingsAccount account =
                new SavingsAccount("SA1001", 1000m);

            Console.WriteLine(
                $"Account Created: {account.AccountNumber}");
            Console.WriteLine(
                $"Opening Balance: GHS {account.Balance}\n");

            Transaction t1 = new Transaction(
                1,
                DateTime.Now,
                100m,
                "Groceries");

            Transaction t2 = new Transaction(
                2,
                DateTime.Now,
                150m,
                "Utilities");

            Transaction t3 = new Transaction(
                3,
                DateTime.Now,
                200m,
                "Entertainment");

            ITransactionProcessor mobile =
                new MobileMoneyProcessor();

            ITransactionProcessor bank =
                new BankTransferProcessor();

            ITransactionProcessor crypto =
                new CryptoWalletProcessor();

            mobile.Process(t1);
            account.ApplyTransaction(t1);

            Console.WriteLine();

            bank.Process(t2);
            account.ApplyTransaction(t2);

            Console.WriteLine();

            crypto.Process(t3);
            account.ApplyTransaction(t3);

            Console.WriteLine();

            _transactions.AddRange(
                new List<Transaction> { t1, t2, t3 });

            Console.WriteLine(
                $"Total Transactions Stored: {_transactions.Count}");

            Console.WriteLine(
                $"Final Account Balance: GHS {account.Balance}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            FinanceApp app = new FinanceApp();
            app.Run();
        }
    }
}
