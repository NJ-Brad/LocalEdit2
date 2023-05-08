using Blazorise;
using LocalEdit.PlanTypes;
using LocalEdit.Shared;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class PlanDependencyModal : LE_ModalBase
    {
        DatePicker<DateTime?>? datePicker;

        DateTime? selectedDate;

        void OnDateChanged(DateTime? date)
        {
            selectedDate = date;
            if (date.HasValue)
            {
                Item.StartDate = date.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                Item.StartDate = string.Empty;
            }
        }

        void ValidateId(ValidatorEventArgs e)
        {
            e.Status = ValidationStatus.Success;
            if (Item.DependencyType == "OTHER")
            {
                if (e.Value != null)
                {
                    string value = Utils.VOD(Convert.ToString(e.Value));

                    //e.Status = string.IsNullOrEmpty(startDate) ? ValidationStatus.None :
                    //    email.Contains("@") ? ValidationStatus.Success : ValidationStatus.Error;
                    e.Status = string.IsNullOrEmpty(value) ? ValidationStatus.Error : ValidationStatus.Success;
                }
            }
            else
            {
                e.Status = ValidationStatus.None;
            }

        }

        void ValidateStartDate(ValidatorEventArgs e)
        {
            e.Status = ValidationStatus.Success;
            if (Item.DependencyType == "DATE")
            {
                DateTime? testVal = (e.Value as dynamic)[0] as DateTime?;

                //dynamic v2 = e.Value;
                //DateTime? v3 = v2[0] as DateTime?;

                if (testVal == DateTime.MinValue)
                {
                    e.Status = ValidationStatus.Error;
                }
            }
            else
            {
                e.Status = ValidationStatus.None;
            }

            //e.Status = string.IsNullOrEmpty(startDate) ? ValidationStatus.None :
            //    email.Contains("@") ? ValidationStatus.Success : ValidationStatus.Error;
        }

        PlanItemDependency? item = null;
        DateTime? StartDateVal { get; set; } = DateTime.Now;

        [Parameter]
        public PlanItemDependency? Item
        {
            get { return item; }
            set
            { 
                item = value;

                if (value != null)
                {
                    if (DateTime.TryParse(value.StartDate, out DateTime asDate))
                    {
                    }
                    if (datePicker != null)
                    {
                        StartDateVal = asDate;
                        //datePicker.Date = asDate;
                    }
                }
            }
        }

        [Parameter]
        public List<PlanItem> Items { get; set; } = new();

    }
}
