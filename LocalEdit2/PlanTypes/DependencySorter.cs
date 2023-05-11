//using Octokit;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;

namespace LocalEdit2.PlanTypes
{
    public class DependencySorter
    {
        // this will put them in order, but at a cost.  It MANGLES the dependencies.
        public static List<PlanItem> Generate(List<PlanItem> originalItemList, DateOnly projectStartDate)
        {
            DateOnly startDate = projectStartDate;
            List<PlanItem> rtnVal = new List<PlanItem>(originalItemList.Count);    // setting the the correct length means that it doesn't have to waste time expanding later
            List<PlanItem> itemsToAdd = new List<PlanItem>(originalItemList);

            int removedItemCount = 0;
            while (itemsToAdd.Count > 0)
            {
                for (int itemNum = 0; itemNum < itemsToAdd.Count; itemNum++)
                {
                    PlanItem item = itemsToAdd[itemNum];
                    // No more un-met dependencies.  Add it to the output
                    //if (item.Dependencies.Count == 0)
                    if (AllDependenciewMet(item, rtnVal))
                    {
                        // set the start date
                        item.StartDate = GetStartDate(item, rtnVal, projectStartDate);

                        // set the end date
                        int days = string.IsNullOrEmpty(item.Duration) ? 1 : int.Parse(item.Duration);
                        item.EndDate = item.StartDate.Value.AddDays(days);

                        rtnVal.Add(item);
                        itemsToAdd.RemoveAt(itemNum);
                        break;  // stop this loop of items to add, and start over
                    }
                }

                //removedItemCount = itemsToRemove.Count;
                //foreach (PlanItem itemToRemove in itemsToRemove)
                //{
                //    startDate = itemToRemove.EndDate.Value.AddDays(1);

                //    foreach (PlanItem item in inFiles)
                //    {
                //        List<PlanItemDependency> depToRemove = new();
                //        foreach (PlanItemDependency dep in item.Dependencies)
                //        {
                //            if (dep.ID == itemToRemove.ID)
                //            {
                //                if((!item.StartDate.HasValue) || (item.StartDate < startDate))
                //                {
                //                    item.StartDate = startDate;
                //                }
                //                depToRemove.Add(dep);
                //            }
                //        }

                //        foreach (PlanItemDependency dep in depToRemove)
                //        {
                //            item.Dependencies.Remove(dep);
                //        }
                //    }

                //    inFiles.Remove(itemToRemove);
                //}
            }
            return rtnVal;
        }

        private static bool AllDependenciewMet(PlanItem item, List<PlanItem> alreadyCreated)
        {
            bool rtnVal = true;

            foreach (PlanItemDependency dep in item.Dependencies)
            {
                bool found = false;
                foreach (PlanItem existing in alreadyCreated)
                {
                    if (existing.ID == dep.ID)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    rtnVal = false;
                }
            }

            return rtnVal;
        }

        private static DateOnly GetStartDate(PlanItem item, List<PlanItem> alreadyCreated, DateOnly projectStartDate)
        {
            DateOnly rtnVal = projectStartDate;

            foreach (PlanItemDependency dep in item.Dependencies)
            {
                foreach (PlanItem existing in alreadyCreated)
                {
                    if (existing.ID == dep.ID)
                    {
                        if (existing.EndDate >= rtnVal)
                        {
                            rtnVal = existing.EndDate.Value.AddDays(1);
                        }
                        //if ((!item.StartDate.HasValue) || (existing.EndDate >= item.StartDate.Value))
                        //{
                        //    item.StartDate = existing.EndDate.Value.AddDays(1);
                        //}
                    }
                }
            }

            return rtnVal;
        }


        // this will put them in order, but at a cost.  It MANGLES the dependencies.
        public static List<PlanItem> GenerateOld(List<PlanItem> originalItemList, DateOnly projectStartDate)
        {
            DateOnly startDate = projectStartDate;
            List<PlanItem> inFiles = new List<PlanItem>(originalItemList);
            List<PlanItem> rtnVal = new List<PlanItem>(originalItemList.Count);    // setting the the correct length means that it doesn't have to waste time expanding later
            List<PlanItem> itemsToRemove = new List<PlanItem>();

            int removedItemCount = 0;
            do
            {
                itemsToRemove.Clear();

                foreach (PlanItem item in inFiles)
                {
                    // No more un-met dependencies.  Add it to the output
                    if (item.Dependencies.Count == 0)
                    {
                        // set the start date
                        item.StartDate = startDate;

                        // set the end date
                        int days = string.IsNullOrEmpty(item.Duration) ? 1 : int.Parse(item.Duration);
                        item.EndDate = item.StartDate.Value.AddDays(days);

                        rtnVal.Add(item);
                        itemsToRemove.Add(item);
                    }
                }

                removedItemCount = itemsToRemove.Count;
                foreach (PlanItem itemToRemove in itemsToRemove)
                {
                    startDate = itemToRemove.EndDate.Value.AddDays(1);

                    foreach (PlanItem item in inFiles)
                    {
                        List<PlanItemDependency> depToRemove = new();
                        foreach (PlanItemDependency dep in item.Dependencies)
                        {
                            if (dep.ID == itemToRemove.ID)
                            {
                                if ((!item.StartDate.HasValue) || (item.StartDate < startDate))
                                {
                                    item.StartDate = startDate;
                                }
                                depToRemove.Add(dep);
                            }
                        }

                        foreach (PlanItemDependency dep in depToRemove)
                        {
                            item.Dependencies.Remove(dep);
                        }
                    }

                    inFiles.Remove(itemToRemove);
                }
            }
            while (removedItemCount > 0);

            return rtnVal;
        }
    }
}
