using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace JobHunting.Control
{
    public class CountRichTextBox : RichTextBox, INotifyPropertyChanged
    {
        public static DependencyProperty CharactersRemainingProperty;
        public static DependencyProperty MaxCharactersAllowedProperty;
        public static DependencyProperty NotiftyLimitProperty;
        public static DependencyProperty NotificationStyleNameProperty;
        public static DependencyProperty NotificationStyleProperty;
        public static DependencyProperty DefaultNotificationStyleNameProperty;
        public static DependencyProperty IsValidProperty;

        static CountRichTextBox()
        {
            CharactersRemainingProperty = DependencyProperty.Register("CharactersRemaining", typeof(int),
                                                                    typeof(CountRichTextBox));


            MaxCharactersAllowedProperty = DependencyProperty.Register("MaxCharactersAllowed", typeof(int),
                                                                       typeof(CountRichTextBox), new PropertyMetadata(0, MaxCharactersAllowedPropertyChanged));

            NotiftyLimitProperty = DependencyProperty.Register("NotifyLimit", typeof(int), typeof(CountRichTextBox));

            NotificationStyleNameProperty = DependencyProperty.Register("NotificationStyleName", typeof(String),
                                                               typeof(CountRichTextBox));

            NotificationStyleProperty = DependencyProperty.Register("NotificationStyle", typeof(Style),
                                                                    typeof(CountRichTextBox));

            DefaultNotificationStyleNameProperty = DependencyProperty.Register("DefaultNotificationStyleName",
                                                                               typeof(String),
                                                                               typeof(CountRichTextBox), new PropertyMetadata(null, DefaultNotificationStyleNamePropertyChanged));

            IsValidProperty = DependencyProperty.Register("IsValid", typeof(bool), typeof(CountRichTextBox));
        }
        /// <summary>
        /// Fired when the style name property is assigned from xaml 
        /// </summary>
        /// <param name="d">Dependency Object</param>
        /// <param name="e">Event Args</param>
        private static void DefaultNotificationStyleNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sharpRichTextBox = d as CountRichTextBox;

            if (sharpRichTextBox == null) return;

            sharpRichTextBox.NotificationStyle = (Style)sharpRichTextBox.FindResource(sharpRichTextBox.DefaultNotificationStyleName);
        }

        /// <summary>
        /// Fired when the max characters property is changed from the xaml view 
        /// </summary>
        /// <param name="d">Dependency object</param>
        /// <param name="e">event args</param>
        private static void MaxCharactersAllowedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // set the characters remaining property
            var sharpRichTextBox = d as CountRichTextBox;

            if (sharpRichTextBox == null) return;

            // inialize the characters remaining with the max characters allowed
            sharpRichTextBox.CharactersRemaining = sharpRichTextBox.MaxCharactersAllowed;
        }

        /// <summary>
        /// This method is used to setup the watermark background for the RichTextBox 
        /// </summary>
        private void SetUpBindingForBackground()
        {
            var visualBrush = new VisualBrush();
            var stackPanel = new StackPanel();
            var textBlock = new TextBlock() { Text = CharactersRemaining.ToString(), Opacity = 0.2 };

            var styleBinding = new Binding("NotificationStyle");
            styleBinding.Source = this;

            var binding = new Binding("CharactersRemaining");
            binding.Source = this;

            textBlock.SetBinding(TextBlock.TextProperty, binding);
            textBlock.SetBinding(StyleProperty, styleBinding);

            stackPanel.Children.Add(textBlock);

            visualBrush.Visual = stackPanel;

            Background = visualBrush;
        }

        public CountRichTextBox()
        {

            PreviewKeyUp += new KeyEventHandler(SharpRichTextBox_PreviewKeyUp);
            PreviewKeyDown += new KeyEventHandler(SharpRichTextBox_PreviewKeyDown);

            // need to handle the paste command 
            var binding = new CommandBinding();
            binding.Command = ApplicationCommands.Paste;
            binding.PreviewCanExecute += binding_PreviewCanExecute;
            binding.PreviewExecuted += binding_PreviewExecuted;
            binding.Executed += binding_Executed;

            CommandBindings.Add(binding);

            // SetUpBindingForBackground();
        }

        /// <summary>
        /// Used to indicate if the RichTextBox is in valid state
        /// </summary>
        public bool IsValid
        {
            get { return CharactersRemaining <= MaxCharactersAllowed; }

        }

        void binding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        /// <summary>
        /// Make sure that the pasted text is correct
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void binding_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (!Clipboard.ContainsText()) return;

            var clipboardText = Clipboard.GetText();

            if (clipboardText.Length <= CharactersRemaining)
            {
                AppendText(clipboardText);
            }
            else
            {
                AppendText(clipboardText.Substring(0, CharactersRemaining));
            }

            e.Handled = true;

            // update the display again! 
            UpdateRemainingCharactersDisplay();
        }

        /// <summary>
        /// Make sure that the user can paste text. If limit has reached then this operation is cancelled. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void binding_PreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (GetNumberOfCharactersRemaining() <= 0)
            {
                e.CanExecute = false;
                e.Handled = true;
                return;
            }

            e.CanExecute = true;
            e.Handled = false;
        }

        /// <summary>
        /// The default style of the control used to display the counter value
        /// </summary>
        public string DefaultNotificationStyleName
        {
            get { return (String)GetValue(DefaultNotificationStyleNameProperty); }
            set { SetValue(DefaultNotificationStyleNameProperty, value); }
        }

        void SharpRichTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.IsArrowKey() || e.Key.IsBackKey() || e.Key.IsDeleteKey())
            {
                e.Handled = false;
                return;
            }

            if (GetNumberOfCharactersRemaining() <= 0)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// gets the number of characters that can be typed. 
        /// </summary>
        /// <returns></returns>
        private int GetNumberOfCharactersRemaining()
        {
            return (MaxCharactersAllowed - GetTextLengthWithoutNewLine());
        }

        /// <summary>
        /// get the length of the characters without the line
        /// </summary>
        /// <returns></returns>
        private int GetTextLengthWithoutNewLine()
        {
            var textRange = new TextRange(Document.ContentStart, Document.ContentEnd);
            var text = textRange.Text.Replace("\r\n", String.Empty);
            return text.Length;
        }

        void SharpRichTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            UpdateRemainingCharactersDisplay();
            e.Handled = true;
        }

        /// <summary>
        /// update the remaining character display
        /// </summary>
        private void UpdateRemainingCharactersDisplay()
        {
            var noOfCharactersEntered = GetTextLengthWithoutNewLine();

            var noOfCharactersRemaining = GetNumberOfCharactersRemaining();

            if (noOfCharactersRemaining < 0)
            {
                return;
            }

            if ((MaxCharactersAllowed - noOfCharactersEntered) <= NotifyLimit)
            {
                NotificationStyle = FindResource(NotificationStyleName) as Style;
            }
            else
            {
                NotificationStyle = FindResource(DefaultNotificationStyleName) as Style;
            }

            CharactersRemaining = MaxCharactersAllowed - noOfCharactersEntered;
        }

        /// <summary>
        /// style used to notify the user about few characters remaining
        /// </summary>
        public Style NotificationStyle
        {
            get { return (Style)GetValue(NotificationStyleProperty); }
            set
            {
                SetValue(NotificationStyleProperty, value);
                OnPropertyChanged("NotificationStyle");
            }
        }

        /// <summary>
        /// the name of the style used to notify the user that few characters are renaming 
        /// </summary>
        public String NotificationStyleName
        {
            get { return (String)GetValue(NotificationStyleNameProperty); }
            set { SetValue(NotificationStyleNameProperty, value); }
        }

        /// <summary>
        /// this is used to indicate that after how many remaining characters apply the notificationstyle
        /// </summary>
        public int NotifyLimit
        {
            get { return (int)GetValue(NotiftyLimitProperty); }
            set { SetValue(NotiftyLimitProperty, value); }
        }

        /// <summary>
        /// no of characters allowed in the textbox 
        /// </summary>
        public int MaxCharactersAllowed
        {
            get { return (int)GetValue(MaxCharactersAllowedProperty); }
            set { SetValue(MaxCharactersAllowedProperty, value); }
        }

        /// <summary>
        /// number of characters that can be typed by the user in the textbox
        /// </summary>
        public int CharactersRemaining
        {
            get
            {
                return (int)GetValue(CharactersRemainingProperty);
            }

            set
            {
                SetValue(CharactersRemainingProperty, value);
                OnPropertyChanged("CharactersEntered");
            }
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public static class ExtensionMethods
    {
        public static bool IsBackKey(this Key keyPressed)
        {
            if (keyPressed.ToString().ToLower() == "back")
                return true;

            return false;
        }

        public static bool IsDeleteKey(this Key keyPressed)
        {
            if (keyPressed.ToString().ToLower() == "delete")
                return true;
            return false;

        }

        public static bool IsArrowKey(this Key keyPressed)
        {
            if (keyPressed.ToString().ToLower() == "left")
                return true;

            if (keyPressed.ToString().ToLower() == "right")
                return true;

            if (keyPressed.ToString().ToLower() == "up")
                return true;

            if (keyPressed.ToString().ToLower() == "down")
                return true;

            return false;
        }
    }
    public class RichTextBoxHelper : DependencyObject
    {
        private static HashSet<Thread> _recursionProtection = new HashSet<Thread>();

        public static string GetDocumentXaml(DependencyObject obj)
        {
            return (string)obj.GetValue(DocumentXamlProperty);
        }

        public static void SetDocumentXaml(DependencyObject obj, string value)
        {
            _recursionProtection.Add(Thread.CurrentThread);
            obj.SetValue(DocumentXamlProperty, value);
            _recursionProtection.Remove(Thread.CurrentThread);
        }

        public static readonly DependencyProperty DocumentXamlProperty = DependencyProperty.RegisterAttached(
            "DocumentXaml",
            typeof(string),
            typeof(RichTextBoxHelper),
            new FrameworkPropertyMetadata(
                "",
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (obj, e) =>
                {
                    if (_recursionProtection.Contains(Thread.CurrentThread))
                        return;

                    var richTextBox = (RichTextBox)obj;

                    // Parse the XAML to a document (or use XamlReader.Parse())

                    try
                    {
                        var stream = new MemoryStream(Encoding.UTF8.GetBytes(GetDocumentXaml(richTextBox)));
                        var doc = (FlowDocument)XamlReader.Load(stream);

                        // Set the document
                        richTextBox.Document = doc;
                    }
                    catch (Exception)
                    {
                        richTextBox.Document = new FlowDocument();
                    }

                    // When the document changes update the source
                    richTextBox.TextChanged += (obj2, e2) =>
                    {
                        RichTextBox richTextBox2 = obj2 as RichTextBox;
                        if (richTextBox2 != null)
                        {
                            SetDocumentXaml(richTextBox, XamlWriter.Save(richTextBox2.Document));
                        }
                    };
                }
            )
        );
    }
}
