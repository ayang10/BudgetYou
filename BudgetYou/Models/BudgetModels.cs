﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BudgetYou.Models
{

    public class Account
    {
        public Account()
        {
            this.Transactions = new HashSet<Transaction>();
        }
        public int Id { get; set; }
        public int HouseholdId { get; set; }
        public string Name { get; set; }
        public decimal WarningBalance { get; set; }


        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal Balance { get; set; }
       
        public DateTimeOffset CreationDate { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal ReconcileBalance { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual Household Household { get; set; }
     

        public Transaction GetTransactions { get; set; }
        
    }

    

    public class Household
    {
        public Household()
        {
            this.Categories = new HashSet<Category>();
            this.Accounts = new HashSet<Account>();
            this.Budgets = new HashSet<Budget>();
            this.Members = new HashSet<ApplicationUser>();
            this.Invitations = new HashSet<Invitation>();
            this.BudgetItems = new HashSet<BudgetItem>();
            this.Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }


        public virtual ICollection<ApplicationUser> Members { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<BudgetItem> BudgetItems { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }

    public class Transaction
    {


        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; }

        [Required]
        public bool Types { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public bool Reconciled { get; set; }
        public string EntryId { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal? ReconciledAmount { get; set; }

        public virtual ApplicationUser Entry { get; set; }
        public virtual Category Category { get; set; }
        public virtual Account Account { get; set; }

    }
    

    public class Category
    {
        public Category()
        {
            this.Households = new HashSet<Household>();
            this.BudgetItems = new HashSet<BudgetItem>();
            this.Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Household> Households { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<BudgetItem> BudgetItems { get; set; }
    }

    public class Budget
    {
        public Budget()
        {
            this.BudgetItems = new HashSet<BudgetItem>();
            this.Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int HouseholdId { get; set; }

        public virtual ICollection<BudgetItem> BudgetItems { get; set; }
        public virtual Household Household { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }

    public class BudgetItem
    {
        public BudgetItem()
        {
            this.Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public int BudgetId { get; set; }
        public int CategoryId { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal Amount { get; set; }

        public virtual Budget Budget { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }


    public class Invitation
    {
        public int Id { get; set; }
        public int HouseholdId { get; set; }
        public Guid JoinCode { get; set; }
        public string ToEmail { get; set; }
        public bool Joined { get; set; }

        public virtual Household Households { get; set; }
        public virtual ApplicationUser User { get; set; }
    }


    public class DashboardViewModels
    {
        public Household Households { get; set; }
        public ICollection<Invitation> Invitations { get; set; }
        public Budget Budgets { get; set; }
        public ICollection<Account> Accounts { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Category> Categories { get; set; }
        public BudgetItem BudgetItems { get; set; }

        public int GetBudgetId { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTimeOffset begin { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTimeOffset end { get; set; }

        
        public int setLowBalance { get; set; }

       
    }
    
    public class SendGridCredential
    {
        public int id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

}