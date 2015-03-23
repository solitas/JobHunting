using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace JobHunting.Model
{
    public class Question : PropertyChangedBase
    {
        private int _index;
        private string _subject;
        private string _content;
        private int _permitCharCount;
        private int _inputCharCount;

        public int Index
        {
            set
            {
                _index = value;
                base.OnPropertyChanged("Index");
            }
            get { return _index; }
        }

        public string Subject
        {
            set
            {
                _subject = value;
                base.OnPropertyChanged("Subject");
            }
            get { return _subject; }
        }
        public string Content
        {
            set
            {
                _content = value;
                base.OnPropertyChanged("Content");
            }
            get { return _content; }
        }
        public int PermitCharCount
        {
            set
            {
                _permitCharCount = value;
                base.OnPropertyChanged("PermitCharCount");
            }
            get { return _permitCharCount; }
        }
        public int InputCharCount
        {
            set
            {
                _inputCharCount = value;
                base.OnPropertyChanged("InputCharCount");
            }
            get { return _inputCharCount; }
        }
        public Dictionary<DateTime, string> ChangeHistory { set; get; }

        public Question(int index)
        {
            Index = index;
            ChangeHistory = new Dictionary<DateTime, string>();
            Subject = "주제";
        }

        public Question(Question question) : this(question.Index)
        {
            Subject = question.Subject;
            Content = question.Content;
            PermitCharCount = question.PermitCharCount;
            InputCharCount = question.InputCharCount;
            ChangeHistory = question.ChangeHistory;

            var enumerator = question.ChangeHistory.GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                var keyPair = enumerator.Current;
                ChangeHistory.Add(keyPair.Key,keyPair.Value);
            }
        }
    }
}
