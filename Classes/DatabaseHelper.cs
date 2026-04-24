using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WpfBowlingClub.AppData;

namespace WpfBowlingClub.Classes
{
    public class DatabaseHelper : IDisposable
    {
        private Bawling_clubdbEntities1 db;

        public DatabaseHelper()
        {
            db = new Bawling_clubdbEntities1();
        }

        // === ТОВАРЫ ===
        public List<Products> GetProducts()
        {
            return db.Products
                .Include(p => p.Suppliers)
                .Include(p => p.Manufacturers)
                .ToList();
        }

        public void AddProduct(Products product)
        {
            db.Products.Add(product);
            db.SaveChanges();
        }

        public void UpdateProduct(Products product)
        {
            var old = db.Products.Find(product.Id);
            if (old != null)
            {
                if (old.Price != product.Price)
                {
                    db.PriceHistory.Add(new PriceHistory
                    {
                        ProductId = product.Id,
                        OldPrice = old.Price,
                        NewPrice = product.Price,
                        ChangeDate = DateTime.Now
                    });
                }
                db.Entry(old).CurrentValues.SetValues(product);
                db.SaveChanges();
            }
        }

        public void DeleteProduct(int id)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
            }
        }

        // === ЗАКАЗЫ ===
        public List<Orders> GetOrders()
        {
            return db.Orders
                .Include(o => o.Users)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();
        }

        public void UpdateOrderStatus(int orderId, int status)
        {
            var order = db.Orders.Find(orderId);
            if (order != null)
            {
                order.Status = status;
                if (status == 4) order.CompletedAt = DateTime.Now;
                db.SaveChanges();
            }
        }

        // === ПОСТАВЩИКИ ===
        public List<Suppliers> GetSuppliers()
        {
            return db.Suppliers.OrderBy(s => s.Name).ToList();
        }

        public void AddSupplier(Suppliers supplier)
        {
            db.Suppliers.Add(supplier);
            db.SaveChanges();
        }

        public void UpdateSupplier(Suppliers supplier)
        {
            var old = db.Suppliers.Find(supplier.Id);
            if (old != null)
            {
                db.Entry(old).CurrentValues.SetValues(supplier);
                db.SaveChanges();
            }
        }

        public void DeleteSupplier(int id)
        {
            var supplier = db.Suppliers.Find(id);
            if (supplier != null)
            {
                db.Suppliers.Remove(supplier);
                db.SaveChanges();
            }
        }

        // === ПРОИЗВОДИТЕЛИ ===
        public List<Manufacturers> GetManufacturers()
        {
            return db.Manufacturers.OrderBy(m => m.Name).ToList();
        }

        public void AddManufacturer(Manufacturers manufacturer)
        {
            db.Manufacturers.Add(manufacturer);
            db.SaveChanges();
        }

        public void UpdateManufacturer(Manufacturers manufacturer)
        {
            var old = db.Manufacturers.Find(manufacturer.Id);
            if (old != null)
            {
                db.Entry(old).CurrentValues.SetValues(manufacturer);
                db.SaveChanges();
            }
        }

        public void DeleteManufacturer(int id)
        {
            var manufacturer = db.Manufacturers.Find(id);
            if (manufacturer != null)
            {
                db.Manufacturers.Remove(manufacturer);
                db.SaveChanges();
            }
        }

        // === ПОЛЬЗОВАТЕЛИ ===

        public void SetUserStatus(int userId, int status)
        {
            var user = db.Users.Find(userId);
            if (user != null)
            {
                user.Status = status;
                db.SaveChanges();
            }
        }

        public List<Users> GetUsers()
        {
            // ВАЖНО: используйте Include для загрузки связанных данных
            return db.Users
                .Include(u => u.Roles)  // Загружаем Role
                .ToList();
        }

        public Users Login(string login, string password)
        {
            // Тоже добавляем Include для Role
            return db.Users
                .Include(u => u.Roles)  // ← Добавить
                .FirstOrDefault(u =>
                    (u.Email == login || u.Phone == login)
                    && u.Password == password
                    && u.Status == 1);
        }

        // === ИСТОРИЯ ЦЕН ===
        public List<PriceHistory> GetPriceHistory(int productId)
        {
            return db.PriceHistory
                .Where(p => p.ProductId == productId)
                .OrderByDescending(p => p.ChangeDate)
                .ToList();
        }

        public void Dispose()
        {
            db?.Dispose();
        }
    }
}