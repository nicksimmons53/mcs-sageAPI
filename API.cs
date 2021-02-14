using System;
using System.Diagnostics;

public class API
{
	public void initializeAPI(Sage.SMB.API.IMBXML gobjMBAPI)
	{
		int initialization = gobjMBAPI.IntializeAPI();

		if (initialization != 0)
		{
			//eventLog.WriteEntry("Failed to Initialize API.");
			this.deinitializeAPI(gobjMBAPI);
			return;
		}

		return;
	}

	public void setDataSource(Sage.SMB.API.IMBXML gobjMBAPI)
	{
		int setSource = gobjMBAPI.SetDataSource("DESKTOP-2RNU0D2\\SQLEXPRESS");

		if (setSource != 0)
		{
			//eventLog.WriteEntry("Failed to Set Data Source - Error Code: " + setSource);
			this.deinitializeAPI(gobjMBAPI);
			return;
		}

		return;
	}

	public void enableRequests(Sage.SMB.API.IMBXML gobjMBAPI)
	{
		int enableRequests = gobjMBAPI.EnableRequests();

		if (enableRequests != 0)
		{
			//eventLog.WriteEntry("Could not Enable Requests.");
			this.deinitializeAPI(gobjMBAPI);
			return;
		}

		return;
	}

	public string submit(Sage.SMB.API.IMBXML gobjMBAPI, String xml)
	{
		string submitXML = gobjMBAPI.submitXML(xml, "password");

		if (submitXML.Length == 0)
		{
			//eventLog.WriteEntry("Invalid XML.");
			this.deinitializeAPI(gobjMBAPI);
			return null;
		}

		return submitXML;
	}

	public void disableRequests(Sage.SMB.API.IMBXML gobjMBAPI)
	{
		gobjMBAPI.DisableRequests();

		return;
	}

	public Sage.SMB.API.IMBXML deinitializeAPI(Sage.SMB.API.IMBXML gobjMAPI)
	{
		gobjMAPI.DeIntializeAPI();

		return null;
	}

	public string request(Sage.SMB.API.IMBXML gobjMBAPI, string xmlRq)
	{
		initializeAPI(gobjMBAPI);
		disableRequests(gobjMBAPI);
		setDataSource(gobjMBAPI);
		enableRequests(gobjMBAPI);

		string response = submit(gobjMBAPI, xmlRq);

		disableRequests(gobjMBAPI);
		deinitializeAPI(gobjMBAPI);

		return response;
    }

	public API()
	{
	}
}