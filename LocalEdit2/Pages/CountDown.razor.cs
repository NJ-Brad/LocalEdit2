using LocalEdit2.ErrorHandling;
using LocalEdit2.Modals;
using LocalEdit2.Shared;
using Microsoft.AspNetCore.Components;

namespace LocalEdit2.Pages
{
    public partial class CountDown
    {
        string title = "Meeting Countdown";

        string currentTime = "Now";

        BlazorTimer? timerRef = null;
        BigText? bigTextRef = null;
        bool started = false;

        TimeSpan? startTime = null;
        TimeSpan? endTime = null;
        int? warningMinutes = 5;
        //    TimeSpan? remainingTime = null;

        CountDownModal? countDownModalRef = null;

        [CascadingParameter(Name = "ErrorComponent")]
        //[CascadingParameter]
        public IErrorComponent? ErrorComponent { get; set; }

        //[CascadingParameter(Name = "TestValue")]
        [CascadingParameter]
        public string? testValue { get; set; }

        private Task Configure()
        {
            //try
            //{
            //    throw new Exception("This is a sample Exception");
            //}
            //catch (Exception e)
            //{
            //    ErrorComponent.ShowError(e.Message, e.StackTrace);
            //}

            countDownModalRef?.ShowModal();

            return Task.CompletedTask;
        }


        private Task OnCountDownModalClosed()
        {
            // remove the new item, if add was cancelled
            if (countDownModalRef?.Result == ModalResult.Cancel)
            {
                //Document.Items.Remove(selectedItemRow);
                //selectedItemRow = null;
            }
            else
            {
                startTime = countDownModalRef?.Configuration.StartTime;
                endTime = countDownModalRef?.Configuration.EndTime;
                warningMinutes = countDownModalRef?.Configuration.WarningMinutes;
                started = true;
            }
            //        adding = false;

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }


        private void ButtonClicked()
        {
            started = true;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }


        Task OnTimerTicked()
        {
            //        TimeZoneInfo tz = TimeZoneInfo.Local;

            DateTime calcCurrentTime = LocalEdit2.Shared.DateTimeExtension.ToRealLocalTime(DateTime.Now);

            if (!started)
            {
                bigTextRef?.RenderText("Meeting has not started yet.");
                title = "Meeting Countdown";
            }
            //else if (DateTime.Now.TimeOfDay < startTime)
            else if (calcCurrentTime.TimeOfDay < startTime)
            {
                bigTextRef?.RenderText("Meeting has not started yet.");
                title = "Meeting has not started yet.";
            }
            //else if (DateTime.Now.TimeOfDay > endTime)
            else if (calcCurrentTime.TimeOfDay > endTime)
            {
                bigTextRef?.RenderText("Meeting should have ended by now.");
                title = "Overtime";
            }
            //else if ((DateTime.Now.TimeOfDay > startTime) && (DateTime.Now.TimeOfDay < endTime))
            else if ((calcCurrentTime.TimeOfDay > startTime) && (calcCurrentTime.TimeOfDay < endTime))
            {
                //TimeSpan remainingTime = endTime.Value - DateTime.Now.TimeOfDay;
                TimeSpan remainingTime = endTime.Value - calcCurrentTime.TimeOfDay;
                bigTextRef?.RenderText(remainingTime.ToString(@"hh\:mm\:ss"), (remainingTime.TotalMinutes < warningMinutes) ? "red" : "black");
                title = remainingTime.ToString(@"hh\:mm\:ss");
            }

            //if (fileManagementModalRef.Result == ModalResult.OK)
            //{
            //    Document = (C4Workspace)JsonSerializer.Deserialize(fileManagementModalRef.FileText, typeof(C4Workspace));
            //    InvokeAsync(() => StateHasChanged());
            //}

            currentTime = calcCurrentTime.ToString();
            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }
    }
}
