using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using AdminApp.View;
using SharedClasses;
using System.Windows.Input;

namespace AdminApp
{
    /// <summary>
    /// Interaction logic for AdminMainWindow.xaml
    /// </summary> 

    #region IValueConverter

    public class NegativeToNullConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            if ((value as double?) < 0)
            {
                return " ";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class IndexToNumberConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            if (value is int)
            {
                var v = (int)value;
                v++;
                return v;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class BoolToOppositeBoolConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }
            return value;
        }

        #endregion
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            if (!(value is bool)) return value;
            if ((bool)value)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public class BoolToVisibilityConverterInverted : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            if (!(value is bool)) return value;
            if ((bool)value)
            {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    #endregion

    public partial class AdminMainWindow : IAdminMainWindow
    {
        #region IAdminMainWindow members

        public event DelegateAutoGotoNext eventAutoGotoNext;

        public event DelegateFetchCompetitions eventFetchCompetitions;
        public event DelegateFetchContestants eventFetchContestants;
        public event DelegateFetchJudges eventFetchJudges;
        public event DelegateFetchJumpCodes eventFetchJumpCodes;

        public event DelegateStartCompetition eventStartCompetition;
        public event DelegateStopCompetition eventStopCompetition;
        public event DelegateChangeRoundCount eventChangeRoundCount;
        public event DelegateNewCompetition eventNewCompetition;
        public event DelegateGotoNextJump eventGotoNextJump;
        public event DelegateEditCompetition eventEditCompetition;
        public event DelegateDeleteCompetition eventDeleteCompetition;

        public event DelegateNewContestant eventNewContestant;
        public event DelegateEditContestant eventEditContestant;
        public event DelegateDeleteContestant eventDeleteContestant;
        public event DelegateAddContestant eventAddContestant;
        public event DelegateRemoveContestant eventRemoveSelectedContestant;

        public event DelegateNewJudge eventNewJudge;
        public event DelegateEditJudge eventEditJudge;
        public event DelegateDeleteJudge eventDeleteJudge;
        public event DelegateAddJudge eventAddJudge;
        public event DelegateRemoveJudge eventRemoveSelectedJudge;

        public event DelegateNewJump eventNewJump;
        public event DelegateEditJump eventEditJump;
        public event DelegateDeleteJump eventDeleteJump;

        #endregion

        private List<TabItem> _tabItems = new List<TabItem>();
        public AdminMainWindow()
        {
            Admin.loggingInit();
            InitializeComponent();

            foreach (TabItem tab in tabControl.Items)
            {
                var newtab1 = new TabItem
                {
                    Content = tab.Content,
                    Header = tab.Header,
                    Name = tab.Name
                };
                _tabItems.Add(newtab1);
            }
            var newtab = new TabItem
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Visibility = Visibility.Collapsed
            };
            DockPanel.SetDock(newtab, Dock.Right);
            _tabItems.Add(newtab);

            tabControlDynamic.DataContext = _tabItems;
            tabControlDynamic.SelectedIndex = 0;

            tabControl.Visibility = Visibility.Collapsed;
            tabControlDynamic.Visibility = Visibility.Visible;
        }

        private TabItem newTab(int id)
        {
            var newTab = new TabItem
            {
                ContentTemplate = TryFindResource("TabItemContent") as DataTemplate,
                HeaderTemplate = tabControlDynamic.TryFindResource("TabHeader") as DataTemplate,
                HorizontalAlignment = HorizontalAlignment.Right,
                Style = TryFindResource("RightTabs") as Style,
                Name = (id != -1 ? "tab_competition" + id : "tab_competitionNew")
            };
            return newTab;
        }

        private void closeTab(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            var tabName = (sender as Button).CommandParameter.ToString();
            var tab = tabControlDynamic.Items.Cast<TabItem>().SingleOrDefault(i => i.Name.Equals(tabName));
            if (((Competition) tab.Content).started)
            {
                showMessageBox("Den här tävlingen körs, avlusta den först.");
                return;
            }

            if (tab != null)
            {
                var selectedTab = tabControlDynamic.SelectedItem as TabItem;
                var selectedIndex = tabControlDynamic.SelectedIndex;
                tabControlDynamic.DataContext = null;
                _tabItems.Remove(tab);
                tabControlDynamic.DataContext = _tabItems;

                if (selectedTab != null && selectedTab.Equals(tab))
                {
                    var count = _tabItems.Count;
                    selectedTab = count > 5 ? _tabItems[count-1] : _tabItems[1];
                }
                tabControlDynamic.SelectedItem = selectedTab;
            }
        }
        
        void IAdminMainWindow.setCompetitionsList(BindingList<Competition> competitions)
        {
            lv_competitions.ItemsSource = competitions;
        }

        void IAdminMainWindow.setContestantsList(BindingList<Contestant> contestants)
        {
            lv_contestants.ItemsSource = contestants;
        }

        void IAdminMainWindow.setJudgesList(BindingList<Judge> judges)
        {
            lv_judges.ItemsSource = judges;
        }

        #region competition
        private void btn_newCompetition_Click(object sender, RoutedEventArgs e)
        {
            eventNewCompetition?.Invoke();
        }

        private void editCompetition()
        {
            if (lv_competitions.SelectedIndex == -1) return;

            var c = (Competition)lv_competitions.SelectedItem;

            if (_tabItems.Exists(i => i.Name == "tab_competition" + c.id.ToString()))
            {
                tabControlDynamic.SelectedItem = tabControlDynamic.Items.OfType<TabItem>().SingleOrDefault(n => n.Name == "tab_competition" + c.id.ToString());
            }
            else
            {
                c.loadFromDatabase();
                tabControlDynamic.DataContext = null;
                var tab = newTab(c.id);
                var b = new Binding
                {
                    Path = new PropertyPath("name"),
                    Source = c,
                    Mode = BindingMode.TwoWay
                };
                BindingOperations.SetBinding(tab, HeaderedContentControl.HeaderProperty, b);

                var b1 = new Binding
                {
                    Path = new PropertyPath("started"),
                    Source = c,
                    Mode = BindingMode.OneWay
                };
                BindingOperations.SetBinding(tab, TagProperty, b1);

                tab.Content = c;
                DockPanel.SetDock(tab, Dock.Right);
                _tabItems.Add(tab);
                tabControlDynamic.DataContext = _tabItems;
                tabControlDynamic.SelectedItem = tab;

            }
        }
        private void btn_editCompetition_Click(object sender, RoutedEventArgs e)
        {
            editCompetition();
        }

        private void btn_deleteCompetition_Click(object sender, RoutedEventArgs e)
        {
            eventDeleteCompetition?.Invoke((Competition)lv_competitions.SelectedItem);
        }
        
        private void btn_StartCompetition_Click(object sender, RoutedEventArgs e)
        {
            var competition = (Competition)tabControlDynamic.SelectedContent;
            eventStartCompetition?.Invoke(competition);
        }

        private void btn_StopCompetition_Click(object sender, RoutedEventArgs e)
        {
            eventStopCompetition?.Invoke();
        }
        #endregion

        #region judge
        private void btn_newJudge_Click(object sender, RoutedEventArgs e)
        {
            var judge = editJudge(new Judge());
            if (judge != null)
            {
            eventNewJudge?.Invoke(judge);
        }
        }

        private void btn_addJudge_Click(object sender, RoutedEventArgs e)
        {
            var competition = (Competition)tabControlDynamic.SelectedContent;
            var dialog = new SelectPersonWindow(lv_judges.ItemsSource)
            {
                ShowInTaskbar = false,
                Title = "Lägg till domare",
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };

            if (dialog.ShowDialog() == true)
            {
                var judge = (Judge) dialog.listBox.SelectedItem;
                eventAddJudge?.Invoke(judge, competition);
            }
        }

        private void btn_removeJudge_Click(object sender, RoutedEventArgs e)
        {
            var competition = (Competition)tabControlDynamic.SelectedContent;
            eventRemoveSelectedJudge?.Invoke(competition);
        }

        private void btn_deleteJudge_Click(object sender, RoutedEventArgs e)
        {
            eventDeleteJudge?.Invoke(lv_judges.SelectedItem as Judge);
        }

        private void btn_editJudge_Click(object sender, RoutedEventArgs e)
        {
            var judge = editJudge(lv_judges.SelectedItem as Judge);
            if(judge != null)
            {
            eventEditJudge?.Invoke(judge);
        }
        }

        private Judge editJudge(Judge judge)
        {
            if (judge == null) return null;
            var dialog = new EditJudgeWindow
            {
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                Title = (judge.id == -1 ? "Ny domare" : "Redigera domare"),
                name = judge.name,
                nationality = judge.nationality
            };

            if (dialog.ShowDialog() == true)
            {
                judge.name = dialog.name;
                judge.nationality = dialog.nationality;
                return judge;         
            }
            return null;
        }

        private void lv_judges_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var judge = lv_judges.SelectedItem as Judge;
                editJudge(judge);
            }
        }
        #endregion

        #region contestant
        private void btn_newContestant_Click(object sender, RoutedEventArgs e)
        {
            var cont = editContestant(new Contestant());
            if (cont != null)
            {
            eventNewContestant?.Invoke(cont);
        }
        }

        private void btn_addContestant_Click(object sender, RoutedEventArgs e)
        {
            var competition = (Competition)tabControlDynamic.SelectedContent;
            var dialog = new SelectPersonWindow(lv_contestants.ItemsSource)
            {
                ShowInTaskbar = false,
                Title = "Lägg till deltagare",
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };
            if (competition.syncro)
            {
                dialog.Width *= 2;
            }
            
            if (dialog.ShowDialog() == true)
            {
                var contestant = (Contestant)dialog.listBox.SelectedItem;
                eventAddContestant?.Invoke(contestant, competition);
            }
        }

        private void btn_removeContestant_Click(object sender, RoutedEventArgs e)
        {
            var competition = (Competition)tabControlDynamic.SelectedContent;
            eventRemoveSelectedContestant?.Invoke(competition);
        }

        private Contestant editContestant(Contestant contestant)
        {
            if (contestant == null) return null;
            var dialog = new EditContestantWindow
            {
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                Title = (contestant.id == -1 ? "Ny tävlande" : "Redigera Tävlande"),
                name = contestant.name,
                club = contestant.club,
                nationality = contestant.nationality,
                gender = contestant.gender,
                birthdate = contestant.birthdate
            };

            if (dialog.ShowDialog() == true)
            {
                contestant.name = dialog.name;
                contestant.club = dialog.club;
                contestant.nationality = dialog.nationality;
                contestant.gender = dialog.gender;
                contestant.birthdate = dialog.birthdate;
                return contestant;
            }
            return null;
        }

        private void btn_edit_contestant_Click(object sender, RoutedEventArgs e)
        {
            var cont = editContestant(lv_contestants.SelectedItem as Contestant);
            if (cont != null)
            {
                eventEditContestant?.Invoke(cont);
            }
        }

        private void btn_deleteContestant_Click(object sender, RoutedEventArgs e)
        {
            eventDeleteContestant?.Invoke(lv_contestants.SelectedItem as Contestant);
        }

        private void lv_contestants_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var cont = lv_contestants.SelectedItem as Contestant;
                editContestant(cont);
            }
        }
        #endregion

        #region jump
        private void btn_editJump_Click(object sender, RoutedEventArgs e)
        {
            var competition = (Competition)tabControlDynamic.SelectedContent;
            if(competition.selectedJump == null || competition.finished) return;
            var dialog = new JumpWindow
            {
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                height = competition.selectedJump.height,
                jumpcode = competition.selectedJump.jumpCode
            };

            if (dialog.ShowDialog() == true)
            {
                var j = new Jump(-1,dialog.height,false,dialog.jumpcode);
                eventEditJump?.Invoke(j, competition);
            }
        }

        private void btn_nextJump_Click(object sender, RoutedEventArgs e)
        {
            var competition = (Competition)tabControlDynamic.SelectedContent;
            eventGotoNextJump?.Invoke(competition);
        }

        private void cb_autoGotoNext_OnClick(object sender, RoutedEventArgs e)
        {
            var isChecked = (((CheckBox) sender).IsChecked == true);
            eventAutoGotoNext?.Invoke(isChecked);
        }
        #endregion

        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            eventFetchCompetitions?.Invoke();
            eventFetchJumpCodes?.Invoke();
            eventFetchContestants?.Invoke();
            eventFetchJudges?.Invoke();
        }

        private void btn_decreaseRounds_OnClick(object sender, RoutedEventArgs e)
        {
            var competition = (Competition)tabControlDynamic.SelectedContent;
            eventChangeRoundCount?.Invoke(competition,-1);
        }

        private void btn_increaseRounds_OnClick(object sender, RoutedEventArgs e)
        {
            var competition = (Competition)tabControlDynamic.SelectedContent;
            eventChangeRoundCount?.Invoke(competition, 1);
        }

        private void lv_editCompetitions_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                editCompetition();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            eventStopCompetition?.Invoke();
        }

        public void showMessageBox(string message, string header = "")
        {
            MessageBox.Show(message,header,MessageBoxButton.OK,MessageBoxImage.Information);
        }

        private void lv_commingJumps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lv = (ListView) sender;
            lv.ScrollIntoView(lv.SelectedItem);
        }
    }
}
