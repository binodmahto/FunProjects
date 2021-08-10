using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool droppingInProgress = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void PersonGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.DragOver += Row_DragOver;
        }

        private void Row_DragOver(object sender, DragEventArgs e)
        {
            var dataContext = this.DataContext as MainViewModel;
            var selectedItems = dataContext.PersonCollection.Where(hp => hp.IsSelected).ToList();
            if (!popup1.IsOpen && selectedItems.Any())
            {
                popUpGrid.IsReadOnly = true;
                popup1.IsOpen = true;
            }

            Size popupSize = new Size(popup1.ActualWidth, popup1.ActualHeight);
            popup1.PlacementRectangle = new Rect(e.GetPosition(this), popupSize);

            popUpGrid.Width = PersonGrid.RenderSize.Width*0.80;
            popUpGrid.ItemsSource = selectedItems;
          
            ResetDropBackGround();
            var targetRowIndex = dataContext.PersonCollection.IndexOf((e.OriginalSource as FrameworkElement).DataContext as Person);
            if (targetRowIndex < 0)
                return;
            var selectedItemsIndexes = selectedItems.Select(x => dataContext.PersonCollection.IndexOf(x)).OrderBy(x => x).ToList();
            var minIndex = selectedItemsIndexes.Min();
            if (targetRowIndex == 0)
                dataContext.PersonCollection[targetRowIndex].RowEffect = DragRowEffect.Before;
            else if (minIndex > targetRowIndex)
                dataContext.PersonCollection[targetRowIndex].RowEffect = DragRowEffect.Before;
            else
                dataContext.PersonCollection[targetRowIndex].RowEffect = DragRowEffect.After;
        }

        private void PersonGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (droppingInProgress)
            {
                return;
            }
            var rows = GetDataGridRows(PersonGrid);
            foreach (DataGridRow row in rows)
            {
                var currItem = (row.Item as Person);
                currItem.IsSelected = row.IsSelected ? true : false;
            }
        }

        private void PersonGrid_Drop(object sender, DragEventArgs e)
        {
            var vm = (sender as DataGrid).DataContext as MainViewModel;
            var selectedItems = vm.PersonCollection.Where(x => x.IsSelected);
            if (!selectedItems.Any())
                return;
            var oldIndices = selectedItems.Select(x => vm.PersonCollection.IndexOf(x)).OrderBy(x => x).ToList();
            int droppedIndex = vm.PersonCollection.IndexOf((e.OriginalSource as FrameworkElement).DataContext as Person);
            if (oldIndices.Contains(droppedIndex) || droppedIndex == -1)
            {
                return;
            }
            if (droppedIndex < oldIndices.Min())
            {
                selectedItems.Reverse();
            }
            foreach (var item in selectedItems.ToList())
            {
                droppingInProgress = true;
                vm.PersonCollection.Remove(item);
                if (droppedIndex > vm.PersonCollection.Count - 1)
                {
                    vm.PersonCollection.Add(item);
                }
                else
                {
                    vm.PersonCollection.Insert(droppedIndex, item);
                }
                droppingInProgress = false;
            }
            ResetDragDrop();
        }

        private IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row != null)
                {
                    yield return row;
                }
            }
        }

        /// <summary>
        /// Hide the selected row flying popup after drop
        /// </summary>
        private void ResetDragDrop()
        {
            popup1.IsOpen = false;
            ResetDropBackGround();
        }

        /// <summary>
        /// Reset the drag indicator effect to NONE
        /// </summary>
        private void ResetDropBackGround()
        {
            (DataContext as MainViewModel).PersonCollection.All(x => {
                x.RowEffect = DragRowEffect.None;
                return true;
            });
        }

        private void Image_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var vm = (DataContext as MainViewModel);
                if (!vm.PersonCollection.Any(person => person.IsSelected))
                {
                    return;
                }
                var selectedItems = vm.PersonCollection.Where(x => x.IsSelected).ToList();
                DragDrop.DoDragDrop(PersonGrid, selectedItems, DragDropEffects.Move);
            }
            else if (e.LeftButton == MouseButtonState.Released)
            {
                ResetDragDrop();
            }
        }
    }
}
