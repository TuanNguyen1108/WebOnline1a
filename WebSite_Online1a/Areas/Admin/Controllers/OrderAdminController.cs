﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Website_Online.Areas.Admin.Models.Authentication;
using WebSite_Online1a.Models;

namespace WebSite_Online1a.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authentication]
    public class OrderAdminController : Controller
    {
        private readonly WebOnline1Context _context;
        private readonly ILogger<WebOnline1Context> _logger;
        public OrderAdminController(WebOnline1Context context, ILogger<WebOnline1Context> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Admin/OrderAdmin
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("This is an information log.");
            _logger.LogWarning("This is a warning log.");
            _logger.LogError("This is an error log.");
            // hiện tên khi đăng nhập
            ViewBag.UserName = HttpContext.Session.GetString("HoTenAdmin");

            var webOnline1Context = _context.Orders.OrderByDescending(x => x.OderDate).Include(o => o.Account);
            return View(await webOnline1Context.ToListAsync());
        }

        // GET: Admin/OrderAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Account)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            var listchitietdonhang = _context.OderDetails
                .Include(x => x.Product)
                .Include(x => x.PriceNavigation)
                .Include(x => x.Order)
                .Where(x => x.OrderId == id)
                .Take(10)
                .ToList();
            ViewBag.ChiTietDonHang = listchitietdonhang;

            return View(order);
        }

        // GET: Admin/OrderAdmin/Create
        /*public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId");
            return View();
        }

        // POST: Admin/OrderAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,AccountId,FullName,Address,OderDate,TotalMoney,Note,OrderStatusId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", order.AccountId);
            return View(order);
        }*/

        // GET: Admin/OrderAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            var statusOptions = new List<OrderStatusOption>
            {
                new OrderStatusOption { Value = 1, TrangThaiDonHang = "Chờ Xác Nhận" },
                new OrderStatusOption { Value = 2, TrangThaiDonHang = "Đã Duyệt" },
                new OrderStatusOption { Value = 3, TrangThaiDonHang = "Đang Vận Chuyển" },
                new OrderStatusOption { Value = 4, TrangThaiDonHang = "Đã Giao" },
            };
            ViewBag.StatusOptions = statusOptions;
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", order.AccountId);
            return View(order);
        }

        // POST: Admin/OrderAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,AccountId,FullName,Phone,Address,OderDate,TotalMoney,Note,OrderStatusId,ShipDate,DateReceived,Payments")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (order.OrderStatusId == 1 || order.OrderStatusId == 2)
                    {
                        order.Payments = 1;
                        order.ShipDate = null;
                        order.DateReceived = null;
                    }
                    if (order.OrderStatusId == 3)
                    {
                        order.Payments = 1;
                        order.ShipDate = DateTime.Now;
                        order.DateReceived = null;
                    }
                    if (order.OrderStatusId == 4)
                    {
                        order.Payments = 2;
                        order.DateReceived = DateTime.Now;
                    }

                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", order.AccountId);
            return View(order);
        }

        // GET: Admin/OrderAdmin/Delete/5
        /*public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Account)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }*/

        /*// POST: Admin/OrderAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'WebOnline1Context.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
