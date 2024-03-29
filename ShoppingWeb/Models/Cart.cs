﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace ShoppingWeb.Models
{
    [Serializable] //序列化
    public class Cart : IEnumerable<CartItem> //購物車類別
    {

        //建構值
        public Cart()
        {
            this.cartItems = new List<CartItem>();
        }
        //List實體化
        private List<CartItem> cartItems;

        //商品總價
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C2}")]
        public decimal TotoalAmount
        {
            get
            {
                decimal totalamount = 0.0m;
                foreach (var cartItem in this.cartItems)
                {
                    totalamount = Math.Round(totalamount + cartItem.Amount, 0);
                }
                return totalamount;
            }
        }

        //取得購物車中商品總數
        public int Count
        {
            get
            {
                return this.cartItems.Count;
            }
        }

        //新增一筆product，使用productId
        public bool AddProduct(int ProductId)
        {
            var FindItem = this.cartItems.Where(s => s.Id == ProductId).Select(s => s).FirstOrDefault(); //取出s的資料 當CartItem.Id = ProductId

            if (FindItem == default(CartItem))
            {
                using (CartsEntities db = new CartsEntities())
                {
                    var product = (from s in db.Product where s.Id == ProductId select s).FirstOrDefault();

                    if (product != default(Product))
                    {
                        this.AddProduct(product);
                    }

                }
            }
            else
            {
                FindItem.Quantity += 1;
            }
            return true;
        }

        public bool AddProduct(Product product)
        {

            //將Product轉化成CartItem
            var CartItem = new CartItem()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                DefaultImgURL = product.DefaultImageURL,
                Quantity = 1
            };

            this.cartItems.Add(CartItem);
            //加入CartItem至購物車
            return true;
        }



        public bool RemoveProduct(int ProductId)
        {

            //將Product轉化成CartItem
            var FindItem = this.cartItems.Where(s => s.Id == ProductId).Select(s => s).FirstOrDefault(); //取出s的資料 當CartItem.Id = ProductId
            if (FindItem != default(CartItem))
            {
                this.cartItems.Remove(FindItem);
            }
            //加入CartItem至購物車
            return true;
        }


        public bool ClearCart()
        {
            this.cartItems.Clear();
            return true;
        }




        //把商品轉換成OrderDetail的list
        public List<Models.OrderDetail> ToOrderDetailList(int orderId)
        {
            var result = new List<OrderDetail>();

            foreach (var cartitem in this.cartItems)
            {
                result.Add(new OrderDetail()
                {
                    Name = cartitem.Name,
                    Price = cartitem.Price,
                    Quantity = cartitem.Quantity,
                    OrderId = orderId

                });
            }
            return result;
        }

        #region IEnumerator

        IEnumerator<CartItem> IEnumerable<CartItem>.GetEnumerator()
        {
            return this.cartItems.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.cartItems.GetEnumerator();
        }

        #endregion



    }

}



