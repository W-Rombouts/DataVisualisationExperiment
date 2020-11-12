using System.Collections.Generic;

//-----------------------------------------------------------------------------
// Copyright 2012-2020 RenderHeads Ltd.  All rights reserved.
//-----------------------------------------------------------------------------

namespace RenderHeads.Media.AVProMovieCapture
{
	/// Allows the user to monitor a capture that has completed, but the file is still being written to asynchronously
	public class FileWritingHandler : System.IDisposable
	{
		private string _path;
		private int _handle;

		public string Path
		{
			get { return _path; }
		}

		internal FileWritingHandler(string path, int handle)
		{
			_path = path;
			_handle = handle;
		}

		public bool IsFileReady()
		{
			bool result = true;
			if (_handle >= 0)
			{
				result = NativePlugin.IsFileWritingComplete(_handle);
				if (result)
				{
					Dispose();
				}
			}
			return result;
		}

		public void Dispose()
		{
			if (_handle >= 0)
			{
				NativePlugin.FreeRecorder(_handle);
				_handle = -1;

				// Issue the free resources plugin event
				NativePlugin.RenderThreadEvent(NativePlugin.PluginEvent.FreeResources, -1);
			}
		}

		// Helper method for cleaning up a list
		// TODO: add an optional System.Action callback for each time the file writer completes
		public static bool Cleanup(List<FileWritingHandler> list)
		{
			bool anyRemoved = false;
			// NOTE: We iterate in reverse order as we're removing elements from the list
			for (int i = list.Count - 1; i >= 0; i--)
			{
				FileWritingHandler handler = list[i];
				if (handler.IsFileReady())
				{
					list.RemoveAt(i);
					anyRemoved = true;
				}
			}
			return anyRemoved;
		}
	}
}