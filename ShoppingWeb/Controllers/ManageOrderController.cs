﻿using ShoppingWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace ShoppingWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageOrderController : Controller
    {
        // GET: ManageOrder
        public ActionResult Index(int page = 1, int pagesize = 10)
        {
            //ViewBag.ResultMessage = TempData["ResultMessage"];
            // ViewBag.DeleteMessage = TempData["DeleteMessage"];
            using (CartsEntities db = new CartsEntities())
            { //抓取所有AspNetUsers中的資料，並且放入Models.ManageUser模型中
                var result = (from s in db.Order orderby s.Id select s);
                return View(result.ToPagedList(page, pagesize));
            }
        }

        public ActionResult Details(int id)
        {
            using (CartsEntities db = new CartsEntities())
            {
                //取得OrderId = 傳入Id的列表
                var result = (from s in db.OrderDetail where s.OrderId == id select s).ToList();
                //如果商品數為0代表有異常，導回Index
                if (result.Count == 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(result);
                }
            }

        }


        public ActionResult SearchByUserName(string UserName)
        {
            //儲存查出來的UserId
            string SearchUserId = null;
            using (UserEntities db = new UserEntities())
            {
                SearchUserId = (from s in db.AspNetUsers where s.UserName == UserName select s.UserName).FirstOrDefault();

            }
            //如果有找到
            if (!string.IsNullOrEmpty(SearchUserId))
            {
                using (CartsEntities db = new CartsEntities())
                {
                    var result = (from s in db.Order where s.UserId == SearchUserId select s).ToList();


                    return View("Index", result);
                }
            }
            else
            {
                return View("Index", new List<Order>());
            }

        }


    }
}