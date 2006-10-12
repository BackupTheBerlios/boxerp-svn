
using System;

namespace clientlib
{
	
	public interface IResponsiveClient
	{
		void StartUpload();
	
		void Upload();
	
		void StartDownload();
	
		void Download();
	
		void UploadComplete();
	
		void DownloadComplete();
		
		void PopulateGUI();
	}
}
