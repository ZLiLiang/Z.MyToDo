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
using Z.MyToDo.Extensions;
using Z.MyToDo.Service;
using Z.MyToDo.Shared.Dtos;
using Z.MyToDo.Shared.Parameters;

namespace Z.MyToDo.ViewModels
{
    public class ToDoViewModel : NavigationViewModel
    {
        private readonly IDialogHostService dialogHostService;
        private readonly IToDoService toDoService;

        #region 委托命令

        public DelegateCommand<string> ExecuteCommand { get; private set; }

        public DelegateCommand<ToDoDto> SelectedCommand { get; private set; }

        public DelegateCommand<ToDoDto> DeleteCommand { get; private set; }

        #endregion

        #region 属性

        private int selectedIndex;

        /// <summary>
        /// 下拉列表选中状态值
        /// </summary>
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; RaisePropertyChanged(); }
        }

        private string search;

        /// <summary>
        /// 搜索条件
        /// </summary>
        public string Search
        {
            get { return search; }
            set { search = value; RaisePropertyChanged(); }
        }

        private bool isRightDrawerOpen;

        /// <summary>
        /// 右侧编辑窗口是否展开
        /// </summary>
        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }

        private ToDoDto currentDto;

        /// <summary>
        /// 编辑选中/新增时对象
        /// </summary>
        public ToDoDto CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<ToDoDto> toDoDtos;

        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; RaisePropertyChanged(); }
        }

        #endregion

        public ToDoViewModel(IContainerProvider containerProvider, IToDoService toDoService) : base(containerProvider)
        {
            this.toDoService = toDoService;
            this.dialogHostService = containerProvider.Resolve<IDialogHostService>();
            ToDoDtos = new ObservableCollection<ToDoDto>();

            ExecuteCommand = new DelegateCommand<string>(Execute);
            SelectedCommand = new DelegateCommand<ToDoDto>(Selected);
            DeleteCommand = new DelegateCommand<ToDoDto>(Delete);
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "新增": Add(); break;
                case "查询": GetDataAsync(); break;
                case "保存": Save(); break;
            }
        }

        /// <summary>
        /// 添加待办
        /// </summary>
        private void Add()
        {
            CurrentDto = new ToDoDto();
            IsRightDrawerOpen = true;
        }

        private async void Save()
        {
            if (string.IsNullOrWhiteSpace(CurrentDto.Title) ||
                string.IsNullOrWhiteSpace(CurrentDto.Content))
            {
                return;
            }

            UpdateLoading(true);

            try
            {
                if (CurrentDto.Id > 0)
                {
                    var updateResult = await toDoService.UpdateAsync(CurrentDto);
                    if (updateResult.Status)
                    {
                        var todo = ToDoDtos.FirstOrDefault(t => t.Id == CurrentDto.Id);
                        if (todo != null)
                        {
                            todo.Title = CurrentDto.Title;
                            todo.Content = CurrentDto.Content;
                            todo.Status = CurrentDto.Status;
                        }
                    }
                    IsRightDrawerOpen = false;
                }
                else
                {
                    var addResult = await toDoService.AddAsync(CurrentDto);
                    if (addResult.Status)
                    {
                        ToDoDtos.Add(addResult.Result);
                        IsRightDrawerOpen = false;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                UpdateLoading(false);
            }
        }

        private async void Selected(ToDoDto dto)
        {
            try
            {
                UpdateLoading(false);
                var todoResult = await toDoService.GetFirstOfDefaultAsync(dto.Id);
                if (todoResult.Status)
                {
                    CurrentDto = todoResult.Result;
                    IsRightDrawerOpen = true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                UpdateLoading(false);
            }
        }

        private async void Delete(ToDoDto dto)
        {
            try
            {
                var dialogResult = await dialogHostService.Question("温馨提示", $"确认删除待办事项:{dto.Title} ?");
                if (dialogResult.Result != ButtonResult.OK)
                {
                    return;
                }

                UpdateLoading(true);
                var deleteResult = await toDoService.DeleteAsync(dto.Id);
                if (deleteResult.Status)
                {
                    var model = ToDoDtos.FirstOrDefault(t => t.Id.Equals(dto.Id));
                    if (model != null)
                    {
                        ToDoDtos.Remove(model);
                    }
                }
            }
            finally
            {
                UpdateLoading(false);
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        private async void GetDataAsync()
        {
            UpdateLoading(true);

            int? Status = SelectedIndex == 0 ? null : SelectedIndex == 2 ? 1 : 0;

            var todoResult = await toDoService.GetAllFilterAsync(new ToDoParameter
            {
                PageIndex = 0,
                PageSize = 100,
                Search = Search,
                Status = Status
            });

            if (todoResult.Status)
            {
                ToDoDtos.Clear();
                foreach (var item in todoResult.Result.Items)
                {
                    ToDoDtos.Add(item);
                }
            }

            UpdateLoading(false);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            if (navigationContext.Parameters.ContainsKey("Value"))
                SelectedIndex = navigationContext.Parameters.GetValue<int>("Value");
            else
                SelectedIndex = 0;
            GetDataAsync();
        }
    }
}
