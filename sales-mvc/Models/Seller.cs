﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace sales_mvc.Models
{
    public class Seller
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} size should be between {2} and {1}")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Birth Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Range(100, 100000, ErrorMessage = "{0} must be from {1} to {2}")]
        [Display(Name = "Base Salary")]
        [DataType(DataType.Currency)]
        public double BaseSalary { get; set; }
        
        public Department Department { get; set; }

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public Seller() { }

        public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }

        public void AddSale(SalesRecord saleRecord)
        {
            Sales.Add(saleRecord);
        }

        public void RemoveSale(SalesRecord saleRecord)
        {
            Sales.Remove(saleRecord);
        }

        public double TotalSales(DateTime initialDate, DateTime finalDate)
        {
            return Sales.Where(saleRecord => saleRecord.Date >= initialDate && saleRecord.Date <= finalDate).
                Sum(saleRecord => saleRecord.Amount);
        }
    }
}
