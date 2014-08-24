/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Archive"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Archive.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public static class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<GoalViewModel>();
            SimpleIoc.Default.Register<MyRecordsViewModel>();
            SimpleIoc.Default.Register<RecordsForGoalViewModel>();
            SimpleIoc.Default.Register<CommentsViewModel>();
            SimpleIoc.Default.Register<FollowViewModel>();
            SimpleIoc.Default.Register<FightingCenterViewModel>();
            SimpleIoc.Default.Register<AwesomeViewModel>();
        }

        public static GoalViewModel GoalViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GoalViewModel>();
            }
        }

        public static MyRecordsViewModel MyRecordsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MyRecordsViewModel>();
            }
        }

        public static RecordsForGoalViewModel RecordsForGoalViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RecordsForGoalViewModel>();
            }
        }

        public static CommentsViewModel CommentsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CommentsViewModel>();
            }
        }

        public static FollowViewModel FollowViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<FollowViewModel>();
            }
        }

        public static FightingCenterViewModel FightingCenterViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<FightingCenterViewModel>();
            }
        }

        public static AwesomeViewModel AwesomeViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AwesomeViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}