using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using NUnit.Framework;
using System.Reflection;
namespace BankApplication
{

    class UnitTestBankClass
    {
        private StringWriter consoleOutput;
        private TextWriter originalConsoleOutput;

        [Test]
        public void Initialize()
        {
            consoleOutput = new StringWriter();
            originalConsoleOutput = Console.Out;
            Console.SetOut(consoleOutput);
        }

        [Test]
        public void Cleanup()
        {
            consoleOutput.Dispose();
            Console.SetOut(originalConsoleOutput);
        }

        [Test]
        public void TestDepositAndWithdraw()
        {
            // Create an account
            Account account = new Account("AC001");

            // Deposit $100
            account.Deposit(DateTime.Now, 100);
            Assert.AreEqual(100, account.Balance);

            // Withdraw $50
            account.Withdraw(DateTime.Now, 50);
            Assert.AreEqual(50, account.Balance);
        }

        //[Test]
        //public void TestInvalidWithdrawal()
        //{
        //    // Create an account
        //    Account account = new Account("AC002");

        //    // Withdraw $50 without any deposit
        //    account.Withdraw(DateTime.Now, 50);
        //    Assert.AreEqual(0, account.Balance);

        //    string consoleOutputStr = consoleOutput.ToString();
        //    Assert.IsTrue(consoleOutputStr.Contains("Invalid transaction. Ensure the first transaction for an account is a deposit."));
        //}

        [Test]
        public void TestInterestCalculation()
        {
            // Create an account
           Account account = new Account("AC003");

            // Deposit $100
            account.Deposit(DateTime.Now, 100);

            // Define an interest rule
            InterestRule interestRule = new InterestRule(DateTime.Now, "RULE01", 5);
            InterestRule.AddRule(interestRule);

            // Apply interest
            account.ApplyInterest(DateTime.Now);
            Assert.AreEqual(105, account.Balance);
        }

        //[Test]
        //public void TestStatementPrinting()
        //{
        //    // Create an account
        //    Account account = new Account("AC004");

        //    // Deposit $100
        //    account.Deposit(DateTime.Now, 100);

        //    // Withdraw $50
        //    account.Withdraw(DateTime.Now, 50);

        //    // Capture console output while printing the statement
        //    using (StringWriter sw = new StringWriter())
        //    {
        //        Console.SetOut(sw);
        //        account.PrintStatement();

        //        string consoleOutputStr = sw.ToString();
        //        Assert.IsTrue(consoleOutputStr.Contains("Account: AC004"));
        //        Assert.IsTrue(consoleOutputStr.Contains("| W    | 50.00  | 50.00  |"));
        //        Assert.IsTrue(consoleOutputStr.Contains("| D    | 100.00 | 100.00 |"));
        //    }
        //}

        [Test]
        public void TestInterestRuleDefinitionAndPrinting()
        {
            // Define interest rules
            InterestRule interestRule1 = new InterestRule(DateTime.Now, "RULE01", 5);
            InterestRule interestRule2 = new InterestRule(DateTime.Now, "RULE02", 3);

            // Capture console output while printing interest rules
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
               // InterestRule.PrintInterestRules();

                string consoleOutputStr = sw.ToString();
                Assert.IsTrue(consoleOutputStr.Contains("Interest rules:"));
                Assert.IsTrue(consoleOutputStr.Contains("RULE01 | 5.00  |"));
                Assert.IsTrue(consoleOutputStr.Contains("RULE02 | 3.00  |"));
            }
        }
    }
}

