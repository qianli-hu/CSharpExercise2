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
    }
}

public abstract class BankAccount
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public abstract string GetAccountNumber();
}

public class SavingAccount : BankAccount
{
    private static int numberOfCustomers = 0;
    private static string accountType = "Saving";

    public double Balance { get; protected set; } = 0;

    public SavingAccount(string name, string? phoneNumber, double? balance) { 
        Name = name;
        PhoneNumber = (string)phoneNumber;
        Balance = (double)balance;
    }
    public override string GetAccountNumber()
    {
        numberOfCustomers++;
        return accountType + numberOfCustomers.ToString();
    }
    public void Deposit(double amount)
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
                Balance += amount;
                Console.WriteLine($"After a deposit of {amount}, the remaining balance is {Balance}");
                break;

        }
    }
    public void WithDrawl(double amount)
    {
       /* bankAccount.Balance -= (bankAccount.Balance > amount) ? amount : 0;
        Console.WriteLine($"After a withdrawl of {amount}, the remaining balance is {bankAccount.Balance}");*/
       if ( amount > Balance ) 
        {
            Console.WriteLine($"Withdrawl amount {amount} exceeds the current balance {Balance}");
        }
       else 
        {
            Balance -= amount;
            Console.WriteLine($"Withdrawl amount {amount} successful; remaining balance {Balance}");
        }
    }
    public void TransferInto(double amount, SavingAccount recipient)
    {
        if (amount > Balance)
        {
            Console.WriteLine("Insufficient funds, not able to transfer");
        }
        else
        {
            recipient.Balance += amount;
            Balance -= amount;
            Console.WriteLine($"Transfer completed. Now sender has balance {Balance},\n" +
                $"recipient has balance {recipient.Balance}");
        }
    }
}

public class CheckingAccount : BankAccount
{
    private static int numberOfCustomers = 0;
    private static string accountType = "Saving";
    public double OverDraftLimit { get; protected set; } = 0;

    public CheckingAccount(string name, string? phoneNumber, double overDraftLimit)
    {
        Name = name;
        PhoneNumber = (string)phoneNumber;
        OverDraftLimit = overDraftLimit;
    }

    public override string GetAccountNumber()
    {
        numberOfCustomers++;
        return accountType + numberOfCustomers.ToString();
    }


}

// Assume:
// 1. one needs a bank account to create a checking account.
// 2. Checking Account has a new account number


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
