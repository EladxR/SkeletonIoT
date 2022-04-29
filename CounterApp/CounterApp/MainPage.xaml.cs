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
        HubConnection hubConnection;

        public MainPage()
        {
            InitializeComponent();
            SyncCounters();
        }



        async private void SyncCounters()
        {
            SignalRSetup();
            await SignalRConnect();
        }

        private void SignalRSetup()
        {
            hubConnection = new HubConnectionBuilder().WithUrl($"https://functionapp220220419172755.azurewebsites.net/api/").Build();
            hubConnection.On<string>("newMessage", (message) =>
            {
                var receivedMessage = $"{message}";
                label1.Text = receivedMessage;
                //label2.Text = "Current counter 2 is: 0";
            });

        }

        async Task SignalRConnect()
        {
            
            await hubConnection.StartAsync();
            
            
        }

    }
}
