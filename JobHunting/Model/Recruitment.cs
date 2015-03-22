
using System;
using System.Collections.ObjectModel;

namespace JobHunting.Model
{
    public class Recruitment : PropertyChangedBase
    {
        private string _company;
        private string _site;
        private DateTime _startDate;
        private DateTime _endDate;
        private RecruitType _recruitType;
        private ScreeingStep _screeningStep;

        public string Company
        {
            set
            {
                _company = value;
                OnPropertyChanged("Company");
            }
            get { return _company; }
        }
        public string Site
        {
            set
            {
                _site = value;
                OnPropertyChanged("Site");
            }
            get { return _site; }
        }

        public DateTime StartDate
        {
            set
            {
                _startDate = value;
               OnPropertyChanged("StartDate");
            }
            get { return _startDate; }
        }

        public DateTime EndDate
        {
            set
            {
                _endDate = value;
                OnPropertyChanged("EndDate");
            }
            get { return _endDate; }
        }
        public RecruitType RecruitType
        {
            set
            {
                _recruitType = value;
                OnPropertyChanged("RecruitType");
            }
            get { return _recruitType; }
        }
        public ScreeingStep ScreeingStep
        {
            set
            {
                _screeningStep = value;
                OnPropertyChanged("ScreeningStep");
            }
            get { return _screeningStep; }
        }

        public ObservableCollection<Question> Questions { set; get; }

        public Recruitment()
        {
            Questions = new ObservableCollection<Question>();
        }
    }
}
