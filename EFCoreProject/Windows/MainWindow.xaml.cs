using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EFCoreProject.Entities;
using EFCoreProject.Windows;
using System.Globalization;
using System.Net;

namespace EFCoreProject
{
    public partial class MainWindow : Window
    {
        private Dictionary<int, TextBox>? createTextBox;
        private Dictionary<int, RowDefinition>? DeleteRowDefinitions;
        private Dictionary<int, Label>? DeleteLabel;

        public MainWindow()
        {
            InitializeComponent();
        }

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

        private void SelectTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadTable();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ServerLogin sl = new ServerLogin();
            sl.Show();
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Row.Visibility = Visibility.Hidden;
            AddRowInTable(false);

            for (int i = 0; i < DataBase.Columns.Count; i++)
            {
                Row.RowDefinitions.Remove(DeleteRowDefinitions![i]);
                Row.Children.Remove(DeleteLabel![i]);
                Row.Children.Remove(createTextBox![i]);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (createTextBox is not null)
            {
                using (OnlineShopContext db = new OnlineShopContext())
                {
                    switch (SelectTable.SelectedItem)
                    {
                        case "Client":
                            Client client = new Client
                            {
                                FirstName = createTextBox[1].Text,
                                Surname = createTextBox[2].Text,
                                Patronymic = createTextBox[3].Text,
                                Address = createTextBox[4].Text,
                                PhoneNumber = Convert.ToDecimal(createTextBox[5].Text)
                            };
                            db.Clients.Add(client);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "Delivery":
                            Delivery delivery = new Delivery
                            {
                                DateDelivery = Convert.ToDateTime(createTextBox[1].Text),
                                Remark = createTextBox[2].Text,
                                SupplierId = Convert.ToInt32(createTextBox[3].Text)
                            };
                            db.Deliveries.Add(delivery);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "DeliveryProduct":
                            DeliveryProduct deliveryProduct = new DeliveryProduct
                            {
                                DeliveryId = Convert.ToInt32(createTextBox[1].Text),
                                ProductId = Convert.ToInt32(createTextBox[2].Text)
                            };
                            db.DeliveryProducts.Add(deliveryProduct);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "Order":
                            Order order = new Order
                            {
                                DateOrder = Convert.ToDateTime(createTextBox[1].Text),
                                Count = Convert.ToInt32(createTextBox[2].Text),
                                PaymentType = createTextBox[3].Text,
                                Remark = createTextBox[4].Text,
                                ClientId = Convert.ToInt32(createTextBox[5].Text)
                            };
                            db.Orders.Add(order);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "Product":
                            Product product = new Product
                            {
                                Name = createTextBox[1].Text,
                                Price = Convert.ToInt32(createTextBox[2].Text),
                                UnitMeasurementId = Convert.ToInt32(createTextBox[3].Text),
                                Description = createTextBox[4].Text,
                                ProductGroupId = Convert.ToInt32(createTextBox[5].Text)
                            };
                            db.Products.Add(product);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "ProductGroup":
                            ProductGroup productGroup = new ProductGroup
                            {
                                Name = createTextBox[1].Text
                            };
                            db.ProductGroups.Add(productGroup);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "ProductOrder":
                            ProductOrder productOrder = new ProductOrder
                            {
                                ProductId = Convert.ToInt32(createTextBox[1].Text),
                                OrderId = Convert.ToInt32(createTextBox[2].Text)
                            };
                            db.ProductOrders.Add(productOrder);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "Supplier":
                            Supplier supplier = new Supplier
                            {
                                Name = createTextBox[1].Text,
                                Address = createTextBox[4].Text,
                                PhoneNumber = Convert.ToDecimal(createTextBox[5].Text)
                            };
                            db.Suppliers.Add(supplier);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "UnitMeasurement":
                            UnitMeasurement unitMeasurement = new UnitMeasurement
                            {
                                Name = createTextBox[1].Text
                            };
                            db.UnitMeasurements.Add(unitMeasurement);
                            db.SaveChanges();
                            LoadTable();
                            break;
                    }
                }
                Row.Visibility = Visibility.Hidden;
                AddRowInTable(false);
            }
            DeleteDinamicList();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (createTextBox is not null)
            {
                using (OnlineShopContext db = new OnlineShopContext())
                {
                    switch (SelectTable.SelectedItem)
                    {
                        case "Client":
                            Client client = (Client)DataBase.SelectedItem;
                            client.FirstName = createTextBox[1].Text;
                            client.Surname = createTextBox[2].Text;
                            client.Patronymic = createTextBox[3].Text;
                            client.Address = createTextBox[4].Text;
                            client.PhoneNumber = Convert.ToDecimal(createTextBox[5].Text);
                            db.Clients.Update(client);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "Delivery":
                            Delivery delivery = (Delivery)DataBase.SelectedItem;
                            delivery.DateDelivery = Convert.ToDateTime(createTextBox[1].Text);
                            delivery.Remark = createTextBox[2].Text;
                            delivery.SupplierId = Convert.ToInt32(createTextBox[3].Text);
                            db.Deliveries.Update(delivery);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "DeliveryProduct":
                            DeliveryProduct deliveryProduct = (DeliveryProduct)DataBase.SelectedItem;
                            deliveryProduct.DeliveryId = Convert.ToInt32(createTextBox[1].Text);
                            deliveryProduct.ProductId = Convert.ToInt32(createTextBox[2].Text);
                            db.DeliveryProducts.Update(deliveryProduct);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "Order":
                            Order order = (Order)DataBase.SelectedItem;
                            order.DateOrder = Convert.ToDateTime(createTextBox[1].Text);
                            order.Count = Convert.ToInt32(createTextBox[2].Text);
                            order.PaymentType = createTextBox[3].Text;
                            order.Remark = createTextBox[4].Text;
                            order.ClientId = Convert.ToInt32(createTextBox[5].Text);
                            db.Orders.Update(order);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "Product":
                            Product product = (Product)DataBase.SelectedItem;
                            product.Name = createTextBox[1].Text;
                            product.Price = Convert.ToInt32(createTextBox[2].Text);
                            product.UnitMeasurementId = Convert.ToInt32(createTextBox[3].Text);
                            product.Description = createTextBox[4].Text;
                            product.ProductGroupId = Convert.ToInt32(createTextBox[5].Text);
                            db.Products.Update(product);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "ProductGroup":
                            ProductGroup productGroup = (ProductGroup)DataBase.SelectedItem;
                            productGroup.Name = createTextBox[1].Text;
                            db.ProductGroups.Update(productGroup);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "ProductOrder":
                            ProductOrder productOrder = (ProductOrder)DataBase.SelectedItem;
                            productOrder.ProductId = Convert.ToInt32(createTextBox[1].Text);
                            productOrder.OrderId = Convert.ToInt32(createTextBox[2].Text);
                            db.ProductOrders.Update(productOrder);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "Supplier":
                            Supplier supplier = (Supplier)DataBase.SelectedItem;
                            supplier.Name = createTextBox[1].Text;
                            supplier.Address = createTextBox[4].Text;
                            supplier.PhoneNumber = Convert.ToDecimal(createTextBox[5].Text);
                            db.Suppliers.Update(supplier);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "UnitMeasurement":
                            UnitMeasurement unitMeasurement = (UnitMeasurement)DataBase.SelectedItem;
                            unitMeasurement.Name = createTextBox[1].Text;
                            db.UnitMeasurements.Update(unitMeasurement);
                            db.SaveChanges();
                            LoadTable();
                            break;
                    }
                }
                Row.Visibility = Visibility.Hidden;
                AddRowInTable(false);
            }
            DeleteDinamicList();
        }

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
                                        createTextBox[0].Text = Convert.ToString(((Client)DataBase.SelectedItem).Id);
                                        createTextBox[1].Text = Convert.ToString(((Client)DataBase.SelectedItem).FirstName);
                                        createTextBox[2].Text = Convert.ToString(((Client)DataBase.SelectedItem).Surname);
                                        createTextBox[3].Text = Convert.ToString(((Client)DataBase.SelectedItem).Patronymic);
                                        createTextBox[4].Text = Convert.ToString(((Client)DataBase.SelectedItem).Address);
                                        createTextBox[5].Text = Convert.ToString(((Client)DataBase.SelectedItem).PhoneNumber);
                                        break;
                                    case "Delivery":
                                        createTextBox[0].Text = Convert.ToString(((Delivery)DataBase.SelectedItem).Id);
                                        createTextBox[1].Text = Convert.ToString(((Delivery)DataBase.SelectedItem).DateDelivery);
                                        createTextBox[2].Text = Convert.ToString(((Delivery)DataBase.SelectedItem).Remark);
                                        createTextBox[3].Text = Convert.ToString(((Delivery)DataBase.SelectedItem).SupplierId);
                                        break;
                                    case "DeliveryProduct":
                                        createTextBox[0].Text = Convert.ToString(((DeliveryProduct)DataBase.SelectedItem).DeliveryId);
                                        createTextBox[1].Text = Convert.ToString(((DeliveryProduct)DataBase.SelectedItem).ProductId);
                                        break;
                                    case "Order":
                                        createTextBox[0].Text = Convert.ToString(((Order)DataBase.SelectedItem).Id);
                                        createTextBox[1].Text = Convert.ToString(((Order)DataBase.SelectedItem).DateOrder);
                                        createTextBox[2].Text = Convert.ToString(((Order)DataBase.SelectedItem).Count);
                                        createTextBox[3].Text = Convert.ToString(((Order)DataBase.SelectedItem).PaymentType);
                                        createTextBox[4].Text = Convert.ToString(((Order)DataBase.SelectedItem).Remark);
                                        createTextBox[5].Text = Convert.ToString(((Order)DataBase.SelectedItem).ClientId);
                                        break;
                                    case "Product":
                                        createTextBox[0].Text = Convert.ToString(((Product)DataBase.SelectedItem).Id);
                                        createTextBox[1].Text = Convert.ToString(((Product)DataBase.SelectedItem).Name);
                                        createTextBox[2].Text = Convert.ToString(((Product)DataBase.SelectedItem).Price);
                                        createTextBox[3].Text = Convert.ToString(((Product)DataBase.SelectedItem).UnitMeasurementId);
                                        createTextBox[4].Text = Convert.ToString(((Product)DataBase.SelectedItem).Description);
                                        createTextBox[5].Text = Convert.ToString(((Product)DataBase.SelectedItem).ProductGroupId);
                                        break;
                                    case "ProductGroup":
                                        createTextBox[0].Text = Convert.ToString(((ProductGroup)DataBase.SelectedItem).Id);
                                        createTextBox[1].Text = Convert.ToString(((ProductGroup)DataBase.SelectedItem).Name);
                                        createTextBox[2].Text = Convert.ToString(((ProductGroup)DataBase.SelectedItem).Products);
                                        break;
                                    case "ProductOrder":
                                        createTextBox[0].Text = Convert.ToString(((ProductOrder)DataBase.SelectedItem).ProductId);
                                        createTextBox[1].Text = Convert.ToString(((ProductOrder)DataBase.SelectedItem).OrderId);
                                        break;
                                    case "Supplier":
                                        createTextBox[0].Text = Convert.ToString(((Supplier)DataBase.SelectedItem).Id);
                                        createTextBox[1].Text = Convert.ToString(((Supplier)DataBase.SelectedItem).Name);
                                        createTextBox[2].Text = Convert.ToString(((Supplier)DataBase.SelectedItem).Address);
                                        createTextBox[3].Text = Convert.ToString(((Supplier)DataBase.SelectedItem).PhoneNumber);
                                        createTextBox[4].Text = Convert.ToString(((Supplier)DataBase.SelectedItem).Deliveries);
                                        break;
                                    case "UnitMeasurement":
                                        createTextBox[0].Text = Convert.ToString(((UnitMeasurement)DataBase.SelectedItem).Id);
                                        createTextBox[1].Text = Convert.ToString(((UnitMeasurement)DataBase.SelectedItem).Name);
                                        createTextBox[2].Text = Convert.ToString(((UnitMeasurement)DataBase.SelectedItem).Products);
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

        private void DeleteLineButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataBase.SelectedItem is not null)
            {
                using (OnlineShopContext db = new OnlineShopContext())
                {
                    switch (SelectTable.SelectedItem)
                    {
                        case "Client":
                            db.Clients.Remove((Client)DataBase.SelectedItem);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "Delivery":
                            db.Deliveries.Remove((Delivery)DataBase.SelectedItem);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "DeliveryProduct":
                            db.DeliveryProducts.Remove((DeliveryProduct)DataBase.SelectedItem);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "Order":
                            db.Orders.Remove((Order)DataBase.SelectedItem);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "Product":
                            db.Products.Remove((Product)DataBase.SelectedItem);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "ProductGroup":
                            db.ProductGroups.Remove((ProductGroup)DataBase.SelectedItem);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "ProductOrder":
                            db.ProductOrders.Remove((ProductOrder)DataBase.SelectedItem);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "Supplier":
                            db.Suppliers.Remove((Supplier)DataBase.SelectedItem);
                            db.SaveChanges();
                            LoadTable();
                            break;
                        case "UnitMeasurement":
                            db.UnitMeasurements.Remove((UnitMeasurement)DataBase.SelectedItem);
                            db.SaveChanges();
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

        private void ClearTable_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) { }
            else
            {
                if (SelectTable.SelectedItems.Count > 0)
                {
                    using (OnlineShopContext db = new OnlineShopContext())
                    {
                        int clearTable = db.Database.ExecuteSqlRaw($"DELETE FROM {SelectTable.SelectedItem}");
                        int resetId = db.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('{SelectTable.SelectedItem}', RESEED, 0)");
                        LoadTable();
                    }
                }
                else
                {
                    MessageBox.Show("Select table");
                }
            }
        }

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

        private void DeleteDinamicList()
        {
            for (int i = 0; i < DataBase.Columns.Count; i++)
            {
                Row.RowDefinitions.Remove(DeleteRowDefinitions![i]);
                Row.Children.Remove(DeleteLabel![i]);
                Row.Children.Remove(createTextBox![i]);
            }
        }
        
        private void AddRowInTable(bool enable)
        {
            if (enable)
            {
                SelectTable.IsEnabled = false;
                AddLineButton.IsEnabled = false;
                EditLineButton.IsEnabled = false;
                DeleteLineButton.IsEnabled = false;
                ClearTable.IsEnabled = false;
                Back.IsEnabled = false;
            }
            else if (enable == false)
            {
                SelectTable.IsEnabled = true;
                AddLineButton.IsEnabled = true;
                EditLineButton.IsEnabled = true;
                DeleteLineButton.IsEnabled = true;
                ClearTable.IsEnabled = true;
                Back.IsEnabled = true;
            }
        }

        private void LoadTable()
        {
            using (OnlineShopContext db = new OnlineShopContext())
            {
                switch (SelectTable.SelectedItem)
                {
                    case "Client":
                        var Client = (from client in db.Clients select client).ToList();
                        DataBase.Columns.Clear();
                        if (db.Clients.Any())
                        {
                            DataBase.ItemsSource = Client;
                        }
                        else
                        {
                            var columnsList = (from t in typeof(Client).GetProperties() select t.Name).ToList();
                            for (int i = 0; i < columnsList.Count; i++)
                            {
                                DataBase.Columns.Add(new DataGridTextColumn()
                                {
                                    Header = columnsList[i]
                                });
                            }
                        }
                        break;
                    case "Delivery":
                        var Deliveries = db.Deliveries.ToList();
                        DataBase.Columns.Clear();
                        if (db.Deliveries.Any())
                        {
                            DataBase.ItemsSource = Deliveries;
                        }
                        else
                        {
                            var columnsList = (from t in typeof(Delivery).GetProperties() select t.Name).ToList();
                            for (int i = 0; i < columnsList.Count; i++)
                            {
                                DataBase.Columns.Add(new DataGridTextColumn()
                                {
                                    Header = columnsList[i]
                                });
                            }
                        }
                        break;
                    case "Delivery_Product":
                        var DeliveryProducts = db.DeliveryProducts.ToList();
                        DataBase.Columns.Clear();
                        if (db.DeliveryProducts.Any())
                        {
                            DataBase.ItemsSource = DeliveryProducts;
                        }
                        else
                        {
                            var columnsList = (from t in typeof(DeliveryProduct).GetProperties() select t.Name).ToList();
                            for (int i = 0; i < columnsList.Count; i++)
                            {
                                DataBase.Columns.Add(new DataGridTextColumn()
                                {
                                    Header = columnsList[i]
                                });
                            }
                        }
                        break;
                    case "Order":
                        var Order = db.Orders.ToList();
                        DataBase.Columns.Clear();
                        if (db.Orders.Any())
                        {
                            DataBase.ItemsSource = Order;
                        }
                        else
                        {
                            var columnsList = (from t in typeof(Order).GetProperties() select t.Name).ToList();
                            for (int i = 0; i < columnsList.Count; i++)
                            {
                                DataBase.Columns.Add(new DataGridTextColumn()
                                {
                                    Header = columnsList[i]
                                });
                            }
                        }
                        break;
                    case "ProductGroup":
                        var ProductGroup = db.ProductGroups.ToList();
                        DataBase.Columns.Clear();
                        if (db.ProductGroups.Any())
                        {
                            DataBase.ItemsSource = ProductGroup;
                        }
                        else
                        {
                            var columnsList = (from t in typeof(ProductGroup).GetProperties() select t.Name).ToList();
                            for (int i = 0; i < columnsList.Count; i++)
                            {
                                DataBase.Columns.Add(new DataGridTextColumn()
                                {
                                    Header = columnsList[i]
                                });
                            }
                        }
                        break;
                    case "Product_Order":
                        var ProductOrder = db.ProductOrders.ToList();
                        DataBase.Columns.Clear();
                        if (db.ProductOrders.Any())
                        {
                            DataBase.ItemsSource = ProductOrder;
                        }
                        else
                        {
                            var columnsList = (from t in typeof(ProductOrder).GetProperties() select t.Name).ToList();
                            for (int i = 0; i < columnsList.Count; i++)
                            {
                                DataBase.Columns.Add(new DataGridTextColumn()
                                {
                                    Header = columnsList[i]
                                });
                            }
                        }
                        break;
                    case "Product":
                        var Product = db.Products.ToList();
                        DataBase.Columns.Clear();
                        if (db.Products.Any())
                        {
                            DataBase.ItemsSource = Product;
                        }
                        else
                        {
                            var columnsList = (from t in typeof(Product).GetProperties() select t.Name).ToList();
                            for (int i = 0; i < columnsList.Count; i++)
                            {
                                DataBase.Columns.Add(new DataGridTextColumn()
                                {
                                    Header = columnsList[i]
                                });
                            }
                        }
                        break;
                    case "Supplier":
                        var Supplier = db.Suppliers.ToList();
                        DataBase.Columns.Clear();
                        if (db.Suppliers.Any())
                        {
                            DataBase.ItemsSource = Supplier;
                        }
                        else
                        {
                            var columnsList = (from t in typeof(Supplier).GetProperties() select t.Name).ToList();
                            for (int i = 0; i < columnsList.Count; i++)
                            {
                                DataBase.Columns.Add(new DataGridTextColumn()
                                {
                                    Header = columnsList[i]
                                });
                            }
                        }
                        break;
                    case "UnitMeasurement":
                        var UnitMeasurement = db.UnitMeasurements.ToList();
                        DataBase.Columns.Clear();
                        if (db.UnitMeasurements.Any())
                        {
                            DataBase.ItemsSource = UnitMeasurement;
                        }
                        else
                        {
                            var columnsList = (from t in typeof(UnitMeasurement).GetProperties() select t.Name).ToList();
                            for (int i = 0; i < columnsList.Count; i++)
                            {
                                DataBase.Columns.Add(new DataGridTextColumn()
                                {
                                    Header = columnsList[i]
                                });
                            }
                        }
                        break;
                }
            }

        }
    }
}
