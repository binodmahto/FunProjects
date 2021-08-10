using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDemo
{
    public class Person : BindableBase
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Occupation { get; set; }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

        private DragRowEffect rowEffect;
        public DragRowEffect RowEffect
        {
            get => rowEffect;
            set => SetProperty(ref rowEffect, value);
        }
    }
    
    public enum DragRowEffect
    {
        None,
        Before,
        After
    }
}
