﻿using BandydosMobile.Repository;

namespace BandydosMobile;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
