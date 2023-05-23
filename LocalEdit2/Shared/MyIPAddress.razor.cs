using Blazorise;
using LocalEdit2.ErrorHandling;
using LocalEdit2.IPAddressTypes;
using LocalEdit2.LaunchTypes;
using Microsoft.AspNetCore.Components;
using static LocalEdit2.Pages.FetchData;
using static System.Net.WebRequestMethods;

namespace LocalEdit2.Shared
{
    public partial class MyIPAddress
    {
        string ipAddress = "?.?.?.?";

        AddressInfo addressInfo;

        // Dynamicly inject from our DI container
        [Inject]
        public IAddressDataService? AddressDataService { get; set; }

        [Inject]
        public ILaunchItemDataService? LaunchItemDataService { get; set; }

        [CascadingParameter(Name = "ErrorComponent")]
        protected IErrorComponent ErrorComponent { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        // This is the function that w will use to call functions that will be initializing data
        protected override async Task OnInitializedAsync()
        {
            //base.OnInitializedAsync();

            if(LaunchItemDataService != null)
            {
                //try
                //{
                //    var v = (await LaunchItemDataService.GetAllItems());
                //}
                //catch (Exception e)
                //{
                //    ErrorComponent.ShowError(e.Message, e.StackTrace);
                //}
            }
            

            // Call our data service which call the API
            if (AddressDataService != null)
            {
                //addressInfo = (await AddressDataService.GetIPAddressInfo());
                //ipAddress = addressInfo.ip;
            }
        }

    }
}
