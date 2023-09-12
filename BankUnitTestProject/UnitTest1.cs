using NUnit.Framework;
using System;
using BankApplication;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

namespace BankUnitTestProject
{
    public class Tests
    {
        [TestFixture]
        public class AccountTests
        {
            //[Test]
            //public void InputTransactions_WithValidData_ShouldAddTransaction()
            //{
            //    // Arrange
            //    var bankingSystem = new Program(); // Assuming Program contains your banking logic

            //    // Act
            //    // Simulate user input for a valid transaction
            //    using (var sw = new StringWriter())
            //    {
            //        Console.SetOut(sw);
            //        Console.SetIn(new StringReader("20230626|AC001|D|100.00\n")); // Input a deposit
            //        Program.Main(null); // Run the banking system

            //        // Assert
            //        var output = sw.ToString();
            //        StringAssert.Contains("Transaction successfully added!", output);
            //    }
            //}

            //[Test]
            //public void PrintStatement_WithValidData_ShouldPrintStatement()
            //{
            //    // Arrange
            //    var bankingSystem = new Program(); // Assuming Program contains your banking logic

            //    // Act
            //    // Simulate user input for printing a statement
            //    using (var sw = new StringWriter())
            //    {
            //        Console.SetOut(sw);
            //        Console.SetIn(new StringReader("AC001|06\n")); // Input an account and month
            //        Program.Main(null); // Run the banking system

            //        // Assert
            //        var output = sw.ToString();
            //        StringAssert.Contains("Account: AC001", output);
            //        StringAssert.Contains("Date     | Txn Id      | Type | Amount | Balance", output);
            //    }
            //}


            [Test]
            public void TestInterestCalculation()
            {
                // Arrange
                var account = new Account
                {
                    // Initialize your Account object with Transactions and InterestRules
                   
                    Transactions = new List<TransactionDetails>
            {
                new TransactionDetails { Date = "20230101", Type = "D", Amount = 1000 },
                new TransactionDetails { Date = "20230105", Type = "W", Amount = 500 },
                new TransactionDetails { Date = "20230110", Type = "D", Amount = 200 },
            },
                    InterestRules = new List<InterestRuleDetails>
            {
                new InterestRuleDetails { Date = "20230101", Rate = Convert.ToDecimal(5.0) },
                new InterestRuleDetails { Date = "20230105", Rate = Convert.ToDecimal(4.0) },
                new InterestRuleDetails { Date = "20230109", Rate = Convert.ToDecimal(3.0) },
            }
                };

                var date = DateTime.ParseExact("20230115", "yyyyMMdd", CultureInfo.InvariantCulture);

                // Act
                decimal interest = Program.Interrest(account, date);
                interest = Math.Truncate(interest * 1000m) / 1000m;
                // Assert
                // The expected interest calculation depends on your specific logic and data.
                // Calculate the expected interest based on the provided data and rules.
                decimal expectedInterest = 2.454m;
                // Add your expected interest calculation logic here

                Assert.AreEqual(expectedInterest, interest, "Interest calculation is incorrect");
            }
        }


    
    }
}