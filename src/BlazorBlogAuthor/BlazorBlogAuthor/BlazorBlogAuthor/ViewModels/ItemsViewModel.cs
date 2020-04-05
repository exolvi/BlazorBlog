using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using BlazorBlogAuthor.Models;
using BlazorBlogAuthor.Views;

namespace BlazorBlogAuthor.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Post> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Post>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Post>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Post;
                Items.Add(newItem);
                await DataStore.WritePostAsync(newItem);
            });

            MessagingCenter.Subscribe<NewItemPage, Post>(this, "EditItem", async (obj, item) =>
            {
                var newItem = item as Post;
                await DataStore.WritePostAsync(newItem);
                //await ExecuteLoadItemsCommand();
            });

            MessagingCenter.Subscribe<NewItemPage, Post>(this, "DeleteItem", async (obj, item) =>
            {
                var newItem = item as Post;
                Items.Remove(newItem);
                await DataStore.DeletePostAsync(newItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetPostsAsync();
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}