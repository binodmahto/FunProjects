using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDemo
{
    class MainViewModel: BindableBase
    {
        private ObservableCollection<Person> persons;

        public ObservableCollection<Person> PersonCollection 
        {
            get => persons;
            set => SetProperty(ref persons, value);
        }


        public MainViewModel()
        {
            PersonCollection = new ObservableCollection<Person>()
            {
                new Person{ FirstName = "Amitabh", LastName = "Bachchan", Occupation = "Actor"},
                new Person{ FirstName = "Ajay", LastName = "Devgan", Occupation = "Actor"},
                new Person{ FirstName = "Ricky", LastName = "Ponting", Occupation = "Cricketer"},
                new Person{ FirstName = "M S", LastName = "Dhoni", Occupation = "Cricketer"},
                new Person{ FirstName = "Sachin", LastName = "Tendulkar", Occupation = "Cricketer"},
                new Person{ FirstName = "Ajay", LastName = "Mehra", Occupation = "Doctor"},
                new Person{ FirstName = "Binod", LastName = "Mahto", Occupation = "IT Professional"},
                new Person{ FirstName = "Anand", LastName = "Bansal", Occupation = "Business Man"},
                new Person{ FirstName = "Neeraj", LastName = "Chopra", Occupation = "Athletics"},
                new Person{ FirstName = "Aditi", LastName = "Ashok", Occupation = "Golfer"}

            };
        }

    }
}
