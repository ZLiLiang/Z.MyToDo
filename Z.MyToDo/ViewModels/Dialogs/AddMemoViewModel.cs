using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.MyToDo.Common;
using Z.MyToDo.Shared.Dtos;

namespace Z.MyToDo.ViewModels.Dialogs
{
    public class AddMemoViewModel : BindableBase, IDialogHostAware
    {
        private MemoDto model;

        public MemoDto Model
        {
            get { return model; }
            set { model = value; RaisePropertyChanged(); }
        }

        public string DialogHostName { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public AddMemoViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Model.Title) || string.IsNullOrWhiteSpace(Model.Content))
            {
                return;
            }

            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                //确定时,把编辑的实体返回并且返回OK
                DialogParameters param = new DialogParameters
                {
                    {"Value",Model }
                };
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));
            }
        }

        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));
            }
        }

        public void OnDialogOpend(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                Model = parameters.GetValue<MemoDto>("Value");
            }
            else
            {
                Model = new MemoDto();
            }
        }
    }
}
