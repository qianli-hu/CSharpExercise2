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

class Program
{
    static void Main(string[] args)
    {
        BankAccount account1 = new BankAccount("Judy", 100);
        BankAccount account2 = new BankAccount("Sandy");

        BankService.Deposit(-100, account1);
        BankService.Deposit(200, account2);
        BankService.GetBalance(account1);
        BankService.GetReport(account1);

        BankService.TransferInto(500, account1, account2);
        BankService.TransferInto(50, account1, account2);
        BankService.GetReport(account1);

    }
}

public class BankAccount
{
    private static int currentNumberOfCustomers = 0;
    private int accountNumber = ++ currentNumberOfCustomers;
    public int AccountNumber => accountNumber;
    public string Name { get; set; }
    public double Balance { get; set; }
    
    public BankAccount(string name, double? balance = 0)
    {
        Name = name;
        Balance = (double)balance;
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
                Console.WriteLine("For deposit, Please input a positive number");
                break;
            case >= 5000000:
                Console.WriteLine("Small bank, not capable of handling large amount of deposit");
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
    public static void TransferInto(double amount, BankAccount bankAccount1, BankAccount bankAccount2) 
    {
        // bankAccount2.Balance += (amount <= bankAccount1.Balance) ? amount : 0;

        if (amount > bankAccount1.Balance)
        {
            Console.WriteLine("Insufficient funds, not able to transfer");
        }
        else
        {
            bankAccount2.Balance += amount;
            bankAccount1.Balance -= amount;
            Console.WriteLine("Transfer completed.");
        }
    }
    public static void GetReport(BankAccount bankAccount)
    {
        int reportNumber = ++ reportCount;
        string fileName = DateTime.Now.ToString("MMddyyyyHHmmss") + "_" + bankAccount.AccountNumber + "_" + reportNumber;
        string folderName = "Report";

            if (!Directory.Exists(folderName)) 
            {
                Directory.CreateDirectory(folderName);
            }

        string filePath = Path.Combine(folderName, fileName);



        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"Account Number: {bankAccount.AccountNumber}");
        stringBuilder.AppendLine($"Name: {bankAccount.Name}");
        stringBuilder.AppendLine($"Balance: {bankAccount.Balance}");
        stringBuilder.AppendLine($"Date: " + DateTime.Now.ToString());

        using (StreamWriter writer = File.CreateText(filePath))
        {
            writer.Write(stringBuilder.ToString());
        }
        Console.WriteLine($"Report generated for account {bankAccount.AccountNumber}");
    }

}

