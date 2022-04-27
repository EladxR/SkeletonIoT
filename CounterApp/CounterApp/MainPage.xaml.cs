using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.AspNetCore.SignalR.Client;


namespace CounterApp
{
    public partial class MainPage : ContentPage
    {
        HubConnection connection;

        public MainPage()
        {
            InitializeComponent();

            // establish a connection
            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:53353/ChatHub")
                .Build();

            connection.On<string>("Send", (message) =>
            {
                AppendMessage(message);
            });


            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            

        }

        public async Task GetCounterAsync()
        {
            await connection.StartAsync();
        }
    }
}
