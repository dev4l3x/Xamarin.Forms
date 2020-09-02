﻿using System;

namespace Xamarin.Forms
{
	public class ShellNavigatingEventArgs : EventArgs
	{
		public ShellNavigatingEventArgs(ShellNavigationState current, ShellNavigationState target, ShellNavigationSource source, bool canCancel)
		{
			Current = current;
			Target = target;
			Source = source;
			CanCancel = canCancel;
		}

		public ShellNavigationState Current { get; }

		public ShellNavigationState Target { get; }

		public ShellNavigationSource Source { get; }

		public bool CanCancel { get; }

		public bool Cancel()
		{
			if (!CanCancel)
				return false;
			Cancelled = true;
			return true;
		}

		public bool Cancelled 
		{ 
			get => _cancelled || _deferalCount > 0;
			private set => _cancelled = value; 
		}

		public ShellNavigatingDeferral GetDeferral()
		{
			if (!CanCancel)
				return null;

			_deferalCount++;
			return new ShellNavigatingDeferral(DecrementDeferral);
		}

		void DecrementDeferral()
		{
			_deferalCount--;
			if (_deferalCount == 0)
				_callback();
		}

		int _deferalCount;

		internal int DeferalCount => _deferalCount;
		Action _callback;
		private bool _cancelled;

		internal void RegisterDeferalCallback(Action callback)
		{
			_callback = callback;
		}
	}

	public class ShellNavigatingDeferral
	{
		Action _completed;

		public ShellNavigatingDeferral(Action completed)
		{
			_completed = completed;
		}

		public void Complete()
		{
			_completed();
			_completed = null;
		}
	}
}