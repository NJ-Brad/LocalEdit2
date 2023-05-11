using Blazor.DownloadFileFast.Interfaces;
using Blazorise;
using Blazorise.DataGrid;
using LocalEdit2.Modals;
using Microsoft.AspNetCore.Components;

namespace LocalEdit2.Shared
{
    public partial class WIPManager
    {
        [Inject]
        public Blazored.LocalStorage.ILocalStorageService localStorage { get; set; }

        [Parameter] 
        public EventCallback DataRequired { get; set; }

        [Parameter]
        public EventCallback DataReady { get; set; }

        [Parameter]
        public string EditorName { get; set; } = "EDITOR";

        [Parameter]
        public int Interval
        {
            get;
            set;
        }

        public async Task <bool> DataExists()
        {
            data = await localStorage.GetItemAsStringAsync($"{EditorName}_WIP");

            return data != null;
        }

        private string data = "";
        public string Data { get => data;
            set
            { 
                data = value;
                if (prevData != data)
                {
                    _ = localStorage.SetItemAsStringAsync($"{EditorName}_WIP", data);
                    //DisplayText = "Saved";
//                    DateTime calcCurrentTime = LocalEdit2.Shared.DateTimeExtension.ToRealLocalTime(DateTime.Now);
//                    DisplayText = $"Last Saved: {calcCurrentTime.ToShortTimeString()}";
                }

                // This should reflect "last attempted save", whether there was an actual update or not
                DateTime calcCurrentTime = LocalEdit2.Shared.DateTimeExtension.ToRealLocalTime(DateTime.Now);
                DisplayText = $"Last Saved: {calcCurrentTime.ToShortTimeString()}";
            }
        }

        public async Task Load()
        {
//            await localStorage.SetItemAsync("name", "John Smith");
            data = await localStorage.GetItemAsStringAsync($"{EditorName}_WIP");

            _ = OnDataReady();
        }

        private async Task OnDataReady()
        {
            if (DataReady.HasDelegate)
            {
                await this.DataReady.InvokeAsync();
            }
            await base.InvokeAsync(new Action(this.StateHasChanged));
        }

        //protected Modal? modalRef;
        //private bool cancelClose;
        //protected bool modalVisible;
        ////private bool cancelled = false;
        ////private bool validationRequired = false;

        //public ModalResult Result { get; protected set; }

        //[Parameter] public EventCallback Closed { get; set; }

        //public Task ShowModal()
        //{
        //    modalVisible = true;

        //    InvokeAsync(() => StateHasChanged());

        //    return Task.CompletedTask;
        //}

        //protected Task OnModalClosing(ModalClosingEventArgs e)
        //{
        //    // just set Cancel to prevent modal from closing

        //    if (e.CloseReason == CloseReason.EscapeClosing)
        //    {
        //        Result = ModalResult.Cancel;

        //        CloseModal();
        //    }

        //    //            if (cancelClose || e.CloseReason != CloseReason.UserClosing)
        //    if (cancelClose)
        //    {
        //        e.Cancel = true;
        //    }

        //    // reset - This covers the case where a user clicks Save (and gets an error), then clicks escape
        //    //validationRequired = false;
        //    cancelClose = false;

        //    return Task.CompletedTask;
        //}

        //protected Task CloseModal()
        //{
        //    // possibly add a chack for changed and prompt to lose changes

        //    cancelClose = false;
        //    //cancelled = true;
        //    Result = ModalResult.Cancel;

        //    if (modalRef != null)
        //        modalRef.Hide();

        //    return Closed.InvokeAsync();
        //}

        //protected async Task OnModalOpened()
        //{
        //    // reset, for the next attempt to close
        //    cancelClose = false;
        //    //cancelled = false;
        //    await ResetValidation();
        //    await Opened();
        //}

        //public virtual async Task Opened()
        //{
        //    await InvokeAsync(() => StateHasChanged());
        //}

        //public Validations? validations { get; set; }
        //public virtual async Task<bool> Validate()
        //{
        //    bool rtnVal = false;
        //    if (validations != null)
        //    {
        //        if (await validations.ValidateAll())
        //        {
        //            rtnVal = true;
        //        }
        //    }
        //    else
        //        rtnVal = true;

        //    return rtnVal;
        //}

        //public virtual async Task ResetValidation()
        //{
        //    if (validations != null)
        //        await validations.ClearAll();
        //}


        //protected async Task TryCloseModal()
        //{
        //    // add a check for validity
        //    //validationRequired = true;

        //    Result = ModalResult.OK;
        //    cancelClose = true;

        //    //            if (await propEditor.IsValid())
        //    if (await Validate())
        //    {
        //        cancelClose = false;
        //    }

        //    if (modalRef != null)
        //        await modalRef.Hide();

        //    await Closed.InvokeAsync();
        //}


        private Timer? _timer;

        //private DateTime startTime;

        //private DateTime stopTime;

        ////private double elapsedtime;

        private bool running = false;

        //[Parameter]
        //public EventCallback Ticked
        //{
        //    get;
        //    set;
        //}

        //public BlazorTimer()
        //{
        //}

        public void Dispose()
        {
            if (_timer != null)
            {
                Timer timer = _timer;
                if (timer != null)
                {
                    timer.Dispose();
                    GC.SuppressFinalize(this);
                }
                else
                {
                }
            }
        }

        //public double ElapsedTimeSecs()
        //{
        //    TimeSpan timeSpan;
        //    timeSpan = (!this.running ? this.stopTime - this.startTime : DateTime.Now - this.startTime);
        //    return timeSpan.TotalSeconds;
        //}

        protected override void OnInitialized()
        {
            base.OnInitialized();
            //this.Start();
        }

        public void Start()
        {
            if (this._timer != null)
            {
                Timer timer = this._timer;
                if (timer != null)
                {
                    timer.Dispose();
                }
                else
                {
                }
                this._timer = null;
            }
            if (TimerTicked != null)
            {
                this._timer = new Timer(new TimerCallback(TimerTicked), null, 0, this.Interval * 1000);
            }
            //this.startTime = DateTime.Now;
            this.running = true;
        }

        public void Stop()
        {
            //stopTime = DateTime.Now;
            if (_timer != null)
            {
                Timer timer = _timer;
                if (timer != null)
                {
                    timer.Dispose();
                }
                else
                {
                }
            }
            _timer = null;
        }

        ////private void Test()
        ////{
        ////    this.Ticked.InvokeAsync();
        ////}

        string DisplayText { get; set; } = string.Empty;

        private string prevData = string.Empty;

        private async void TimerTicked(object? state)
        {
            if (DataRequired.HasDelegate)
            {
                prevData = data;
                await this.DataRequired.InvokeAsync();
            }
            await base.InvokeAsync(new Action(this.StateHasChanged));
        }
    }
}
