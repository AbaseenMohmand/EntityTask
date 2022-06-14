﻿using System.ComponentModel.DataAnnotations;

namespace EntityTask.Models
{
    public class OrderItem
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        
        public int Quanity { get; set; }
    }
}