using PaymentsEF.Command;
using PaymentsEF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PaymentsEF.ViewModel
{
    class VM : INotifyPropertyChanged
    {
        #region ArrivalsVariables
        /// <summary>
        /// Отображаемый список взносов
        /// </summary>
        public ObservableCollection<Arrivals> ArrivalsList { get; set; }
        /// <summary>
        /// Отдельно взятый взнос
        /// </summary>
        public Arrivals ArrivalSelected { get; set; }
        List<int> selectedIndexesArrivals;
        /// <summary>
        /// Индексы выбранных взносов
        /// </summary>
        public List<int> SelectedIndexesArrivals
        {
            get { return selectedIndexesArrivals; }
            set { selectedIndexesArrivals = value; }
        }
        string stringSelectedIndexesArrivals;
        /// <summary>
        /// Индексы выбранных взносов в виде строки для отображения
        /// </summary>
        public string StringSelectedIndexesArrivals
        {
            get { return stringSelectedIndexesArrivals; }
            set { stringSelectedIndexesArrivals = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// Новый взнос (берется с TextBox)
        /// </summary>
        public decimal NewSumOfArrival { get; set; }
        /// <summary>
        /// Все выбранные взносы
        /// </summary>
        List<Arrivals> AllArrivalModels { get; set; }
        #endregion

        #region OrdersVariables
        /// <summary>
        /// Отображаемый список заказов
        /// </summary>
        public ObservableCollection<Orders> OrdersList { get; set; }
        /// <summary>
        /// Отдельно взятый заказ
        /// </summary>
        public Orders OrderSelected { get; set; }
        List<int> selectedIndexesOrders;
        /// <summary>
        /// Индексы выбранных заказов
        /// </summary>
        public List<int> SelectedIndexesOrders
        {
            get { return selectedIndexesOrders; }
            set { selectedIndexesOrders = value; }
        }

        string stringSelectedIndexesOrders = "";
        /// <summary>
        /// Индексы выбранных заказов в виде строки для отображения
        /// </summary>
        public string StringSelectedIndexesOrders
        {
            get { return stringSelectedIndexesOrders; }
            set { stringSelectedIndexesOrders = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// Новый заказ (берется с TextBox)
        /// </summary>
        public decimal NewSumOfOrder { get; set; }
        /// <summary>
        /// Все выбранные заказы
        /// </summary>
        List<Orders> AllOrderModels { get; set; }
        #endregion

        /// <summary>
        /// Отображаемый список платежей
        /// </summary>
        public ObservableCollection<Payments> Payments { get; set; }
        /// <summary>
        /// Сумма списания (берется с TextBox)
        /// </summary>
        public decimal MoneyToOrder { get; set; }

        #region ArrivalsMethods
        /// <summary>
        /// Загрузка данных по взносам
        /// </summary>
        void FillArrivalsList()
        {
            ArrivalsList.Clear();

            using (var db = new PaymentsEFContext())
            {
                var arriv = db.Arrivals;
                foreach (var arrival in arriv)
                {
                    ArrivalsList.Add(arrival);
                }
            }

        }

        //Команда для выбора взноса
        ICommand arrivalCommand;

        public ICommand ArrivalCommand
        {
            get
            {
                if (arrivalCommand == null)
                {
                    arrivalCommand = new MyCommand(ArrivalExecute, CanArrivalExecute);
                }
                return arrivalCommand;
            }
        }

        void ArrivalExecute(object parameter)
        {
            if (!SelectedIndexesArrivals.Contains(ArrivalSelected.Idarrival) && ArrivalSelected.Idarrival != 0)
            {
                SelectedIndexesArrivals.Add(ArrivalSelected.Idarrival);
                StringSelectedIndexesArrivals = String.Join(", ", SelectedIndexesArrivals);
                AllArrivalModels.Add(ArrivalSelected);
            }
        }

        bool CanArrivalExecute(object parameter)
        {
            return true;
        }

        //Команда для сброса выбранных взносов
        ICommand resetArrivalsCommand;

        public ICommand ResetArrCommand
        {
            get
            {
                if (resetArrivalsCommand == null)
                {
                    resetArrivalsCommand = new MyCommand(ResetArrivalsExecute, CanResetArrivalsExecute);
                }
                return resetArrivalsCommand;
            }
        }

        void ResetArrivalsExecute(object parameter)
        {
            SelectedIndexesArrivals.Clear();
            StringSelectedIndexesArrivals = "";
            AllArrivalModels.Clear();
        }

        bool CanResetArrivalsExecute(object parameter)
        {
            return true;
        }

        //Команда для добавления взноса
        ICommand addArrivalCommand;

        public ICommand AddArrivalCommand
        {
            get
            {
                if (addArrivalCommand == null)
                {
                    addArrivalCommand = new MyCommand(AddArrivalExecute, CanAddArrivalExecute);
                }
                return addArrivalCommand;
            }
        }

        void AddArrivalExecute(object parameter)
        {
            AddArrival();
        }

        bool CanAddArrivalExecute(object parameter)
        {
            if (NewSumOfArrival <= 0)
                return false;
            return true;
        }

        void AddArrival()
        {
            try
            {
                using (var db = new PaymentsEFContext())
                {
                    Arrivals newArrival = new Arrivals();
                    newArrival.ArrivalDate = DateTime.Now;
                    newArrival.SumOfArrival = NewSumOfArrival;
                    newArrival.Remains = NewSumOfArrival;
                    db.Arrivals.Add(newArrival);
                    db.SaveChanges();
                }
                FillArrivalsList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при вставке взноса:\n {ex.Message}");
                Update();
            }
        }
        #endregion

        #region OrdersMethods
        /// <summary>
        /// Загрузка данных по заказам
        /// </summary>
        void FillOrdersList()
        {
            OrdersList.Clear();

            using (var db = new PaymentsEFContext())
            {
                var ord = db.Orders;
                foreach (var order in ord)
                {
                    OrdersList.Add(order);
                }
            }
        }

        //Команда для выбора заказа
        ICommand orderCommand;
        public ICommand OrderCommand
        {
            get
            {
                if (orderCommand == null)
                {
                    orderCommand = new MyCommand(OrdersExecute, CanOrdersExecute);
                }
                return orderCommand;
            }
        }
        void OrdersExecute(object parameter)
        {
            if (!SelectedIndexesOrders.Contains(OrderSelected.Idorder) && OrderSelected.Idorder != 0)
            {
                SelectedIndexesOrders.Add(OrderSelected.Idorder);
                StringSelectedIndexesOrders = String.Join(", ", SelectedIndexesOrders);
                AllOrderModels.Add(OrderSelected);
            }
        }
        bool CanOrdersExecute(object parameter)
        {
            return true;
        }

        //Команда для сброса выбранных заказов
        ICommand resetOrdersCommand;
        public ICommand ResetOrdersCommand
        {
            get
            {
                if (resetOrdersCommand == null)
                {
                    resetOrdersCommand = new MyCommand(ResetOrdersExecute, CanResetOrdersExecute);
                }
                return resetOrdersCommand;
            }
        }
        void ResetOrdersExecute(object parameter)
        {
            SelectedIndexesOrders.Clear();
            StringSelectedIndexesOrders = "";
            AllOrderModels.Clear();
        }
        bool CanResetOrdersExecute(object parameter)
        {
            return true;
        }

        //Команда для добавления заказа в бд
        ICommand addOrderCommand;
        public ICommand AddOrderCommand
        {
            get
            {
                if (addOrderCommand == null)
                {
                    addOrderCommand = new MyCommand(AddOrderExecute, CanAddOrderExecute);
                }
                return addOrderCommand;
            }
        }
        void AddOrderExecute(object parameter)
        {
            AddOrder();
        }
        bool CanAddOrderExecute(object parameter)
        {
            if (NewSumOfOrder <= 0)
                return false;
            return true;
        }
        void AddOrder()
        {
            try
            {
                using (var db = new PaymentsEFContext())
                {
                    Orders newOrder = new Orders();
                    newOrder.OrderDate = DateTime.Now;
                    newOrder.Payment = NewSumOfOrder;
                    db.Orders.Add(newOrder);
                    db.SaveChanges();
                }
                FillOrdersList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при вставке заказа:\n {ex.Message}");
            }
        }
        #endregion

        #region PaymentsMethods
        /// <summary>
        /// Загрузка данных по платежам
        /// </summary>
        void FillPaymentsList()
        {
            try
            {
                Payments.Clear();

                using (var db = new PaymentsEFContext())
                {
                    var pay = db.Payments;
                    foreach (var payment in pay)
                    {
                        Payments.Add(payment);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"FillPaymentsList error: {ex.Message}");
            }
        }

        //Команда для формирования платежа
        ICommand addPaymentCommand;
        public ICommand AddPaymentCommand 
        {
            get
            {
                if (addPaymentCommand == null)
                {
                    addPaymentCommand = new MyCommand(AddPaymentExecute, CanAddPaymentExecute);
                }
                return addPaymentCommand;
            }
        }
        void AddPaymentExecute(object parameter)
        {
            if (AllArrivalModels.Count == 0)
                MessageBox.Show($"Выберите счета взносов!");
            else if (AllOrderModels.Count == 0)
                MessageBox.Show($"Выберите заказы!");
            else if (MoneyToOrder <= 0)
                MessageBox.Show($"Введите корректную сумму списания!");
            else
                ChooseArrivalAndOrder(AllArrivalModels, AllOrderModels, MoneyToOrder);
        }
        bool CanAddPaymentExecute(object parameter)
        {
            return true;
        }
        /// <summary>
        /// Выбор с какого счета и на какой заказ сколько будет списываться
        /// </summary>
        void ChooseArrivalAndOrder(List<Arrivals> CurrentArrivals, List<Orders> CurrentOrders, decimal moneyToOrder)
        {
            //проверяем актуальность данных
            if (!CheckData(CurrentArrivals, CurrentOrders))
            {
                SelectedIndexesArrivals.Clear();
                StringSelectedIndexesArrivals = "";
                AllArrivalModels.Clear();
                SelectedIndexesOrders.Clear();
                StringSelectedIndexesOrders = "";
                AllOrderModels.Clear();
                CurrentArrivals.Clear();
                CurrentOrders.Clear();
                Update();
                return;
            }

            /* считаем сколько на счетах остатков
             * если меньше суммы списания - оповещаем об этом
             * и прерываем метод
             */
            decimal? remains = 0;
            for (int i = 0; i < CurrentArrivals.Count; i++)
            {
                remains += CurrentArrivals[i].Remains;
            }
            if (remains < moneyToOrder)
            {
                MessageBox.Show($"На счетах находится меньше денег, чем заявленная сумма");
                Update();
                return;
            }

            /* считаем сумму нужных платежей по заказам
             * если она меньше суммы списания - непонятно, 
             * куда пойдут оставшиеся деньги, 
             * оповещаем об этом и прерываем метод
             */
            decimal? sumAmount = 0;
            for (int i = 0; i < CurrentOrders.Count; i++)
            {
                sumAmount += CurrentOrders[i].Payment - CurrentOrders[i].PaymentAmount;
            }
            if (moneyToOrder > sumAmount)
            {
                MessageBox.Show($"Введенной суммы слишком много для оплаты заказов. Возможно, заказ уже оплачен");
                Update();
                return;
            }
            //сумма списания
            decimal amount = moneyToOrder;
            //индекс счета
            int j = 0;
            //проходим по всем заказам
            for (int i = 0; i < CurrentOrders.Count; i++)
            {
                if (CurrentOrders[i].PaymentAmount == CurrentOrders[i].Payment)
                {
                    MessageBox.Show($"Заказ {CurrentOrders[i].Idorder} полностью оплачен");
                    Update();
                    continue;
                }
                //пока сумма списания > 0 и заказ не оплачен
                while (amount > 0 && CurrentOrders[i].Payment > CurrentOrders[i].PaymentAmount)
                {
                    //если дошли до последнего счета - прерываем метод, т.к. больше не откуда списывать
                    if (j == CurrentArrivals.Count)
                        return;
                    //если деньги на счету закончились - переходим к следующему
                    if (CurrentArrivals[j].Remains == 0)
                    {
                        j++;
                        continue;
                    }
                    //выбираем минимум из остатка на счете, нужной суммой для заказа и оставшейся сумме списания
                    decimal moneyToPay = 0;
                    if (CurrentArrivals[j].Remains is decimal rem && CurrentOrders[i].PaymentAmount is decimal payAmount)
                    {
                        moneyToPay = Math.Min(Math.Min(rem, CurrentOrders[i].Payment - payAmount), amount);
                    }
                    //заводим платеж
                    AddPayment(CurrentArrivals[j].Idarrival, CurrentOrders[i].Idorder, moneyToPay);
                    amount -= moneyToPay;
                    CurrentArrivals[j].Remains -= moneyToPay;
                    CurrentOrders[i].PaymentAmount += moneyToPay;
                    if (CurrentOrders[i].PaymentAmount == CurrentOrders[i].Payment)
                    {

                        MessageBox.Show($"Заказ {CurrentOrders[i].Idorder} полностью оплачен");
                        Update();
                    }
                }
            }
        }

        /// <summary>
        /// Проверка актуальность данных
        /// </summary>
        bool CheckData(List<Arrivals> Arrivals, List<Orders> Orders)
        {
            using (var db = new PaymentsEFContext())
            {
                foreach (var arrival in Arrivals)
                {
                    if (db.Arrivals.Find(arrival.Idarrival).Remains != arrival.Remains)
                    {
                        if (MessageBox.Show("Данные выбранных позиций неактуальны. Транзакция может привести к неожиданным результатам. Вы уверены, что хотите продолжить?", "Несовпадение данных", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                        {
                            return false;
                        }
                    }
                }
                foreach (var order in Orders)
                {
                    if (db.Orders.Find(order.Idorder).Payment != order.Payment)
                    {
                        if (MessageBox.Show("Данные выбранных позиций неактуальны. Транзакция может привести к неожиданным результатам. Вы уверены, что хотите продолжить?", "Несовпадение данных", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Добавление платежа в таблицу бд
        /// </summary>
        void AddPayment(int indexArrival, int indexOrder, decimal moneyToOrder)
        {
            try
            {
                using (var db = new PaymentsEFContext())
                {
                    Payments newPayment = new Payments();
                    newPayment.OrderId = indexOrder;
                    newPayment.ArrivalId = indexArrival;
                    newPayment.Amount = moneyToOrder;
                    db.Payments.Add(newPayment);
                    db.SaveChanges();
                }
                Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при вставке платежа:\n {ex.Message}");
                Update();
            }
        }
        #endregion

        //Команда обновления списков
        ICommand updateCommand;
        public ICommand UpdateCommand
        {
            get
            {
                if (updateCommand == null)
                {
                    updateCommand = new MyCommand(UpdateExecute, CanUpdateExecute);
                }
                return updateCommand;
            }
        }
        void UpdateExecute(object parameter)
        {
            Update();
        }
        bool CanUpdateExecute(object parameter)
        {
            return true;
        }

        void Update()
        {
            FillOrdersList();
            FillArrivalsList();
            FillPaymentsList();
            ArrivalSelected = new Arrivals();
            OrderSelected = new Orders();
        }

        public VM()
        {
            selectedIndexesArrivals = new List<int>();
            AllArrivalModels = new List<Arrivals>();
            ArrivalsList = new ObservableCollection<Arrivals>();
            selectedIndexesOrders = new List<int>();
            AllOrderModels = new List<Orders>();
            OrdersList = new ObservableCollection<Orders>();
            Payments = new ObservableCollection<Payments>();
            Update();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
