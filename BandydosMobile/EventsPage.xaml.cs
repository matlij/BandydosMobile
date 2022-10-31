using Bandydos.Dto;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;

namespace BandydosMobile;

public partial class EventsPage : ContentPage
{
    private readonly EventsViewModel _viewModel;

    public EventsPage()
	{
		InitializeComponent();

        BindingContext = _viewModel = new EventsViewModel();
    }

    protected override async void OnAppearing()
    {
        await _viewModel.RefreshDataAsync();
    }
}

public static class Constants
{
    public const string RestUrl = "https://bandydosapi.azurewebsites.net/api";
    public const string Code = "j4qDYPfWu-AwM4-6685Jms49wN0L2TXjkdRsvlwV3M4GAzFuiE6pcw==";
}

public class EventsViewModel
{
	private readonly HttpClient _client;


	public ObservableCollection<EventDto> Items { get; } = new ObservableCollection<EventDto>();

	public EventsViewModel()
	{
		_client = new HttpClient();
	}

    public async Task RefreshDataAsync()
    {
        Items.Clear();

        var uri = new Uri($"{Constants.RestUrl}/event?code={Constants.Code}");
        try
        {
            HttpResponseMessage response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var items = JsonSerializer.Deserialize<ObservableCollection<EventDto>>(content);

                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
        }
    }

}