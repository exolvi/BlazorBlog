using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using BlazorBlogAuthor.Models;

namespace BlazorBlogAuthor.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewItemPage : ContentPage
    {
        public Post Item { get; set; }

        public NewItemPage(Post post)
        {
            InitializeComponent();

            Item = post;

            BindingContext = this;
        }

        public NewItemPage()
        {
            InitializeComponent();

            Item = new Post
            {
                Title = "",
                Author = "Bob Crawford",
                Body = "",
                IsPublished = false,
                Id = Guid.Empty,
                Tags = ""
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            if (Item.Id == Guid.Empty)
            {
                Item.Id = Guid.NewGuid();
                MessagingCenter.Send(this, "AddItem", Item);
            }
            else
            {
                MessagingCenter.Send(this, "EditItem", Item);
            }
            
            await Navigation.PopModalAsync(true);
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }

        async void Delete_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "DeleteItem", Item);
            if (Navigation.ModalStack.Count > 0)
            {
                await Navigation.PopModalAsync(true);
            }
            else
            {
                await Navigation.PopAsync(true);
            }
        }
    }
}