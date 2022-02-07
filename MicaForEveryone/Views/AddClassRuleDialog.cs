﻿using Microsoft.Extensions.DependencyInjection;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

using MicaForEveryone.Interfaces;
using MicaForEveryone.UI;
using MicaForEveryone.UI.ViewModels;
using MicaForEveryone.ViewModels;

namespace MicaForEveryone.Views
{
    internal class AddClassRuleDialog : ContentDialog
    {
        public AddClassRuleDialog() : 
            this(new(), Program.CurrentApp.Container.GetService<IAddClassRuleViewModel>())
        {
        }

        private AddClassRuleDialog(ContentDialogView view, IAddClassRuleViewModel viewModel) :
            base(view, viewModel)
        {
            var resources = ResourceLoader.GetForCurrentView();
            Title = resources.GetString("AddRuleDialog/Title");
            Width = 400;
            Height = 300;

            ViewModel = viewModel;
            ViewModel.Title = resources.GetString("AddClassRuleContentDialog/Title");

            var element = (AutoSuggestBox)XamlReader.Load(@"
<AutoSuggestBox xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'
                xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
                x:Uid='ClassNameSuggestBox'
                HorizontalAlignment='Stretch'
                ItemsSource='{Binding Suggestions}'
                Text='{Binding ClassName, Mode=TwoWay}' />
"); 
            element.DataContext = ViewModel;
            element.QuerySubmitted += (sender, args) =>
            {
                ViewModel.PrimaryCommand.Execute(this);
            };
            element.SuggestionChosen += (sender, args) =>
            {
                ViewModel.ClassName = args.SelectedItem.ToString();
            };
            ViewModel.Content = element;

            ViewModel.IsPrimaryButtonEnabled = true;
            ViewModel.PrimaryButtonContent = resources.GetString("AddRuleButton/Content");
            ViewModel.PrimaryCommandParameter = this;

            ViewModel.IsSecondaryButtonEnabled = true;
            ViewModel.SecondaryButtonContent = resources.GetString("CancelButton/Content");
            ViewModel.SecondaryCommand = CloseDialogCommand;
            ViewModel.SecondaryCommandParameter = this;
        }

        public new IAddClassRuleViewModel ViewModel { get; }
    }
}