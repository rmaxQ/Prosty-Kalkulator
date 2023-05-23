using Simple_Calculator.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Simple_Calculator.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        string _screenVal;
        List<string> _availableOperations = new List<string> { "+", "-", "/", "*" };
        DataTable _dataTable = new DataTable();
        bool isLastSignOperation;
        public MainViewModel()
        {
            ScreenVal = "0";
            isLastSignOperation = false;
            AddNumberCommand = new RelayCommand(AddNumber);
            AddOperationCommand = new RelayCommand(AddOperation);
            ClearScreenCommand = new RelayCommand(ClearScreen);
            GetResultCommand = new RelayCommand(GetResult, CanGetResult);
        }

        private bool CanGetResult(object obj)
        {
            return !isLastSignOperation;
        }

        private void GetResult(object obj)
        {
            var result = Math.Round(Convert.ToDouble(_dataTable.Compute(ScreenVal.Replace(",","."), "")),2);
            ScreenVal = result.ToString();
            isLastSignOperation = false;
        }

        private void ClearScreen(object obj)
        {
            ScreenVal = "0";
            isLastSignOperation = false;
        }

        private void AddOperation(object obj)
        {
            var operation = obj as string;

            if (_availableOperations.Contains(ScreenVal.Substring(ScreenVal.Length - 1)))
            {
                ScreenVal=ScreenVal.Remove(ScreenVal.Length - 1, 1);
            }
            ScreenVal += operation;
            isLastSignOperation = true;
        }
        private void AddNumber(object obj)
        {
            var number = obj as string;
            if (ScreenVal == "0" && number != ",")
                ScreenVal = string.Empty;
            else if (number == "," && _availableOperations.Contains(ScreenVal.Substring(ScreenVal.Length-1)))
                number = "0,";
            ScreenVal += number;
            isLastSignOperation = false;
        }

        public ICommand AddOperationCommand { get; set; }

        public ICommand AddNumberCommand { get; set; }
        public ICommand ClearScreenCommand { get; set; }
        public ICommand GetResultCommand { get; set; }
        public string ScreenVal
        {
            get { return _screenVal; }
            set 
            { 
                _screenVal = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
