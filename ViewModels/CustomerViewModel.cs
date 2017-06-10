using CrudWithRealmApp.Models;
using CrudWithRealmApp.Views;
using Realms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CrudWithRealmApp.ViewModels
{
    public class CustomerViewModel : INotifyPropertyChanged
    {
        Realm _getRealmInstance = Realm.GetInstance();

        private List<CustomerDetails> _listOfCustomerDetails;

        public List<CustomerDetails> ListOfCustomerDetails
        {
            get { return _listOfCustomerDetails; }
            set
            {
                _listOfCustomerDetails = value;
                OnPropertyChanged();
            }
        }

        private CustomerDetails _customerDetails = new CustomerDetails();

        public CustomerDetails CustomerDetails
        {
            get { return _customerDetails; }
            set
            {
                _customerDetails = value;
                OnPropertyChanged();
            }
        }


        public Command CreateCommand
        {
            get
            {
                return new Command(() =>
                {
                    _customerDetails.CustomerId = _getRealmInstance.All<CustomerDetails>().Count() + 1;
                    _getRealmInstance.Write(() =>
                    {
                        _getRealmInstance.Add(_customerDetails);
                    });
                });
            }
        }

        public Command UpdateCommand
        {
            get
            {
                return new Command(() =>
                {
                    var customerDetailsUpdate = new CustomerDetails
                    {
                        CustomerId = _customerDetails.CustomerId,
                        CustomerName = _customerDetails.CustomerName,
                        CustomerAge = _customerDetails.CustomerAge
                    };

                    _getRealmInstance.Write(() =>
                    {
                        _getRealmInstance.Add(customerDetailsUpdate, update: true);
                    });
                });
            }
        }

        public Command RemoveCommand
        {
            get
            {
                return new Command(() =>
                {
                    var getAllCustomerDetailsById = _getRealmInstance.All<CustomerDetails>().First(x => x.CustomerId == _customerDetails.CustomerId);

                    using (var transaction = _getRealmInstance.BeginWrite())
                    {
                        _getRealmInstance.Remove(getAllCustomerDetailsById);
                        transaction.Commit();
                    };
                });
            }
        }

        public CustomerViewModel()
        {
            ListOfCustomerDetails = _getRealmInstance.All<CustomerDetails>().ToList();
        }

        // For Navigation Page
        public Command NavToListCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await App.Current.MainPage.Navigation.PushAsync(new ListOfCustomers());
                });
            }
        }

        public Command NavToCreateCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await App.Current.MainPage.Navigation.PushAsync(new CreateCustomer());
                });
            }
        }

        public Command NavToUpdateDeleteCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await App.Current.MainPage.Navigation.PushAsync(new UpdateOrDeleteCustomer());
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
