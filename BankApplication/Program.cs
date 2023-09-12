using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace BankApplication
{
    public class TransactionDetails
    {
        public string Date { get; set; }
        public string Account { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string TxnId { get; set; }
    }

    public class InterestRuleDetails
    {
        public string Date { get; set; }
        public string RuleId { get; set; }
        public decimal Rate { get; set; }
    }

    public class Account
    {
        public string AccountNumber { get; set; }
        public List<TransactionDetails> Transactions { get; set; } = new List<TransactionDetails>();
        public List<InterestRuleDetails> InterestRules { get; set; } = new List<InterestRuleDetails>();
    }

    public class Program
    {
        private static List<Account> accounts = new List<Account>();

        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to AwesomeGIC Bank! What would you like to do?");
            while (true)
            {
                Console.WriteLine("[I]nput transactions");
                Console.WriteLine("[D]efine interest rules");
                Console.WriteLine("[P]rint statement");
                Console.WriteLine("[Q]uit");
                Console.Write("> ");

                var choice = Console.ReadLine().ToLower();
                switch (choice)
                {
                    case "i":
                        TransactionBegin();
                        break;

                    case "d":
                        IntroduceRules();
                        break;

                    case "p":
                        Print();
                        break;

                    case "q":
                        Console.WriteLine("Thank you for banking with AwesomeGIC Bank.");
                        Console.WriteLine("Have a nice day!");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void TransactionBegin()
        {
            Console.WriteLine("Please enter transaction details in <Date>|<Account>|<Type>|<Amount> format");
            Console.WriteLine("(or enter blank to go back to the main menu):");
            Console.Write("> ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                return;

            var parts = input.Split('|');
            if (parts.Length != 4)
            {
                Console.WriteLine("Invalid input format. Please try again.");
                return;
            }

            var date = parts[0];
            var accountNumber = parts[1];
            var type = parts[2].ToUpper();
            var amountStr = parts[3];

            if (!DateTime.TryParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                Console.WriteLine("Invalid date format. Please use YYYYMMdd.");
                return;
            }

            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                Console.WriteLine("Invalid account number. Account number cannot be empty.");
                return;
            }

            if (type != "D" && type != "W")
            {
                Console.WriteLine("Invalid transaction type. Please use 'D' for deposit or 'W' for withdrawal.");
                return;
            }

            if (!decimal.TryParse(amountStr, out var amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount. Amount must be a positive number.");
                return;
            }

            var account = accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);

            if (account == null)
            {
                account = new Account { AccountNumber = accountNumber };
                accounts.Add(account);
            }

            if (account.Transactions.Count == 0 && type == "W")
            {
                Console.WriteLine("The first transaction for an account should not be a withdrawal.");
                return;
            }

            var txnId = $"{date}-{account.Transactions.Count + 1:D2}";
            var transaction = new TransactionDetails { Date = date, Account = accountNumber, Type = type, Amount = amount, TxnId = txnId };

            if (type == "W")
            {
                var balance = account.Transactions.Sum(t => t.Type == "D" ? t.Amount : -t.Amount);
                if (balance - amount < 0)
                {
                    Console.WriteLine("TransactionDetails would result in a negative balance. Operation not allowed.");
                    return;
                }
            }

            account.Transactions.Add(transaction);
            Console.WriteLine("TransactionDetails successfully added!");
        }

        private static void IntroduceRules()
        {
            Console.WriteLine("Please enter interest rules details in <Date>|<RuleId>|<Rate in %> format");
            Console.WriteLine("(or enter blank to go back to the main menu):");
            Console.Write("> ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                return;

            var parts = input.Split('|');
            if (parts.Length != 3)
            {
                Console.WriteLine("Invalid input format. Please try again.");
                return;
            }

            var date = parts[0];
            var ruleId = parts[1];
            var rateStr = parts[2];

            if (!DateTime.TryParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                Console.WriteLine("Invalid date format. Please use YYYYMMdd.");
                return;
            }

            if (string.IsNullOrWhiteSpace(ruleId))
            {
                Console.WriteLine("Invalid rule ID. Rule ID cannot be empty.");
                return;
            }

            if (!decimal.TryParse(rateStr, out var rate) || rate <= 0 || rate >= 100)
            {
                Console.WriteLine("Invalid interest rate. Rate must be a positive number less than 100.");
                return;
            }

            var accountWithRule = accounts.FirstOrDefault(a => a.InterestRules.Any(r => r.RuleId == ruleId));
            if (accountWithRule != null)
            {
                var existingRule = accountWithRule.InterestRules.First(r => r.RuleId == ruleId);
                existingRule.Date = date;
                existingRule.Rate = rate;
            }
            else
            {
                var interestRule = new InterestRuleDetails { Date = date, RuleId = ruleId, Rate = rate };
                foreach (var account in accounts)
                {
                    account.InterestRules.Add(interestRule);
                }
            }
            Console.WriteLine("Interest rule successfully added!");
            PrintRules();
        }

        static void Print()
        {
            Console.WriteLine("Please enter account and month to generate the statement <Account>|<Month>");
            Console.WriteLine("(or enter blank to go back to the main menu):");
            Console.Write("> ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                return;

            var parts = input.Split('|');
            if (parts.Length != 2)
            {
                Console.WriteLine("Invalid input format. Please try again.");
                return;
            }

            var accountnum = parts[0];
            var month = parts[1];

            var account = accounts.FirstOrDefault(acc => acc.AccountNumber == accountnum);
            if (account == null)
            {
                Console.WriteLine("Account not found. Please check the account number.");
                return;
            }

            var monthstart = new DateTime(DateTime.Now.Year, int.Parse(month), 1);
            var monthend = monthstart.AddMonths(1).AddDays(-1);

            Console.WriteLine($"Account: {account.AccountNumber}");
            Console.WriteLine("Date     | Txn Id      | Type | Amount | Balance");

            var balance = 0m;
            var transactions = account.Transactions.OrderBy(t => t.Date).ToList();

            foreach (var transaction in transactions)
            {
                var txnDate = DateTime.ParseExact(transaction.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                if (txnDate >= monthstart && txnDate <= monthend)
                {
                    var interest = Interrest(account, txnDate);
                    balance += (transaction.Type == "D" ? transaction.Amount : -transaction.Amount) + interest;

                    Console.WriteLine($"{transaction.Date} | {transaction.TxnId} | {transaction.Type}    | {transaction.Amount,6:F2} | {balance,7:F2}");
                }
            }

    
            var finalday = monthstart.AddMonths(1).AddDays(-1);
            var interestmonth = Interrest(account, finalday);
            balance += interestmonth;

            Console.WriteLine($"{finalday:yyyyMMdd} |             | I    | {interestmonth,6:F2} | {balance,7:F2}\n");
        }
        public static decimal Interrest(Account account, DateTime date)
        {
            var calbalance = account.Transactions
                .Where(t => DateTime.ParseExact(t.Date, "yyyyMMdd", CultureInfo.InvariantCulture) <= date)
                .Sum(t => t.Type == "D" ? t.Amount : -t.Amount);

            var rulesList = account.InterestRules
                .Where(rule => DateTime.ParseExact(rule.Date, "yyyyMMdd", CultureInfo.InvariantCulture) <= date)
                .OrderByDescending(rule => DateTime.ParseExact(rule.Date, "yyyyMMdd", CultureInfo.InvariantCulture))
                .ToList();

            decimal interest = 0;
            foreach (var rule in rulesList)
            {
                var dayCount = (date - DateTime.ParseExact(rule.Date, "yyyyMMdd", CultureInfo.InvariantCulture)).Days;
                var rateforday = rule.Rate / 100 / 365;
                interest += calbalance * rateforday * dayCount;
            }

            return interest;
        }

        private static void PrintRules()
        {
            Console.WriteLine("Interest rules:");
            Console.WriteLine("Date     | RuleId | Rate (%)");

            var rules = accounts.SelectMany(acc => acc.InterestRules).ToList();
            var distinctrules = rules.GroupBy(r => r.RuleId)
                .Select(group => group.OrderByDescending(r => DateTime.ParseExact(r.Date, "yyyyMMdd", CultureInfo.InvariantCulture)).First())
                .OrderBy(rule => DateTime.ParseExact(rule.Date, "yyyyMMdd", CultureInfo.InvariantCulture))
                .ToList();

            foreach (var rule in distinctrules)
            {
                Console.WriteLine($"{rule.Date} | {rule.RuleId} | {rule.Rate:F2}");
            }
        }
    }



}