using Bandydos.Dto;
using BandydosMobile.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;

namespace BandydosMobile;

public partial class EventsPage : ContentPage
{
    private readonly EventsViewModel _viewModel;

    public EventsPage(EventsViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        _viewModel.OnAppearing();
    }
}
