﻿using Filmster.Views;
using System;
using System.Linq;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Filmster.Services
{
    public static class NavigationService
    {
        public static event NavigatedEventHandler Navigated;

        public static event NavigationFailedEventHandler NavigationFailed;

        private static Frame _frame;

        public static Frame Frame
        {
            get
            {
                if (_frame == null)
                {
                    _frame = Window.Current.Content as Frame;
                    RegisterFrameEvents();
                }

                return _frame;
            }

            set
            {
                UnregisterFrameEvents();
                _frame = value;
                RegisterFrameEvents();
            }
        }

        public static bool CanGoBack => Frame.CanGoBack;

        public static bool CanGoForward => Frame.CanGoForward;

        public static bool GoBack()
        {
            if (CanGoBack)
            {
                Frame.GoBack();
                return true;
            }

            return false;
        }

        public static void GoForward() => Frame.GoForward();

        public static bool Reload(object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            var navigationResult = Navigate(Frame.CurrentSourcePageType, parameter ?? true, infoOverride);
            if (navigationResult)
            {
                Frame.BackStack.Remove(Frame.BackStack.Last());
            }
            return navigationResult;
        }

        public static bool Navigate(Type pageType, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            if (pageType == null || !pageType.IsSubclassOf(typeof(Page)))
            {
                throw new ArgumentException($"Invalid pageType '{pageType}', please provide a valid pageType.", nameof(pageType));
            }

            return
                (Frame.Content?.GetType() != pageType || (parameter != null)) &&
                Frame.Navigate(pageType, parameter, infoOverride);
        }

        public static bool Navigate<T>(object parameter = null, NavigationTransitionInfo infoOverride = null)
            where T : Page
            => Navigate(typeof(T), parameter, infoOverride);

        public static void NavigateSearchBase(SearchBase searchBase)
        {
            switch (searchBase.MediaType)
            {
                case MediaType.Movie:
                    Navigate<MovieDetailPage>(searchBase.Id);
                    break;
                case MediaType.Tv:
                    Navigate<TvShowDetailPage>(searchBase.Id);
                    break;
                case MediaType.Person:
                    Navigate<PersonDetailPage>(searchBase.Id);
                    break;
                default:
                    break;
            }
        }

        private static void RegisterFrameEvents()
        {
            if (_frame != null)
            {
                _frame.Navigated += Frame_Navigated;
                _frame.NavigationFailed += Frame_NavigationFailed;
            }
        }

        private static void UnregisterFrameEvents()
        {
            if (_frame != null)
            {
                _frame.Navigated -= Frame_Navigated;
                _frame.NavigationFailed -= Frame_NavigationFailed;
            }
        }

        private static void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e) => NavigationFailed?.Invoke(sender, e);

        private static void Frame_Navigated(object sender, NavigationEventArgs e) => Navigated?.Invoke(sender, e);
    }
}
