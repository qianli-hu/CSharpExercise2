/*
Bank Account:
Create a bank account class that has the following properties: account number, account holder name, balance. 
    Implement the following methods:
*****
Deposit(amount): adds the specified amount to the balance
Withdraw(amount): subtracts the specified amount from the balance
GetBalance(): returns the current balance
Transfer(amount, account): transfers the specified amount to the specified account
ToString(): returns a string representation of the account
*****
Use a switch statement to handle the different types of exceptions that may occur during account transactions, 
such as insufficient funds or invalid account numbers. Use inheritance to create a checking account subclass 
that allows for overdrafts, and implement the Transfer method so that overdrafts are allowed up to a certain limit.
Finally, use LINQ to implement a method that returns all accounts with a balance greater than a specified amount.
*/

using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

class Program
{
    static void Main(string[] args)
    {

        Console.WriteLine("Creating two accounts...");
        BankAccount account1 = new BankAccount("Judy", 100);
        BankAccount account2 = new BankAccount("Sandy");
        Console.WriteLine("Two accounts created, ");
        Console.WriteLine($"One with name {account1.Name}, balance {account1.Balance}, account number {account1.GetAccountNumber()}");
        Console.WriteLine($"The other with name {account2.Name} and balance {account2.Balance}, account number {account2.GetAccountNumber()}");
        Console.WriteLine("*****" );

        Console.WriteLine("Demonstrating Deposits...");
        BankService.Deposit(-100, account1);
        BankService.Deposit(78927492837, account1);
        BankService.Deposit(500, account2);
        Console.WriteLine("*****");

        Console.WriteLine("Demonstrating transfer method...");
        BankService.TransferInto(300, account1, account2);
        BankService.TransferInto(50, account1, account2);
        BankService.GetReport(account1);
        BankService.GetReport(account2);
        Console.WriteLine("*****");

        Console.WriteLine("Demonstrating GetBalance method on account1...");
        BankService.GetBalance(account1);
        Console.WriteLine("*****");

        /*     Questions:
             1. Better way to "link" account1 and account1Checking?
             2. Accessibility concerns for some of the methods in services
     */
        Console.WriteLine("Creating a checking account for account1...");
        CheckingAccount account1Checking = new CheckingAccount(account1, 200);
        Console.WriteLine($"Checking account created: name: {account1Checking.Name}, overdraft limit: {account1Checking.OverDraftLimit} \n" +
            $"Remaining balance: {account1Checking.Balance}, account number {account1Checking.GetAccountNumber()}");
        Console.WriteLine("*****");

        Console.WriteLine("Demonstrating spending on the checking account...");
        CheckingService.Purchase(300, account1Checking);
        Console.WriteLine("###");
        CheckingService.Purchase(200, account1Checking);
        Console.WriteLine("###");
        CheckingService.Purchase(200, account1Checking);
    }
}

public class BankAccount
{
    private static int currentNumberOfCustomers = 0;
    private int accountNumber = ++ currentNumberOfCustomers;
    public string Name { get; set; }
    public double Balance { get; set; }
    
    public BankAccount(string name, double? balance = 0)
    {
        Name = name;
        Balance = (double)balance;
    }
    public int GetAccountNumber()
    {
        return accountNumber;
    }
}


public class BankService
{
    static int reportCount = 0;
    public static void Deposit(double amount, BankAccount bankAccount)
    {
        switch (amount)
        {
            case <= 0:
                Console.WriteLine($"Input {amount} is negative. For deposit, Please input a positive number");
                break;
            case >= 5000000:
                Console.WriteLine($"Input amount {amount} too large for our small bank. Wells Fargo around the corner.");
                break;
            default:
                bankAccount.Balance += amount;
                Console.WriteLine($"After a deposit of {amount}, the remaining balance is {bankAccount.Balance}");
                break;

        }
    }
    public static void WithDrawl(double amount, BankAccount bankAccount)
    {
        bankAccount.Balance -= (bankAccount.Balance > amount) ? amount : 0;
        Console.WriteLine($"After a withdrawl of {amount}, the remaining balance is {bankAccount.Balance}");
    }
    
    public static void GetBalance(BankAccount bankAccount) 
    {
        Console.WriteLine($"Current balance is {bankAccount.Balance}");
    }
    public static void TransferInto(double amount, BankAccount accountOut, BankAccount accountInto) 
    {
        // bankAccount2.Balance += (amount <= bankAccount1.Balance) ? amount : 0;

        if (amount > accountOut.Balance)
        {
            Console.WriteLine("Insufficient funds, not able to transfer");
        }
        else
        {
            accountInto.Balance += amount;
            accountOut.Balance -= amount;
            Console.WriteLine("Transfer completed.");
        }
    }
    public static void GetReport(BankAccount bankAccount)
    {
        int reportNumber = ++ reportCount;
        string fileName = DateTime.Now.ToString("MMddyyyyHHmmss") + "_" + bankAccount.GetAccountNumber() + "_" + reportNumber + ".txt";
        string folderName = "Report";

            if (!Directory.Exists(folderName)) 
            {
                Directory.CreateDirectory(folderName);
            }

        string filePath = Path.Combine(folderName, fileName);



        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"Account Number: {bankAccount.GetAccountNumber()}");
        stringBuilder.AppendLine($"Name: {bankAccount.Name}");
        stringBuilder.AppendLine($"Balance: {bankAccount.Balance}");
        stringBuilder.AppendLine($"Date: " + DateTime.Now.ToString());

        using (StreamWriter writer = File.CreateText(filePath))
        {
            writer.Write(stringBuilder.ToString());
        }
        Console.WriteLine($"Report generated for account {bankAccount.GetAccountNumber()}");
    }

}

// Assume:
// 1. one needs a bank account to create a checking account.
// 2. Checking Account has a new account number
public class CheckingAccount : BankAccount
{
    public double OverDraftLimit { get; set; }
    public double StandingBalance { get; set; } = 0;
    public CheckingAccount(BankAccount bankaccount, double overDraftLimit) : base(bankaccount.Name, bankaccount.Balance)
    {
        OverDraftLimit = overDraftLimit;
    }
}

public class CheckingService : BankService
{
    public static void Purchase(double amount, CheckingAccount checkingAccount)
    {
        if (amount > checkingAccount.OverDraftLimit)
        {
            Console.WriteLine($"Error. Amount {amount} exceeds overdraft limit");
            return;
        }

        else if (amount + checkingAccount.StandingBalance> checkingAccount.Balance + checkingAccount.OverDraftLimit) {
            Console.WriteLine($"Error. Amount {amount} will result in insufficient fund in balance");
            return;
        }
        else
        {
            checkingAccount.StandingBalance += amount;
            Console.WriteLine($"Spending of {amount} success. Now checking has running balance of {checkingAccount.StandingBalance}, and remaining balance of {checkingAccount.Balance}");
        }
    }

    public static void Settle(CheckingAccount checkingAccount)
    {
        if (checkingAccount.StandingBalance < checkingAccount.Balance)
        {
            checkingAccount.Balance -= checkingAccount.StandingBalance;
            checkingAccount.StandingBalance = 0;
            Console.WriteLine($"Settled, the remaining balance is {checkingAccount.Balance}");
        }
        else
        {
            checkingAccount.Balance = 0;
            checkingAccount.StandingBalance -= checkingAccount.Balance;
            Console.WriteLine($"UNRESOLVED, the remaining standing balance is {checkingAccount.StandingBalance}");
        }
    }
}
