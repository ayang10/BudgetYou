﻿using System;
using System.Collections.Generic;
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

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal Balance { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreationDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal ReconcileBalance { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual Household Household { get; set; }
    }

    public class Household
    {
        public Household()
        {
            this.Categories = new HashSet<Category>();
            this.Accounts = new HashSet<Account>();
            this.Budgets = new HashSet<Budget>();
            this.Members = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> Members { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
    }

    public class Transaction
    {


        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public bool Reconciled { get; set; }
        public int EntryId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal? ReconciledAmount { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Category Category { get; set; }
        public virtual Account Account { get; set; }

    }

    public class Category
    {
        public Category()
        {
            this.Households = new HashSet<Household>();
            this.Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Household> Households { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }

    public class Budget
    {
        public Budget()
        {
            this.BudgetItems = new HashSet<BudgetItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int HouseholdId { get; set; }

        public virtual ICollection<BudgetItem> BudgetItems { get; set; }
        public virtual Household Household { get; set; }
    }

    public class BudgetItem
    {
        public BudgetItem()
        {
            this.Budgets = new HashSet<Budget>();
        }

        public int Id { get; set; }
        public int CategoryId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal Amount { get; set; }

        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual Category Category { get; set; }
    }


    public class Invitation
    {
        public int Id { get; set; }
        public string ToEmail { get; set; }
        public int SendFromId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }


    public class Dashboard
    {
        public Dashboard()
        {
            this.Accounts = new HashSet<Account>();
            this.Households = new HashSet<Household>();
            this.Transactions = new HashSet<Transaction>();
            this.Categories = new HashSet<Category>();
            this.Budgets = new HashSet<Budget>();
            this.BudgetItems = new HashSet<BudgetItem>();
            this.Invitations = new HashSet<Invitation>();
        }

        public int Id { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Household> Households { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<BudgetItem> BudgetItems { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }

    }


}