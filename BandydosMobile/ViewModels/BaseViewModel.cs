using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using BandydosMobile.Models;
using BandydosMobile.Services;

namespace BandydosMobile.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    protected bool isBusy;

    [ObservableProperty]
    protected string title = "Bandydsos IK";
}
