using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AutoMapper;

namespace EFCoreProject
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Variables for storing elements of the dynamic window "Row"
        /// </summary>
        private Dictionary<int, TextBox>? createTextBox;
        private Dictionary<int, RowDefinition>? DeleteRowDefinitions;
        private Dictionary<int, Label>? DeleteLabel;
        /// <summary>
        /// Initialize "MainWindow"
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Filling the ListBox(Selected Table) with table names and hiding the dynamic "Row" window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (OnlineShopContext db = new OnlineShopContext())
            {
                var tables = db.Model.GetEntityTypes()
                    .Select(t => t.GetTableName())
                    .Distinct()
                    .ToList();
                foreach (var table in tables!)
                {
                    SelectTable.Items.Add(table);
                }
            }
            Row.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Calling Load Table() method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadTable();
        }
        /// <summary>
        /// Hiding the dynamic window "Row", calling "AddRowInTable" method, calling "DeleteDinamicList" method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Row.Visibility = Visibility.Hidden;
            AddRowInTable(false);
            DeleteDinamicList();
        }
        /// <summary>
        /// Adding data to a new row of the table by clicking on the "Add" button in the "Row" dynamic window, hiding the dynamic window, calling "AddRowInTable" and "DeleteDinamicList" methods
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (createTextBox is not null)
            {
                using (OnlineShopContext db = new OnlineShopContext())
                {
                    switch (SelectTable.SelectedItem)
                    {
                        case "Client":
                            try
                            {
                                db.Database.ExecuteSqlRaw(
                                    $"INSERT INTO [Client] (FirstName, Surname, Patronymic, Address, PhoneNumber) VALUES (" +
                                    $"'{createTextBox[1].Text}', " +
                                    $"'{createTextBox[2].Text}', " +
                                    $"'{createTextBox[3].Text}', " +
                                    $"'{createTextBox[4].Text}', " +
                                    $"'{createTextBox[5].Text}');");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "Delivery":
                            try
                            {
                                db.Database.ExecuteSqlRaw(
                                    $"INSERT INTO [Delivery] (DateDelivery, Remark, Supplier_Id) VALUES ('{createTextBox[1].Text}', '{createTextBox[2].Text}', '{createTextBox[3].Text}');");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "Delivery_Product":
                            try
                            {
                                db.Database.ExecuteSqlRaw(
                                    $"INSERT INTO [Delivery_Product] (Delivery_Id, Product_Id) VALUES (" +
                                    $"'{createTextBox[0].Text}', " +
                                    $"'{createTextBox[1].Text}');");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "Order":
                            try
                            {
                                db.Database.ExecuteSqlRaw(
                                    $"INSERT INTO [Order] (DateOrder, Count, PaymentType, Remark, Client_Id) VALUES (" +
                                    $"'{createTextBox[1].Text}', " +
                                    $"'{createTextBox[2].Text}', " +
                                    $"'{createTextBox[3].Text}', " +
                                    $"'{createTextBox[4].Text}', " +
                                    $"'{createTextBox[5].Text}');");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "Product":
                            try
                            {
                                db.Database.ExecuteSqlRaw(
                                    $"INSERT INTO [Product] (Name, Price, UnitMeasurement_Id, Description, ProductGroup_Id) VALUES (" +
                                    $"'{createTextBox[1].Text}', " +
                                    $"'{createTextBox[2].Text}', " +
                                    $"'{createTextBox[3].Text}', " +
                                    $"'{createTextBox[4].Text}', " +
                                    $"'{createTextBox[5].Text}');");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "ProductGroup":
                            try
                            {
                                db.Database.ExecuteSqlRaw(
                                    $"INSERT INTO [ProductGroup] (Name) VALUES (" +
                                    $"'{createTextBox[1].Text}');");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "Product_Order":
                            try
                            {
                                db.Database.ExecuteSqlRaw(
                                    $"INSERT INTO [Product_Order] (Product_Id, Order_Id) VALUES (" +
                                    $"'{createTextBox[0].Text}', " +
                                    $"'{createTextBox[1].Text}');");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "Supplier":
                            try
                            {
                                db.Database.ExecuteSqlRaw(
                                    $"INSERT INTO [Supplier] (Name, Address, PhoneNumber) VALUES (" +
                                    $"'{createTextBox[1].Text}', " +
                                    $"'{createTextBox[2].Text}', " +
                                    $"'{createTextBox[3].Text}');");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "UnitMeasurement":
                            try
                            {
                                db.Database.ExecuteSqlRaw(
                                    $"INSERT INTO [UnitMeasurement] (Name) VALUES (" +
                                    $"'{createTextBox[1].Text}');");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                    }
                }
                Row.Visibility = Visibility.Hidden;
                AddRowInTable(false);
            }
            DeleteDinamicList();
        }
        /// <summary>
        /// Changing the data in the selected row of the table by clicking on the "Edit" button in the "Row" dynamic window, hiding the dynamic window, calling "AddRowInTable" and "DeleteDinamicList" methods
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (createTextBox is not null)
            {
                using (OnlineShopContext db = new OnlineShopContext())
                {
                    switch (SelectTable.SelectedItem)
                    {
                        case "Client":
                            try
                            {
                                db.Database.ExecuteSqlRaw(
                                    $"UPDATE [Client] SET FirstName = '{createTextBox[1].Text}', Surname = '{createTextBox[2].Text}', Patronymic = '{createTextBox[3].Text}', Address = '{createTextBox[4].Text}', PhoneNumber = '{createTextBox[5].Text}' WHERE ID = {((DTO.Client)DataBase.SelectedItem).Id};");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "Delivery":
                            try
                            {
                                db.Database.ExecuteSqlRaw(
                                    $"UPDATE [Delivery] SET DateDelivery = '{createTextBox[1].Text}', Remark = '{createTextBox[2].Text}', Supplier_Id = '{createTextBox[3].Text}' WHERE ID = {((DTO.Delivery)DataBase.SelectedItem).Id};");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "Delivery_Product":
                            try
                            {
                                db.Database.ExecuteSqlRaw(
                                    $"UPDATE [Delivery_Product] SET Delivery_Id = '{createTextBox[1].Text}', Product_Id = '{createTextBox[2].Text}' WHERE Delivery_Id = {((DTO.DeliveryProduct)DataBase.SelectedItem).DeliveryId};");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "Order":
                            try
                            {
                                db.Database.ExecuteSqlRaw(
                                    $"UPDATE [Order] SET DateOrder = '{createTextBox[1].Text}', Count = '{createTextBox[2].Text}', PaymentType = '{createTextBox[3].Text}', Remark = '{createTextBox[4].Text}', Client_Id = '{createTextBox[5].Text}' WHERE Id = {((DTO.Order)DataBase.SelectedItem).Id};");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "Product":
                            try
                            {
                                db.Database.ExecuteSqlRaw(
                                    $"UPDATE [Product] SET Name = '{createTextBox[1].Text}', Price = '{createTextBox[2].Text}', UnitMeasurement_Id = '{createTextBox[3].Text}', Description = '{createTextBox[4].Text}', ProductGroup_Id = '{createTextBox[5].Text}' WHERE Id = {((DTO.Product)DataBase.SelectedItem).Id};");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "ProductGroup":
                            try
                            {
                                db.Database.ExecuteSqlRaw(
                                    $"UPDATE [ProductGroup] SET Name = '{createTextBox[1].Text}' WHERE Id = {((DTO.ProductGroup)DataBase.SelectedItem).Id};");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "Product_Order":
                            try
                            {
                                db.Database.ExecuteSqlRaw(
                                    $"UPDATE [Product_Order] SET Name = '{createTextBox[1].Text}', Order_Id = '{createTextBox[2].Text}' WHERE Product_Id = {((DTO.ProductOrder)DataBase.SelectedItem).ProductId};");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "Supplier":
                            try
                            {
                                db.Database.ExecuteSqlRaw(
                                    $"UPDATE [Supplier] SET Name = '{createTextBox[1].Text}', Address = '{createTextBox[2].Text}', PhoneNumber = '{createTextBox[3].Text}' WHERE Id = {((DTO.Supplier)DataBase.SelectedItem).Id};");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "UnitMeasurement":
                            try
                            {
                                db.Database.ExecuteSqlRaw(
                                    $"UPDATE [UnitMeasurement] SET Name = '{createTextBox[1].Text}' WHERE Id = {((DTO.UnitMeasurement)DataBase.SelectedItem).Id};");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                    }
                }
                Row.Visibility = Visibility.Hidden;
                AddRowInTable(false);
            }
            DeleteDinamicList();
        }
        /// <summary>
        /// Calling the "AddRowInTable" and "DinamicListAddRows" methods
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddLineButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectTable.SelectedItems.Count > 0)
            {
                AddRowInTable(true);
                DinamicListAddRows();
            }
            else
            {
                MessageBox.Show("Select table");
            }
        }
        /// <summary>
        /// Calling the "AddRowInTable" and "DinamicListEditRows" methods, inserting data from the selected row of the table into the fields of the dynamic Row window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditLineButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectTable.SelectedItems.Count > 0)
            {
                if (DataBase.SelectedItems.Count > 0)
                {
                    AddRowInTable(true);
                    DinamicListEditRows();
                    if (DataBase.SelectedItem is not null)
                    {
                        for (int i = 0; i < createTextBox!.Count; i++)
                        {
                            using (OnlineShopContext db = new OnlineShopContext())
                            {
                                switch (SelectTable.SelectedItem)
                                {
                                    case "Client":
                                        createTextBox[0].Text = Convert.ToString(((DTO.Client)DataBase.SelectedItem).Id);
                                        createTextBox[1].Text = Convert.ToString(((DTO.Client)DataBase.SelectedItem).FirstName);
                                        createTextBox[2].Text = Convert.ToString(((DTO.Client)DataBase.SelectedItem).Surname);
                                        createTextBox[3].Text = Convert.ToString(((DTO.Client)DataBase.SelectedItem).Patronymic);
                                        createTextBox[4].Text = Convert.ToString(((DTO.Client)DataBase.SelectedItem).Address);
                                        createTextBox[5].Text = Convert.ToString(((DTO.Client)DataBase.SelectedItem).PhoneNumber);
                                        break;
                                    case "Delivery":
                                        createTextBox[0].Text = Convert.ToString(((DTO.Delivery)DataBase.SelectedItem).Id);
                                        createTextBox[1].Text = Convert.ToString(((DTO.Delivery)DataBase.SelectedItem).DateDelivery);
                                        createTextBox[2].Text = Convert.ToString(((DTO.Delivery)DataBase.SelectedItem).Remark);
                                        createTextBox[3].Text = Convert.ToString(((DTO.Delivery)DataBase.SelectedItem).SupplierId);
                                        break;
                                    case "Delivery_Product":
                                        createTextBox[0].Text = Convert.ToString(((DTO.DeliveryProduct)DataBase.SelectedItem).DeliveryId);
                                        createTextBox[1].Text = Convert.ToString(((DTO.DeliveryProduct)DataBase.SelectedItem).ProductId);
                                        break;
                                    case "Order":
                                        createTextBox[0].Text = Convert.ToString(((DTO.Order)DataBase.SelectedItem).Id);
                                        createTextBox[1].Text = Convert.ToString(((DTO.Order)DataBase.SelectedItem).DateOrder);
                                        createTextBox[2].Text = Convert.ToString(((DTO.Order)DataBase.SelectedItem).Count);
                                        createTextBox[3].Text = Convert.ToString(((DTO.Order)DataBase.SelectedItem).PaymentType);
                                        createTextBox[4].Text = Convert.ToString(((DTO.Order)DataBase.SelectedItem).Remark);
                                        createTextBox[5].Text = Convert.ToString(((DTO.Order)DataBase.SelectedItem).ClientId);
                                        break;
                                    case "Product":
                                        createTextBox[0].Text = Convert.ToString(((DTO.Product)DataBase.SelectedItem).Id);
                                        createTextBox[1].Text = Convert.ToString(((DTO.Product)DataBase.SelectedItem).Name);
                                        createTextBox[2].Text = Convert.ToString(((DTO.Product)DataBase.SelectedItem).Price);
                                        createTextBox[3].Text = Convert.ToString(((DTO.Product)DataBase.SelectedItem).UnitMeasurementId);
                                        createTextBox[4].Text = Convert.ToString(((DTO.Product)DataBase.SelectedItem).Description);
                                        createTextBox[5].Text = Convert.ToString(((DTO.Product)DataBase.SelectedItem).ProductGroupId);
                                        break;
                                    case "ProductGroup":
                                        createTextBox[0].Text = Convert.ToString(((DTO.ProductGroup)DataBase.SelectedItem).Id);
                                        createTextBox[1].Text = Convert.ToString(((DTO.ProductGroup)DataBase.SelectedItem).Name);
                                        break;
                                    case "Product_Order":
                                        createTextBox[0].Text = Convert.ToString(((DTO.ProductOrder)DataBase.SelectedItem).ProductId);
                                        createTextBox[1].Text = Convert.ToString(((DTO.ProductOrder)DataBase.SelectedItem).OrderId);
                                        break;
                                    case "Supplier":
                                        createTextBox[0].Text = Convert.ToString(((DTO.Supplier)DataBase.SelectedItem).Id);
                                        createTextBox[1].Text = Convert.ToString(((DTO.Supplier)DataBase.SelectedItem).Name);
                                        createTextBox[2].Text = Convert.ToString(((DTO.Supplier)DataBase.SelectedItem).Address);
                                        createTextBox[3].Text = Convert.ToString(((DTO.Supplier)DataBase.SelectedItem).PhoneNumber);
                                        break;
                                    case "UnitMeasurement":
                                        createTextBox[0].Text = Convert.ToString(((DTO.UnitMeasurement)DataBase.SelectedItem).Id);
                                        createTextBox[1].Text = Convert.ToString(((DTO.UnitMeasurement)DataBase.SelectedItem).Name);
                                        break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Select row");
                }
            }
            else
            {
                MessageBox.Show("Select table");
            }
        }
        /// <summary>
        /// Deleting a selected table row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteLineButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataBase.SelectedItem is not null)
            {
                using (OnlineShopContext db = new OnlineShopContext())
                {
                    switch (SelectTable.SelectedItem)
                    {
                        case "Client":
                            try
                            {
                                db.Database.ExecuteSqlRaw($"DELETE FROM [Client] WHERE ID = {((DTO.Client)DataBase.SelectedItem).Id};");
                            }
                            catch(Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "Delivery":
                            try
                            {
                                db.Database.ExecuteSqlRaw($"DELETE FROM [Delivery] WHERE ID = {((DTO.Delivery)DataBase.SelectedItem).Id};");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "Delivery_Product":
                            try
                            {
                                db.Database.ExecuteSqlRaw($"DELETE FROM [Delivery_Product] WHERE ProductId = {((DTO.DeliveryProduct)DataBase.SelectedItem).ProductId};");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "Order":
                            try
                            {
                                db.Database.ExecuteSqlRaw($"DELETE FROM [Order] WHERE ID = {((DTO.Order)DataBase.SelectedItem).Id};");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "Product":
                            try
                            {
                                db.Database.ExecuteSqlRaw($"DELETE FROM [Product] WHERE ID = {((DTO.Product)DataBase.SelectedItem).Id};");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "ProductGroup":
                            try
                            {
                                db.Database.ExecuteSqlRaw($"DELETE FROM [ProductGroup] WHERE ID = {((DTO.ProductGroup)DataBase.SelectedItem).Id};");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "Product_Order":
                            try
                            {
                                db.Database.ExecuteSqlRaw($"DELETE FROM [Product_Order] WHERE ProductId = {((DTO.ProductOrder)DataBase.SelectedItem).ProductId};");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "Supplier":
                            try
                            {
                                db.Database.ExecuteSqlRaw($"DELETE FROM [Supplier] WHERE ID = {((DTO.Supplier)DataBase.SelectedItem).Id};");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                        case "UnitMeasurement":
                            try
                            {
                                db.Database.ExecuteSqlRaw($"DELETE FROM [UnitMeasurement] WHERE ID = {((DTO.UnitMeasurement)DataBase.SelectedItem).Id};");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException is not null)
                                {
                                    MessageBox.Show(ex.InnerException!.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            LoadTable();
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Select row");
            }
        }
        /// <summary>
        /// Clearing the table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearTable_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) { }
            else
            {
                if (SelectTable.SelectedItems.Count > 0)
                {
                    using (OnlineShopContext db = new OnlineShopContext())
                    {
                        try
                        {
                            db.Database.ExecuteSqlRaw($"DELETE FROM '{SelectTable.SelectedItem}';"); 
                            db.Database.ExecuteSqlRaw($"DELETE FROM sqlite_sequence WHERE name = '{SelectTable.SelectedItem}';");

                        }
                        catch (Exception ex)
                        {
                            if (ex.InnerException is not null)
                            {
                                MessageBox.Show(ex.InnerException!.Message);
                            }
                            else
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        LoadTable();
                    }
                }
                else
                {
                    MessageBox.Show("Select table");
                }
            }
        }
        /// <summary>
        /// Exiting the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        /// <summary>
        /// Adding fields to the dynamic Row window whose number is equal to the number of columns in the table and the "Edit" button
        /// </summary>
        private void DinamicListEditRows()
        {
            Row.Visibility = Visibility.Visible;
            createTextBox = new Dictionary<int, TextBox>();
            DeleteRowDefinitions = new Dictionary<int, RowDefinition>();
            DeleteLabel = new Dictionary<int, Label>();

            Button button = new Button();
            Row.Children.Add(button);
            button.Name = "Edit";
            button.Content = "Edit";
            button.Width = 100;
            button.Height = 30;
            Grid.SetColumn(button, 3);
            Grid.SetRow(button, 0);
            button.Click += new RoutedEventHandler(Edit_Click);
            button.Margin = new Thickness(10, 0, 0, 0);

            Row.UpdateLayout();

            for (int i = 0; i < DataBase.Columns.Count; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Name = $"{DataBase.Columns[i].Header}" + "Row";
                DeleteRowDefinitions.Add(i, row);
                Row.RowDefinitions.Add(row);

                TextBox textBox = new TextBox();
                if (i == 0) { textBox.IsEnabled = false; }
                Row.Children.Add(textBox);
                textBox.Name = $"{DataBase.Columns[i].Header}" + "TextBox";
                textBox.Width = 450;
                textBox.Height = 30;
                textBox.VerticalContentAlignment = VerticalAlignment.Center;
                textBox.FontSize = (double)14;
                Grid.SetColumn(textBox, 2);
                Grid.SetRow(textBox, i);
                createTextBox.Add(i, textBox);

                Label label = new Label();
                Row.Children.Add(label);
                label.Height = 30;
                label.Content = (string)DataBase.Columns[i].Header;
                Grid.SetColumn(label, 1);
                Grid.SetRow(label, i);
                label.Foreground = Brushes.White;
                DeleteLabel.Add(i, label);
            }
        }
        /// <summary>
        /// Adding fields to the dynamic Row window whose number is equal to the number of columns in the table and the "Add" button
        /// </summary>
        private void DinamicListAddRows()
        {
            Row.Visibility = Visibility.Visible;
            createTextBox = new Dictionary<int, TextBox>();
            DeleteRowDefinitions = new Dictionary<int, RowDefinition>();
            DeleteLabel = new Dictionary<int, Label>();

            Button button = new Button();
            Row.Children.Add(button);
            button.Name = "Add";
            button.Content = "Add";
            button.Width = 100;
            button.Height = 30;
            Grid.SetColumn(button, 3);
            Grid.SetRow(button, 0);
            button.Click += new RoutedEventHandler(Add_Click);
            button.Margin = new Thickness(10, 0, 0, 0);

            Row.UpdateLayout();

            for (int i = 0; i < DataBase.Columns.Count; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Name = $"{DataBase.Columns[i].Header}" + "Row";
                DeleteRowDefinitions.Add(i, row);
                Row.RowDefinitions.Add(row);

                TextBox textBox = new TextBox();
                if (i == 0)
                { textBox.IsEnabled = false; }
                if (SelectTable.SelectedItem.ToString() == "Delivery_Product" || SelectTable.SelectedItem.ToString() == "Product_Order")
                {textBox.IsEnabled = true;}
                Row.Children.Add(textBox);
                textBox.Name = $"{DataBase.Columns[i].Header}" + "TextBox";
                textBox.Width = 450;
                textBox.Height = 30;
                textBox.VerticalContentAlignment = VerticalAlignment.Center;
                textBox.FontSize = (double)14;
                Grid.SetColumn(textBox, 2);
                Grid.SetRow(textBox, i);
                createTextBox.Add(i, textBox);

                Label label = new Label();
                Row.Children.Add(label);
                label.Height = 30;
                label.Content = (string)DataBase.Columns[i].Header;
                Grid.SetColumn(label, 1);
                Grid.SetRow(label, i);
                label.Foreground = Brushes.White;
                DeleteLabel.Add(i, label);
            }
        }
        /// <summary>
        /// Clearing the "Row" dynamic window
        /// </summary>
        private void DeleteDinamicList()
        {
            for (int i = 0; i < DataBase.Columns.Count; i++)
            {
                Row.RowDefinitions.Remove(DeleteRowDefinitions![i]);
                Row.Children.Remove(DeleteLabel![i]);
                Row.Children.Remove(createTextBox![i]);
            }
        }
        /// <summary>
        /// Disabling and enabling certain buttons in the program
        /// </summary>
        /// <param name="enable"></param>
        private void AddRowInTable(bool enable)
        {
            if (enable)
            {
                SelectTable.IsEnabled = false;
                AddLineButton.IsEnabled = false;
                EditLineButton.IsEnabled = false;
                DeleteLineButton.IsEnabled = false;
                ClearTable.IsEnabled = false;
                Exit.IsEnabled = false;
            }
            else if (enable == false)
            {
                SelectTable.IsEnabled = true;
                AddLineButton.IsEnabled = true;
                EditLineButton.IsEnabled = true;
                DeleteLineButton.IsEnabled = true;
                ClearTable.IsEnabled = true;
                Exit.IsEnabled = true;
            }
        }
        /// <summary>
        /// Using the automapper, transferring data from database tables to DTO tables and displaying DTO tables in a DataGrid
        /// </summary>
        private void LoadTable()
        {
            using (OnlineShopContext db = new OnlineShopContext())
            {
                switch (SelectTable.SelectedItem)
                {
                    case "Client":
                        var configClient = new MapperConfiguration(cfg => cfg.CreateMap<Entities.Client, DTO.Client>());
                        var mapperClient = new Mapper(configClient);
                        var clientDTO = mapperClient.Map<List<DTO.Client>>(db.Clients.ToList());
                        DataBase.ItemsSource = clientDTO;
                        break;
                    case "Delivery":
                        var configDelivery = new MapperConfiguration(cfg => cfg.CreateMap<Entities.Delivery, DTO.Delivery>());
                        var mapperDelivery = new Mapper(configDelivery);
                        var deliveryDTO = mapperDelivery.Map<List<DTO.Delivery>>(db.Deliveries.ToList());
                        DataBase.ItemsSource = deliveryDTO;
                        break;
                    case "Delivery_Product":
                        var configDeliveryProduce = new MapperConfiguration(cfg => cfg.CreateMap<Entities.DeliveryProduct, DTO.DeliveryProduct>());
                        var mapperDeliveryProduce = new Mapper(configDeliveryProduce);
                        var deliveryProductDTO = mapperDeliveryProduce.Map<List<DTO.DeliveryProduct>>(db.DeliveryProducts.ToList());
                        DataBase.ItemsSource = deliveryProductDTO;
                        break;
                    case "Order":
                        var configOrder = new MapperConfiguration(cfg => cfg.CreateMap<Entities.Order, DTO.Order>());
                        var mapperOrder = new Mapper(configOrder);
                        var orderDTO = mapperOrder.Map<List<DTO.Order>>(db.Orders.ToList());
                        DataBase.ItemsSource = orderDTO;
                        break;
                    case "ProductGroup":
                        var configProductGroup = new MapperConfiguration(cfg => cfg.CreateMap<Entities.ProductGroup, DTO.ProductGroup>());
                        var mapperProductGroup = new Mapper(configProductGroup);
                        var productGroupDTO = mapperProductGroup.Map<List<DTO.ProductGroup>>(db.ProductGroups.ToList());
                        DataBase.ItemsSource = productGroupDTO;
                        break;
                    case "Product_Order":
                        var configProductOrder = new MapperConfiguration(cfg => cfg.CreateMap<Entities.ProductOrder, DTO.ProductOrder>());
                        var mapperProductOrder = new Mapper(configProductOrder);
                        var productOrderDTO = mapperProductOrder.Map<List<DTO.ProductOrder>>(db.ProductOrders.ToList());
                        DataBase.ItemsSource = productOrderDTO;
                        break;
                    case "Product":
                        var configProduct = new MapperConfiguration(cfg => cfg.CreateMap<Entities.Product, DTO.Product>());
                        var mapperProduct = new Mapper(configProduct);
                        var productDTO = mapperProduct.Map<List<DTO.Product>>(db.Products.ToList());
                        DataBase.ItemsSource = productDTO;
                        break;
                    case "Supplier":
                        var configSupplier = new MapperConfiguration(cfg => cfg.CreateMap<Entities.Supplier, DTO.Supplier>());
                        var mapperSupplier = new Mapper(configSupplier);
                        var supplierDTO = mapperSupplier.Map<List<DTO.Supplier>>(db.Suppliers.ToList());
                        DataBase.ItemsSource = supplierDTO;
                        break;
                    case "UnitMeasurement":
                        var configUnitMeasurement = new MapperConfiguration(cfg => cfg.CreateMap<Entities.UnitMeasurement, DTO.UnitMeasurement>());
                        var mapperUnitMeasurement = new Mapper(configUnitMeasurement);
                        var unitMeasurementDTO = mapperUnitMeasurement.Map<List<DTO.UnitMeasurement>>(db.UnitMeasurements.ToList());
                        DataBase.ItemsSource = unitMeasurementDTO;
                        break;
                }
            }

        }
    }
}
