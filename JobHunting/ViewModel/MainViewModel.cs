using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using JobHunting.Data;
using JobHunting.Model;

namespace JobHunting.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        private IRepository _repository;
        private Recruitment _selectedRecruitment;
        private Recruitment _addRecruitment;
        private Question _selectedQuestion;
        private bool _viewMode;
        private bool _isModify;
        private ScreeingStep _screeingStep;
        private RecruitType _recruitType;

        #endregion

        #region Properties

        private ObservableCollection<Recruitment> _recruitments; 
        public ObservableCollection<Recruitment> Recruitments
        {
            set
            {
                _recruitments = value;
                NotifyPropertyChanged("Recruitments");
            }
            get
            {
                return _recruitments;
            }
        }

        public Recruitment SelectedRecruitment
        {
            set
            {
                _selectedRecruitment = value;
                if (_selectedRecruitment.Questions.Count > 0)
                    SelectedQuestion = _selectedRecruitment.Questions[0];
                NotifyPropertyChanged("SelectedRecruitment");

            }
            get { return _selectedRecruitment; }
        }

        public Recruitment AddRecruitment
        {
            set
            {
                _addRecruitment = value;
                NotifyPropertyChanged("AddRecruitment");
            }
            get { return _addRecruitment; }
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

        public MainViewModel(IRepository repository)
        {
            _repository = repository;
            
            ViewMode = true;
            AddRecruitment = new Recruitment();
            Recruitments = new ObservableCollection<Recruitment>();
            LoadRecruitment();
        }
        private void LoadRecruitment()
        {
            Recruitments = _repository.LoadRecruitments().ToObservableCollection();
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
        private RelayCommand _questionSaveCommand;
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

        public ICommand QuestionSaveCommand
        {
            get { return _questionSaveCommand ?? (_questionSaveCommand = new RelayCommand(QuestionSaveHandler)); }
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
                Recruitment newRecruitment = new Recruitment(_addRecruitment); // 복사
                AddRecruitment = new Recruitment();
                Recruitments.Add(newRecruitment);
                _repository.SaveRecruitments(_recruitments);
            }
        }
        private void RecruitDeleteHandler()
        {
            if (_selectedRecruitment != null)
            {
                Recruitments.Remove(_selectedRecruitment);
                _selectedRecruitment = null;
                _repository.SaveRecruitments(_recruitments);
            }
        }

        private void RecruitModifyHandler()
        {
            if (SelectedRecruitment == null)
            {
                MessageBox.Show("Not selected item");
                return;
            }

            ViewMode = false;
            _isModify = true;
            AddRecruitment = SelectedRecruitment;

            _repository.SaveRecruitments(_recruitments);
        }
        private void QuestionAdditionHandler()
        {
            if (_selectedRecruitment == null)
            {
                return;
            }
            int index = _selectedRecruitment.Questions.Count;

            Question newQuestion = new Question(index+1);
            _selectedRecruitment.Questions.Add(newQuestion);
            SelectedQuestion = newQuestion;

            _repository.SaveRecruitments(_recruitments);
        }

        private void QuestionSaveHandler()
        {
            if (_selectedRecruitment == null)
            {
                return;
            }
            _repository.SaveRecruitments(_recruitments);
        }

        private void QuestionDeleteHandler()
        {
            _repository.SaveRecruitments(_recruitments);
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
