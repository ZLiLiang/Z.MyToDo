using ImTools;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.MyToDo.Common;
using Z.MyToDo.Common.Models;
using Z.MyToDo.Extensions;
using Z.MyToDo.Service;
using Z.MyToDo.Shared.Dtos;

namespace Z.MyToDo.ViewModels
{
    public class IndexViewModel : NavigationViewModel
    {
        private readonly IToDoService toDoService;
        private readonly IMemoService memoService;
        private readonly IDialogHostService dialog;
        private readonly IRegionManager regionManager;

        public IndexViewModel(IContainerProvider containerProvider, IDialogHostService dialog) : base(containerProvider)
        {

        }

        #region 委托命令

        public DelegateCommand<ToDoDto> ToDoCompltedCommand { get; private set; }

        public DelegateCommand<ToDoDto> EditToDoCommand { get; private set; }

        public DelegateCommand<MemoDto> EditMemoCommand { get; private set; }

        public DelegateCommand<string> ExecuteCommand { get; private set; }

        public DelegateCommand<TaskBar> NavigateCommand { get; private set; }

        #endregion

        #region 属性

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<TaskBar> taskBars;

        public ObservableCollection<TaskBar> TaskBars
        {
            get { return taskBars; }
            set { taskBars = value; RaisePropertyChanged(); }
        }

        private SummaryDto summary;

        /// <summary>
        /// 首页统计
        /// </summary>
        public SummaryDto Summary
        {
            get { return summary; }
            set { summary = value; RaisePropertyChanged(); }
        }


        #endregion

        private void Navigate(TaskBar obj)
        {
            if (string.IsNullOrWhiteSpace(obj.Target))
            {
                return;
            }

            NavigationParameters param = new();

            if (obj.Title == "已完成")
            {
                param.Add("Value", 2);
            }

            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.Target, param);
        }

        private async void Complted(ToDoDto obj)
        {
            try
            {
                UpdateLoading(true);
                var updateResult = await toDoService.UpdateAsync(obj);
                if (updateResult.Status)
                {
                    var todo = summary.ToDoList.FirstOrDefault(t => t.Id.Equals(obj.Id));
                    if (todo != null)
                    {
                        summary.ToDoList.Remove(todo);
                        summary.CompletedCount += 1;
                        summary.CompletedRatio = (summary.CompletedCount / (double)summary.Sum).ToString("0%");
                        this.Refresh();
                    }
                    aggregator.SendMessage("已完成!");
                }
            }
            finally
            {
                UpdateLoading(false);
            }
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "新增待办": AddToDo(null); break;
                case "新增备忘录": break;
            }
        }

        /// <summary>
        /// 添加待办事项
        /// </summary>
        /// <param name="model"></param>
        private async void AddToDo(ToDoDto model)
        {
            DialogParameters param = new();
            if (model != null)
            {
                param.Add("Value", model);
            }

            var dialogResult = await dialog.ShowDialog("AddToDoView", param);

            if (dialogResult.Result == ButtonResult.OK)
            {
                try
                {
                    UpdateLoading(true);
                    var todo = dialogResult.Parameters.GetValue<ToDoDto>("Value");
                    if (todo.Id > 0)
                    {
                        var updateResult = await toDoService.UpdateAsync(todo);
                        if (updateResult.Status)
                        {
                            var todoModel = summary.ToDoList.FirstOrDefault(t => t.Id.Equals(todo.Id));
                            if (todoModel != null)
                            {
                                todoModel.Title = todo.Title;
                                todoModel.Content = todo.Content;
                            }
                        }
                    }
                    else
                    {
                        var addRsult = await toDoService.AddAsync(todo);
                        if (addRsult.Status)
                        {
                            summary.Sum += 1;
                            summary.ToDoList.Add(addRsult.Result);
                            summary.CompletedRatio = (summary.CompletedCount / (double)summary.Sum).ToString("0%");
                            this.Refresh();
                        }
                    }
                }
                finally
                {
                    UpdateLoading(false);
                }
            }
        }

        /// <summary>
        /// 添加备忘录
        /// </summary>
        /// <param name="model"></param>
        private async void AddMemo(MemoDto model)
        {
            DialogParameters param = new DialogParameters();
            if (model != null)
            {
                param.Add("Value", model);
            }

            var dialogResult = await dialog.ShowDialog("AddMemoView", param);
            if (dialogResult.Result == ButtonResult.OK)
            {
                try
                {
                    UpdateLoading(true);
                    var memo = dialogResult.Parameters.GetValue<MemoDto>("Value");

                    if (memo.Id > 0)
                    {
                        var updateResult = await memoService.UpdateAsync(memo);
                        if (updateResult.Status)
                        {
                            var todoModel = summary.MemoList.FirstOrDefault(t => t.Id.Equals(memo.Id));
                            if (todoModel != null)
                            {
                                todoModel.Title = memo.Title;
                                todoModel.Content = memo.Content;
                            }
                        }
                    }
                    else
                    {
                        var addResult = await memoService.AddAsync(memo);
                        if (addResult.Status)
                        {
                            summary.MemoList.Add(addResult.Result);
                        }
                    }
                }
                finally
                {
                    UpdateLoading(false);
                }
            }
        }

        private void CreateTaskBars()
        {
            TaskBars = new ObservableCollection<TaskBar>
            {
                new TaskBar {Icon="ClockFast",Title="汇总",Color="#FF0CA0FF",Target="ToDoView"},
                new TaskBar {Icon="ClockCheckOutline",Title="已完成",Color="#FF1ECA3A",Target="ToDoView"},
                new TaskBar {Icon="ChartLineVariant",Title="完成比例",Color="#FF02C6DC",Target=""},
                new TaskBar {Icon="PlaylistStar",Title="备忘录",Color="#FFFFA000",Target="MemoView"}
            };

        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            var summaryResult = await toDoService.SummaryAsync();
            if (summaryResult.Status)
            {
                Summary = summaryResult.Result;
                this.Refresh();
            }
            base.OnNavigatedTo(navigationContext);
        }

        private void Refresh()
        {
            TaskBars[0].Content = summary.Sum.ToString();
            TaskBars[1].Content = summary.CompletedCount.ToString();
            TaskBars[2].Content = summary.CompletedRatio;
            TaskBars[3].Content = summary.MemoeCount.ToString();
        }
    }
}
