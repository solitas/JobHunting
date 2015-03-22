using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using JobHunting.Model;

namespace JobHunting.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private Recruitment _selectedRecruitment;
        private Recruitment _AddRecruitment;
        private Question _selectedQuestion;
        private bool _viewMode;
        private bool _isModify;
        private ScreeingStep _screeingStep;
        private RecruitType _recruitType;

        #endregion

        #region Properties
        public ObservableCollection<Recruitment> Recruitments { set; get; }

        public Recruitment SelectedRecruitment
        {
            set
            {
                _selectedRecruitment = value;
                NotifyPropertyChanged("SelectedRecruitment");

            }
            get { return _selectedRecruitment; }
        }

        public Recruitment AddRecruitment
        {
            set
            {
                _AddRecruitment = value;
                NotifyPropertyChanged("AddRecruitment");
            }
            get { return _AddRecruitment; }
        }

        public Question SelectedQuestion
        {
            set
            {
                _selectedQuestion = value;
                NotifyPropertyChanged("SelectedQuestion");
            }
            get { return _selectedQuestion; }
        }

        [DefaultValue(true)]
        public bool ViewMode
        {
            get
            {
                return _viewMode;
            }
            set
            {
                if (value != _viewMode)
                {
                    _viewMode = value;
                    NotifyPropertyChanged("ViewMode");
                }
            }
        }

        public RecruitType RecruitType
        {
            get
            {
                return _recruitType;
            }
            set
            {
                if (value != _recruitType)
                {
                    AddRecruitment.RecruitType = value;
                    _recruitType = value;
                    NotifyPropertyChanged("RecruitType");
                }
            }
        }

        public ScreeingStep ScreeningStep
        {
            get { return _screeingStep; }
            set
            {
                if (value != _screeingStep)
                {
                    AddRecruitment.ScreeingStep = value;
                    _screeingStep = value;
                    NotifyPropertyChanged("ScreeningStep");
                }
            }
        }
        #endregion

        #region Constructor

        public MainViewModel()
        {
            Recruitments = new ObservableCollection<Recruitment>();
            ViewMode = true;
            AddRecruitment = new Recruitment();
        }

        #endregion

        #region Commands

        private RelayCommand _recruitPreperAddCommand;
        private RelayCommand _recruitCancelCommand;
        private RelayCommand _recruitAdditionCommand;
        private RelayCommand _recruitDeleteCommand;
        private RelayCommand _recruitModifyCommand;
        private RelayCommand _questionAdditionCommand;
        private RelayCommand _questionDeleteCommand;
        
        public ICommand RecruitPrepareAddCommand
        {
            get
            {
                return _recruitPreperAddCommand ?? (_recruitPreperAddCommand = new RelayCommand(RecruitPrepareAddHandler));
            }
        }

        public ICommand RecruitCancelCommand
        {
            get { return _recruitCancelCommand ?? (_recruitCancelCommand = new RelayCommand(RecruitCancelHandler)); }
        }
        public ICommand RecruitAdditionCommand
        {
            get
            {
                return _recruitAdditionCommand ?? (_recruitAdditionCommand = new RelayCommand(RecruitAdditionHandler));
            }
        }
        public ICommand RecruitDeleteCommand
        {
            get
            {
                return _recruitDeleteCommand ?? (_recruitDeleteCommand = new RelayCommand(RecruitDeleteHandler));
            }
        }
        public ICommand RecruitModifyCommand
        {
            get
            {
                return _recruitModifyCommand ?? (_recruitModifyCommand = new RelayCommand(RecruitModifyHandler));
            }
        }
        public ICommand QuestionAdditionCommand
        {
            get
            {
                return _questionAdditionCommand ?? (_questionAdditionCommand = new RelayCommand(QuestionAdditionHandler));
            }
        }
        public ICommand QuestionDeleteCommand
        {
            get
            {
                return _questionDeleteCommand ?? (_questionDeleteCommand = new RelayCommand(QuestionDeleteHandler));
            }
        }

        #endregion

        #region Command Handler

        private void RecruitPrepareAddHandler()
        {
            ViewMode = false;
            InitializeAddRecruitment();
        }

        private void RecruitCancelHandler()
        {
            ViewMode = true;
            InitializeAddRecruitment();
        }

        private void RecruitAdditionHandler()
        {
            ViewMode = true;

            if (_isModify)
            {
                _isModify = false;
            }
            else
            {

            }
        }
        private void RecruitDeleteHandler()
        {

        }
        private void RecruitModifyHandler()
        {
            ViewMode = false;
            _isModify = true;
        }
        private void QuestionAdditionHandler()
        {

        }
        private void QuestionDeleteHandler()
        {

        }
        #endregion

        #region Private Methods

        private void InitializeAddRecruitment()
        {
            if (AddRecruitment != null)
            {
                AddRecruitment.Company = string.Empty;
                AddRecruitment.Site = string.Empty;
                AddRecruitment.EndDate = DateTime.Today;
                AddRecruitment.StartDate = DateTime.Today;
                RecruitType = RecruitType.Public;
                ScreeningStep = ScreeingStep.Paper;
            }
        }

        private void InitializeSelectedQuestion()
        {
            
        }
        #endregion
    }
}
