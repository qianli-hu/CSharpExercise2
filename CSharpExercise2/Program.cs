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
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Some examples to test the saving & checking account
        SavingAccount savingAccount1 = new SavingAccount("John", "555-1234", 1000);
        SavingAccount savingAccount2 = new SavingAccount("John", "333-1234");
        Console.WriteLine($"Accounts created;\n One account number {savingAccount1.AccountNumber}, another {savingAccount2.AccountNumber}");

        Console.WriteLine("\nPlease write your deposit amount: ");
        double deposit = double.Parse(Console.ReadLine());
        savingAccount1.Deposit(deposit);

        Console.Write("\nTesting transfer...");
        savingAccount1.TransferInto(2000, savingAccount2);

        CheckingAccount checkingAccount1 = new CheckingAccount("Paul", 300);
        Console.WriteLine($"\nChecking account created, with account number {checkingAccount1.AccountNumber} and limit {checkingAccount1.OverDraftLimit}");

        Console.WriteLine("\nTesting purchase and pay");
        checkingAccount1.Purchase(500);
        checkingAccount1.Purchase(200);
        checkingAccount1.Pay(50);
        double change = checkingAccount1.Pay(300);
        Console.WriteLine($"After payment, the change for the payment is {change}");



    }
}

public abstract class BankAccount
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
}

public class SavingAccount : BankAccount
{
    private static int numberOfCustomers = 0;
    private static string accountType = "Saving ";
    public double Balance { get; protected set; }
    public string AccountNumber { get; }

    public SavingAccount(string name, string? phoneNumber, double? balance = 0) {
        AccountNumber = accountType + (++numberOfCustomers).ToString();
        Name = name;
        PhoneNumber = (string)phoneNumber;
        Balance = (double)balance;
    }
    public void Deposit(double amount)
    {
        switch (amount)
        {
            case <= 0:
                Console.WriteLine($"Input {amount} is negative. Please deposit a positive amount.");
                break;
            case >= 5000000:
                Console.WriteLine($"Input amount {amount} too large for our small bank.");
                break;
            default:
                this.Balance += amount;
                Console.WriteLine($"After a deposit of {amount}, the remaining balance is {this.Balance}.");
                break;

        }
    }
    public void WithDrawl(double amount)
    {
       if ( amount > this.Balance ) 
        {
            Console.WriteLine($"Withdrawl amount {amount} exceeds the current balance {this.Balance}.");
        }
       else 
        {
            this.Balance -= amount;
            Console.WriteLine($"Withdrawl of amount {amount} successful; remaining balance {this.Balance}.");
        }
    }
    public void TransferInto(double amount, SavingAccount recipient)
    {
        if (amount > this.Balance)
        {
            Console.WriteLine("Insufficient funds, not able to transfer.");
        }
        else
        {
            recipient.Balance += amount;
            this.Balance -= amount;
            Console.WriteLine($"Transfer completed. Now sender has balance {this.Balance},\n" +
                $"recipient has balance {recipient.Balance}.");
        }
    }
}

public class CheckingAccount : BankAccount
{
    private static int numberOfCustomers = 0;
    private static string accountType = "Checking ";
    public double OverDraftLimit { get; protected set; } = 0;
    public double StandingBalance { get; protected set; } = 0;
    public double RemainingBalance
    {
        get { return this.OverDraftLimit - this.StandingBalance; }
    }
    public string AccountNumber { get; }
    public CheckingAccount(string name, double overDraftLimit, string? phoneNumber = null)
    {
        AccountNumber = accountType + (++numberOfCustomers).ToString();
        Name = name;
        PhoneNumber = (string)phoneNumber;
        OverDraftLimit = (double)overDraftLimit;
    }

    public void Purchase(double amount)
    {
        if (amount > this.RemainingBalance)
        {
            Console.WriteLine($"Purchase amount {amount} exceeds current remaining balance {this.RemainingBalance}.");
        }
        else
        {
            this.StandingBalance += amount;
            Console.WriteLine($"Purchase amount { amount} successful; current remaining balance {this.RemainingBalance}.");
        }
    }

    public double Pay(double amount)
    {
        if (amount >= this.StandingBalance)
        {
            double change = amount - this.StandingBalance;
            this.StandingBalance = 0;
            Console.WriteLine("Payment in full.");
            return change;
        }
        else
        {
            this.StandingBalance-= amount;
            Console.WriteLine($"Payment of {amount}; standing balance is {this.StandingBalance}.");
            return 0;
        }
    }
}

