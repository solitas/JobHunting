﻿
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
        private ScreeningStep _screeningStep;

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
        public ScreeningStep ScreeningStep
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

        public Recruitment(Recruitment recruitment) : this()
        {
            _recruitType = recruitment.RecruitType;
            _company = recruitment.Company;
            _startDate = recruitment.StartDate;
            _endDate = recruitment.EndDate;
            _site = recruitment.Site;
            _screeningStep = recruitment.ScreeningStep;
            
            foreach (var question in recruitment.Questions)
            {
                Questions.Add(new Question(question));    
            }
        }
    }
}
